﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
	public bool displayGrid;
	public LayerMask unwakableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
	}

	public void CreateGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwakableMask));
				grid[x, y] = new Node(walkable, worldPoint, x, y);
			}
		}
	}

	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)
					continue;
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
				{
					neighbours.Add(grid[checkX, checkY]);
				}

			}
		}
		return neighbours;
	}

	public Node NodeFromWorldPoint(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.x / 2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return grid[x, y];
	}

	public Vector3 GetWorldFreeNeighboursNodeFromWorldPoint(Vector3 worldPosition)
	{
		foreach(Node node in GetNeighbours(NodeFromWorldPoint(worldPosition)))
		{
			if (node.walkable)
			{
				return node.worldPosition;
			}
		}
		return new Vector3();
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if (grid != null && displayGrid)
		{
			foreach (Node n in grid)
			{
				Gizmos.color = (n.walkable) ? Color.white : Color.red;
				//if (n.nodeColor != Color.white)
				//	Gizmos.color = n.nodeColor;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
			}
		}
	}
	public List<Node> GetNode()
	{
		List<Node> tiles = new List<Node>();
		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				tiles.Add(grid[x, y]);
			}
		}
			return tiles;
	}
}