using UnityEngine;
using System.Collections;

public class Task {
	bool finished;
	public virtual Worker activeWorker { get; set; }
	public int id { get; set; }
	public virtual bool IsFinished()
	{
		return finished;
	}

	public virtual string GetName()
	{
		return null;
	}

	public virtual void DoTask()
	{

	}

}
