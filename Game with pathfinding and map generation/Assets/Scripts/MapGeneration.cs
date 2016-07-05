﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGeneration : MonoBehaviour {

	public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;

	[Range(0, 100)]
	public int randomfillPercent;

	[Range(0, 10)]
	public int smoothValue;

	[Range(0, 100)]
	public int lessRoomSize;

	[Range(0, 10)]
	public int tunnelWidth;

	[Range(0, 5)]
	public float randomRockSize;

	public bool connectRoom;

	public Transform wall;
	public Transform spawner;
	public Transform Maps;

	int[,] map;

	void Awake()
	{
		GenerateMap();
		this.gameObject.GetComponent<Grid>().CreateGrid();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			//GenerateMap();
		}
	}
	
	void GenerateMap()
	{
		map = new int[width, height];
		RandomFillMap();

		for (int i = 0; i < smoothValue; i++)
		{
			SmoothMap();
		}
		ProcessMap();
		PlaceWall();
	}

	void ProcessMap()
	{
		List<List<Coord>> wallRegions = GetRegions(1);

		foreach (List<Coord> wallRegion in wallRegions)
		{
			if (wallRegion.Count < lessRoomSize)
			{
				foreach (Coord tile in wallRegion)
				{
					map[tile.tileX, tile.tileY] = 0;
				}
			}
		}

		List<List<Coord>> roomRegions = GetRegions(0);
		List<Room> survivingRooms = new List<Room>();

		foreach (List<Coord> roomRegion in roomRegions)
		{
			if (roomRegion.Count < lessRoomSize)
			{
				foreach (Coord tile in roomRegion)
				{
					map[tile.tileX, tile.tileY] = 1;
				}
			}
			else
			{
				survivingRooms.Add(new Room(roomRegion, map));
			}
		}
		survivingRooms.Sort();
		survivingRooms[0].isMainRoom = true;
		survivingRooms[0].isAccessibleFromMainRoom = true;

		if (connectRoom)
		{
			ConnectClosestRooms(survivingRooms);
		}

		CreateEnnemySpawner(survivingRooms[0]);
	}

	void CreateEnnemySpawner(Room room)
	{
		float randomTile = UnityEngine.Random.Range(0, room.tiles.Count - 1);
		Coord tile = room.tiles[(int)randomTile];

		Vector3 pos = new Vector3(CoordToWorldPoint(tile).x, 0, CoordToWorldPoint(tile).z);
		Transform go = Instantiate(spawner, pos, Quaternion.identity) as Transform;
		go.parent = Maps;
		DrawCircle(tile, 5);
	}

	void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
	{

		List<Room> roomListA = new List<Room>();
		List<Room> roomListB = new List<Room>();

		if (forceAccessibilityFromMainRoom)
		{
			foreach(Room room in allRooms)
			{
				if(room.isAccessibleFromMainRoom)
				{
					roomListB.Add(room);
				}
				else
				{
					roomListA.Add(room);
				}
			}
		}
		else
		{
			roomListA = allRooms;
			roomListB = allRooms;
		}

		int bestDistance = 0;
		Coord bestTileA = new Coord();
		Coord bestTileB = new Coord();
		Room bestRoomA = new Room();
		Room bestRoomB = new Room();
		bool possibleConnectionFound = false;

		foreach (Room roomA in roomListA)
		{
			if (!forceAccessibilityFromMainRoom)
			{
				possibleConnectionFound = false;
				if (roomA.connectedRooms.Count > 0)
				{
					continue;
				}
			}
			foreach (Room roomB in roomListB)
			{
				if (roomA == roomB || roomA.IsConnected(roomB))
				{
					continue;
				}
				for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
				{
					for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
					{
						Coord tileA = roomA.edgeTiles[tileIndexA];
						Coord tileB = roomB.edgeTiles[tileIndexB];
						int distanceBetweenRooms = (int)(Math.Pow(tileA.tileX - tileB.tileX, 2) + Math.Pow(tileA.tileY - tileB.tileY, 2));

						if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
						{
							bestDistance = distanceBetweenRooms;
							possibleConnectionFound = true;
							bestTileA = tileA;
							bestTileB = tileB;
							bestRoomA = roomA;
							bestRoomB = roomB;
						}
					}
				}
			}
			if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
			{
				CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
			}
		}

		if (possibleConnectionFound && forceAccessibilityFromMainRoom)
		{
			CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
			ConnectClosestRooms(allRooms, true);
		}

		if (!forceAccessibilityFromMainRoom)
		{
			ConnectClosestRooms(allRooms, true);
		}
	}

	void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
	{
		Room.ConnectRooms(roomA, roomB);

		List<Coord> line = Getline(tileA, tileB);
		foreach (Coord c in line)
		{
			DrawCircle(c, tunnelWidth);
		}
	}

	void DrawCircle(Coord c, int r)
	{
		for (int x = -r; x <= r; x++)
		{
			for (int y = -r; y <= r; y++)
			{
				if (x*x + y*y <= r*r)
				{
					int drawX = c.tileX + x;
					int drawY = c.tileY + y;
					if (IsInMapRange(drawX, drawY))
					{
						map[drawX, drawY] = 0;
					}
				}
			}
		}
	}

	List<Coord> Getline(Coord from, Coord to)
	{
		List<Coord> line = new List<Coord>();

		int x = from.tileX;
		int y = from.tileY;

		int dx = to.tileX - from.tileX;
		int dy = to.tileY - from.tileY;

		bool inverted = false;
		int step = Math.Sign(dx);
		int gradientStep = Math.Sign(dy);

		int longest = Mathf.Abs(dx);
		int shortest = Mathf.Abs(dy);

		if (longest < shortest)
		{
			inverted = true;
			longest = Mathf.Abs(dy);
			shortest = Mathf.Abs(dx);

			step = Math.Sign(dy);
			gradientStep = Math.Sign(dx);
		}

		int gradientAccumulation = longest / 2;
		for (int i = 0 ; i < longest; i++)
		{
			line.Add(new Coord(x ,y));

			if (inverted)
			{
				y += step;
			}
			else
			{
				x += step;
			}

			gradientAccumulation += shortest;
			if (gradientAccumulation >= longest)
			{
				if (inverted)
				{
					x += gradientStep;
				}
				else
				{
					y += gradientStep;
				}
				gradientAccumulation -= longest;
			}
		}
		return line;
	}

	Vector3 CoordToWorldPoint(Coord tile)
	{
		return new Vector3(-width / 2 + 0.5f + tile.tileX, transform.position.y + 2, -height / 2 + 0.5f + tile.tileY);
	}

	List<List<Coord>> GetRegions(int tileType)
	{
		List<List<Coord>> regions = new List<List<Coord>>();
		int[,] mapFlags = new int[width, height];

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (mapFlags[x, y] == 0 && map[x, y] == tileType)
				{
					List<Coord> newRegion = GetRegionTiles(x, y);
					regions.Add(newRegion);

					foreach (Coord tile in newRegion)
					{
						mapFlags[tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}
		return regions;
	}

	List<Coord> GetRegionTiles(int startX, int startY)
	{
		List<Coord> tiles = new List<Coord>();
		int[,] mapFlags = new int[width, height];
		int tileType = map[startX, startY];

		Queue<Coord> queue = new Queue<Coord>();
		queue.Enqueue(new Coord(startX, startY));
		mapFlags[startX, startY] = 1;

		while (queue.Count > 0)
		{
			Coord tile = queue.Dequeue();
			tiles.Add(tile);

			for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
			{
				for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
				{
					if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
					{
						if (mapFlags[x, y] == 0 && map[x, y] == tileType)
						{
							mapFlags[x, y] = 1;
							queue.Enqueue(new Coord(x, y));
						}
					}
				}
			}
		}
		return tiles;
	}

	bool IsInMapRange(int x, int y)
	{
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	void RandomFillMap()
	{
		if(useRandomSeed)
		{
			seed = UnityEngine.Random.Range(-1000000, 1000000).ToString();
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x++)
		{
			for (int y = 0 ; y < height; y++)
			{
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
				{
					map[x, y] = 1;
				}
				else
				{
					map[x, y] = (pseudoRandom.Next(0, 100) > randomfillPercent) ? 1 : 0;
				}
			}
		}
	}

	void SmoothMap()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int neighbourWall = GetSurroundingWallCount(x, y);

				if (neighbourWall > 4)
				{
					map[x, y] = 1;
				}
				else if (neighbourWall < 4)
				{
					map[x, y] = 0;
				}
			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY)
	{
		int wallCount = 0;

		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
		{
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
			{
				if (IsInMapRange(neighbourX, neighbourY))
				{
					if (neighbourX != gridX || neighbourY != gridY)
					{
						wallCount += map[neighbourX, neighbourY];
					}
				}
				else
				{
					wallCount++;
				}
			}
		}
		return wallCount;
	}

	struct Coord
	{
		public int tileX;
		public int tileY;

		public Coord(int x, int y)
		{
			tileX = x;
			tileY = y;
		}
	}

	void PlaceWall()
	{
		if (map != null)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (map[x, y] == 1)
					{
						Vector3 pos = new Vector3(-width / 2 + x + 0.5f, transform.position.y, -height / 2 + y + 0.5f);
						Transform go = Instantiate(wall, pos, Quaternion.identity) as Transform;
						go.localScale = new Vector3(go.localScale.x, 2 + UnityEngine.Random.Range(0, randomRockSize), go.localScale.z);
						go.parent = Maps;
					}
				}
			}
		}
	}

	class Room : IComparable<Room>
	{
		public List<Coord> tiles;
		public List<Coord> edgeTiles;
		public List<Room> connectedRooms;
		public int roomSize;
		public bool isAccessibleFromMainRoom;
		public bool isMainRoom;

		public Room()
		{
		}

		public Room(List<Coord> roomTiles, int [,] map)
		{
			tiles = roomTiles;
			roomSize = tiles.Count;
			connectedRooms = new List<Room>();

			edgeTiles = new List<Coord>();
			foreach (Coord tile in tiles)
			{
				for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
				{
					for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
					{
						if (x == tile.tileX || y == tile.tileY)
						{
							if (map[x, y] == 1)
							{
								edgeTiles.Add(tile);
							}
						}
					}
				}
			}
		}

		public void SetAccessibleFromMainRoom()
		{
			if (!isAccessibleFromMainRoom)
			{
				isAccessibleFromMainRoom = true;
				foreach(Room connectedRoom in connectedRooms)
				{
					connectedRoom.SetAccessibleFromMainRoom();
				}
			}
		}

		public static void ConnectRooms(Room roomA, Room roomB)
		{
			if (roomA.isAccessibleFromMainRoom)
			{
				roomB.SetAccessibleFromMainRoom();
			}
			else if (roomB.isAccessibleFromMainRoom)
			{
				roomA.SetAccessibleFromMainRoom();
			}
			roomA.connectedRooms.Add(roomB);
			roomB.connectedRooms.Add(roomA);
		}

		public bool IsConnected(Room otherRoom)
		{
			return connectedRooms.Contains(otherRoom);
		}

		public int CompareTo(Room otherRoom)
		{
			return otherRoom.roomSize.CompareTo(roomSize);
		}
	}
}
