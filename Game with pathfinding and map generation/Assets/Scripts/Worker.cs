using UnityEngine;
using System.Collections;

public class Worker : MonoBehaviour {

	public int maxStock;
	public int actualStock {get; set; }
	public float mineTime;
	public int mineCount;
	public float buildTime;

	public int stock;

	public Task actualTask;

	public GameController game { get; private set; }

	public string affichageNameTask;
	// Use this for initialization
	void Start () {
		game = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (actualTask != null)
			affichageNameTask = actualTask.GetName();
		else
			affichageNameTask = "null";
		stock = actualStock;
		if (actualTask != null && !actualTask.IsFinished())
		{
			actualTask.DoTask();
		}
		else
		{
			actualTask = null;
		}
	}
	
	public void SetTask(Task task)
	{
		//Debug.Log(transform.name + " has received task " + task.GetName() + " with id " + task.id);
		actualTask = task;
	}

	public int EmptyStock()
	{
		return maxStock - actualStock;
	}

	public bool IsHaveTask()
	{
		return (actualTask != null);
	}

	public Depot GetNearestDepot(bool searchFullDepot)
	{
		float distance = 999999;
		Depot nearestDepot;
		nearestDepot = null;
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Building"))
		{
			Depot dep = go.GetComponent<Depot>();
			if (dep != null && go.GetComponent<Building>().operate)
			{
				if (searchFullDepot && !dep.IsEmpty())
				{
					float newDistance = Mathf.Sqrt(Mathf.Pow(transform.position.x - dep.transform.position.x, 2) + Mathf.Pow(transform.position.z - dep.transform.position.z, 2));
					if (newDistance < distance)
					{
						distance = newDistance;
						nearestDepot = dep;
					}
				}
				else if (!searchFullDepot && !dep.IsFull())
				{
					float newDistance = Mathf.Sqrt(Mathf.Pow(transform.position.x - dep.transform.position.x, 2) + Mathf.Pow(transform.position.z - dep.transform.position.z, 2));
					if (newDistance < distance)
					{
						distance = newDistance;
						nearestDepot = dep;
					}
				}
			}
		}
		return nearestDepot;
	}

	public void DoEnumeratorTask(IEnumerator task)
	{
		StopCoroutine(task);
		StartCoroutine(task);
	}
}
