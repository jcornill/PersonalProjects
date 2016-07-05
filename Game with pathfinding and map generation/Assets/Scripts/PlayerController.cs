using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	GameController game;
	Camera camera;
	public TaskManager taskManager { get; private set; }


	float tempTime = 0.1f;
	float frameTempTime;
	// Use this for initialization
	void Start () {
		frameTempTime = Time.time;
		game = GetComponent<GameController>();
		camera = GameObject.Find("Camera").GetComponent<Camera>();
		taskManager = new TaskManager();
	}
	
	// Update is called once per frame
	void Update () {
		if (game.gameStarted)
		{
			if (Input.GetMouseButton(0) && frameTempTime + tempTime < Time.time)
			{
				Vector3 pos = camera.ScreenToWorldPoint(Input.mousePosition);
				Debug.DrawRay(pos, Vector3.down * 100, Color.red, 10);
				RaycastHit hit;
				if (Physics.Raycast(pos, Vector3.down, out hit))
				{
					Debug.Log("Ray hit : " + hit.collider.name);
					if(hit.collider.gameObject.tag == "Harvestable" && !hit.collider.gameObject.GetComponent<Mineral>().GetRecolted())
					{
						GatherTask newTask = new GatherTask(hit.collider.gameObject.GetComponent<Mineral>());
						hit.collider.gameObject.GetComponent<Mineral>().SetRecolted(true);
						taskManager.AddTask(newTask);
					}
				}
				frameTempTime = Time.time;
			}
		}
		taskManager.ManageTask();
	}

}
