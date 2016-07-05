using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskManager {
	List<Task> taskList = new List<Task>();
	List<Worker> workerList = new List<Worker>();
	int idTask;

	public TaskManager()
	{
		idTask = 0;
	}

	public void AddWorkerToTaskList(Worker worker)
	{
		workerList.Add(worker);
	}

	public void AddTask(Task task)
	{
		task.id = idTask;
		//Debug.Log("Adding task '" + task.GetName() + "' to task list id " + task.id);
		idTask++;
		taskList.Add(task);
	}

	Worker GetIdleWorker()
	{
		foreach (Worker worker in workerList)
		{
			if (!worker.IsHaveTask())
			{
				//Debug.Log("Finding " + worker.name + " with actual task " + worker.actualTask.GetName());
				return worker;
			}
		}
		return null;
	}

	void AssignTask(Task task, Worker worker)
	{
		//Debug.Log("Task '" + task.GetName() + "' id " + task.id + " assigned to worker " + worker.name);
		taskList.Remove(task);
		task.activeWorker = worker;
		worker.SetTask(task);
	}

	public void ManageTask()
	{
		if (taskList.Count > 0 && GetIdleWorker() != null)
		{
			AssignTask(taskList[0], GetIdleWorker());
		}

	}
}
