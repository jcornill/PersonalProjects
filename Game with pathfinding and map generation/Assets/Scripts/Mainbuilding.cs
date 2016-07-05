using UnityEngine;
using System.Collections;

public class Mainbuilding : MonoBehaviour {

	public Transform worker;
	public int nombreWorkers;
	public int workerCost;

	GameController game;

	bool workersSpawned;

	// Use this for initialization
	void Start () {
		game = GameObject.Find("GameController").GetComponent<GameController>();
		workersSpawned = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (game.gameStarted && !workersSpawned)
		{
			CreateWorkers();
		}
	}

	void CreateWorkers()
	{
		for (int i = 0; i < nombreWorkers; i++)
		{
			Vector3 spawnPos = game.GetFreeWorldPosFromWorldPos(transform.position);
			Transform go = Instantiate(worker, spawnPos, Quaternion.identity) as Transform;
			go.name = go.name.Replace("(Clone)", i.ToString());
			game.GetComponent<PlayerController>().taskManager.AddWorkerToTaskList(go.GetComponent<Worker>());
		}
		workersSpawned = true;
	}
}
