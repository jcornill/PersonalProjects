using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public bool gameStarted {get; set; }
	public GameObject MapsController;
	public Grid maps { get; set; }
	public GameObject helpText;

	private PlayerController player;
	private bool help;

	// Use this for initialization
	void Start ()
	{
		gameStarted = false;
		maps = MapsController.GetComponent<Grid>();
		player = gameObject.GetComponent<PlayerController>();
		help = true;
	}

	public void UpdateGrid()
	{
		maps.CreateGrid();
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyUp(KeyCode.H))
		{
			if (help)
			{
				help = false;
				helpText.SetActive(false);
			}
			else
			{
				helpText.SetActive(true);
				help = true;
			}
		}
	}

	public void SetNodeStatus(Vector3 worldPosition, bool status)
	{
		maps.NodeFromWorldPoint(worldPosition).walkable = status;
	}

	public Vector3 GetFreeWorldPosFromWorldPos(Vector3 worldPosition) 
	{
		return maps.GetWorldFreeNeighboursNodeFromWorldPoint(worldPosition);
	}

	public TaskManager GetTaskManager()
	{
		return player.taskManager;
	}
}
