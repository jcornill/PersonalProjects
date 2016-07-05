using UnityEngine;
using System.Collections;

public class GatherTask : Task
{

	public override Worker activeWorker { get; set; }
	Mineral target;
	Depot nearestDepot;
	public string name;
	bool finished;
	bool launched;

	public GatherTask(Mineral mineral)
	{
		target = mineral;
		name = "Gather task";
		finished = false;
		launched = false;
	}

	public override string GetName()
	{
		return name;
	}

	public override bool IsFinished()
	{
		return finished;
	}

	public override void DoTask()
	{
		if (!launched)
		{
			activeWorker.DoEnumeratorTask(DoingTask());
			launched = true;
		}
	}

	IEnumerator DoingTask()
	{
		while (target.IsAlive() && !finished)
		{
			int temple = 0;
			while (!activeWorker.GetComponent<Units>().MoveToPosition(activeWorker.game.GetFreeWorldPosFromWorldPos(target.transform.position)))
			{
				temple++;
				if (!activeWorker.GetComponent<Units>().foundPath && temple > 3)
				{
					target.SetRecolted(false);
					finished = true;
					yield break;
				}
				else
					yield return null;
			}
			if (!activeWorker.GetComponent<Units>().foundPath)
			{
				target.SetRecolted(false);
				finished = true;
				yield break;
			}
			while (activeWorker.actualStock + activeWorker.mineCount < activeWorker.maxStock && target.IsAlive())
			{
				activeWorker.actualStock += target.CollectMineral(activeWorker.mineCount);
				yield return new WaitForSeconds(activeWorker.mineTime);
			}
			nearestDepot = activeWorker.GetNearestDepot(false);
			int compteur = 0;
			while (nearestDepot == null)
			{
				nearestDepot = activeWorker.GetNearestDepot(false);
				if (compteur > 10)
				{
					finished = true;
					target.SetRecolted(false);
					yield break;
				}
				compteur++;
				yield return new WaitForSeconds(1);
			}
			while (!activeWorker.GetComponent<Units>().MoveToPosition(activeWorker.game.GetFreeWorldPosFromWorldPos(nearestDepot.transform.position)))
			{
				if (!activeWorker.GetComponent<Units>().foundPath && Time.time > activeWorker.GetComponent<Units>().test && activeWorker.GetComponent<Units>().test != -1)
				{
					Debug.Log("Cant find path");
					target.SetRecolted(false);
					finished = true;
					yield break;
				}
				yield return null;
			}
			if (nearestDepot.stockActuel + activeWorker.actualStock > nearestDepot.GetStockMax())
			{
				activeWorker.actualStock -= nearestDepot.GetStockMax() - nearestDepot.stockActuel;
				nearestDepot.stockActuel = nearestDepot.GetStockMax();
				finished = true;
				target.SetRecolted(false);
				yield break;
			}
			else
			{
				nearestDepot.stockActuel += activeWorker.actualStock;
				activeWorker.actualStock = 0;
			}
			yield return null;
		}
		finished = true;
	}
}
