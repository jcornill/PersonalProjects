using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildController : MonoBehaviour {

	public Camera camera;

	public Transform ghostBuilding;
	public Building ghostBuildingStatus;

	Grid grid;
	GameController game;
	TaskManager taskManager;

	Node previousNode;

	public Transform[] listBuilding;

	// Use this for initialization
	void Start () {
		game = GameObject.Find("GameController").GetComponent<GameController>();
		ghostBuildingStatus = ghostBuilding.GetComponent<Building>();
		grid = game.maps;
		taskManager = game.GetTaskManager();
	}
	
	// Update is called once per frame
	void Update () {
		if (ghostBuilding)
		{
			Vector3 pos = Input.mousePosition;
			pos.z = camera.transform.position.y;
			pos = camera.ScreenToWorldPoint(pos);
			pos.y = 0;
			pos.x = (int)pos.x;
			pos.z = (int)pos.z;
			if (ghostBuildingStatus.width == 1)
				pos.x += 0.5f;
			if (ghostBuildingStatus.height == 1)
				pos.z += 0.5f;
			ghostBuilding.position = pos;
			if (Input.GetMouseButtonDown(0))
			{
				if (CheckNode())
				{
					PlaceBuilding();
					ghostBuilding = null;
				}
			}
		}
		if (game.gameStarted)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				if (ghostBuilding)
					Destroy(ghostBuilding.gameObject);
				ghostBuildingStatus = null;
				ghostBuilding = Instantiate(listBuilding[0], new Vector3(0, -5, 0), Quaternion.identity) as Transform;
				ghostBuildingStatus = ghostBuilding.GetComponent<Building>();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				if (ghostBuilding)
					Destroy(ghostBuilding.gameObject);
				ghostBuildingStatus = null;
				ghostBuilding = Instantiate(listBuilding[1], new Vector3(0, -5, 0), Quaternion.identity) as Transform;
				ghostBuildingStatus = ghostBuilding.GetComponent<Building>();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				if (ghostBuilding)
					Destroy(ghostBuilding.gameObject);
				ghostBuildingStatus = null;
			}
			if (Input.GetKeyDown(KeyCode.W))
			{
				Depot mainBuilding = GameObject.Find("Main Building").GetComponent<Depot>();
				int cost = mainBuilding.GetComponent<Mainbuilding>().workerCost;
				int idWorker = mainBuilding.GetComponent<Mainbuilding>().nombreWorkers;
				if (mainBuilding.stockActuel >= cost)
				{
					mainBuilding.stockActuel -= cost;
					idWorker++;
					Transform go = Instantiate(mainBuilding.GetComponent<Mainbuilding>().worker, game.GetFreeWorldPosFromWorldPos(mainBuilding.transform.position), Quaternion.identity) as Transform;
					go.name = go.name.Replace("(Clone)", idWorker.ToString());
					game.GetComponent<PlayerController>().taskManager.AddWorkerToTaskList(go.GetComponent<Worker>());
					mainBuilding.GetComponent<Mainbuilding>().nombreWorkers = idWorker;
				}
			}
		}
	}

	bool CheckNode()
	{
		Vector3 worldBottomLeft = ghostBuilding.position - Vector3.right * ghostBuildingStatus.width / 2 - Vector3.forward * ghostBuildingStatus.height / 2;
		List<Node> nodes = new List<Node>();

		for (int x = 0; x < ghostBuildingStatus.width; x++)
		{
			for (int y = 0; y < ghostBuildingStatus.height; y++)
			{
				float nodeDiameter = grid.nodeRadius * 2;
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + grid.nodeRadius) + Vector3.forward * (y * nodeDiameter + grid.nodeRadius);
				previousNode = grid.NodeFromWorldPoint(worldPoint);
				nodes.Add(previousNode);
			}
		}
		int compteur = 0;
		foreach(Node n in nodes)
		{
			if (n.walkable)
			{
				compteur++;
			}
		}
		if (compteur == ghostBuildingStatus.width * ghostBuildingStatus.height)
		{
			return true;
		}
		return false;
	}

	void PlaceBuilding()
	{
		game.UpdateGrid();
		if (!game.gameStarted)
		{
			GameObject.Find("Text Above Screen").SetActive(false);
			game.gameStarted = true;
		}
		if (ghostBuildingStatus.cost > 0)
		{
			BuildingTask newTask = new BuildingTask(ghostBuildingStatus);
			taskManager.AddTask(newTask);
		}
	}
}
