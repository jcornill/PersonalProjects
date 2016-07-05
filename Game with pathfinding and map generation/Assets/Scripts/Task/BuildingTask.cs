using UnityEngine;
using System.Collections;

public class BuildingTask : Task
{

	public override Worker activeWorker { get; set; }
	Building target;
	Depot nearestDepot;
	public string name;
	bool finished;
	bool launched;

	float lifeAdder;

	public BuildingTask(Building building)
	{
		target = building;
		name = "Building task";
		finished = false;
		launched = false;
		lifeAdder = 1 / (float)target.cost * target.GetComponent<DamageableEntities>().maxLife;
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
		while (target.GetComponent<DamageableEntities>().IsAlive() && !finished)
		{
			nearestDepot = activeWorker.GetNearestDepot(true);
			while (nearestDepot == null)
			{
				nearestDepot = activeWorker.GetNearestDepot(true);
				yield return new WaitForSeconds(1);
			}
			int temple = 0;
			while (!activeWorker.GetComponent<Units>().MoveToPosition(activeWorker.game.GetFreeWorldPosFromWorldPos(nearestDepot.transform.position)))
			{
				temple++;
				if (!activeWorker.GetComponent<Units>().foundPath && temple > 3)
				{
					target.GetComponent<Building>().deconstruct();
					finished = true;
					yield break;
				}
				else
					yield return null;
			}
			if (nearestDepot.stockActuel - activeWorker.EmptyStock() >= 0 && target.cost >= activeWorker.EmptyStock())
			{
				nearestDepot.stockActuel -= activeWorker.EmptyStock();
				activeWorker.actualStock += activeWorker.EmptyStock();
			}
			else if (nearestDepot.stockActuel - target.cost >= 0 && target.cost < activeWorker.EmptyStock())
			{
				nearestDepot.stockActuel -= target.cost;
				activeWorker.actualStock += target.cost;
			}
			else if (nearestDepot.stockActuel - activeWorker.EmptyStock() < 0 && target.cost >= nearestDepot.stockActuel) 
			{
				activeWorker.actualStock = nearestDepot.stockActuel;
				nearestDepot.stockActuel = 0;
			}
			temple = 0;
			while (!activeWorker.GetComponent<Units>().MoveToPosition(activeWorker.game.GetFreeWorldPosFromWorldPos(target.transform.position)))
			{
				temple++;
				Debug.Log(activeWorker.GetComponent<Units>().foundPath + " " + temple);
				if ((activeWorker.GetComponent<Units>().foundPath && temple == 1))
					yield return new WaitForSeconds(1);
				if (!activeWorker.GetComponent<Units>().foundPath)
				{
					target.GetComponent<Building>().deconstruct();
					finished = true;
					yield break;
				}
				else
					yield return null;
			}
			while(activeWorker.actualStock > 0)
			{
				activeWorker.actualStock--;
				target.cost--;
				target.GetComponent<DamageableEntities>().Heal(lifeAdder);
				if (target.cost <= 0)
				{
					finished = true;
					target.operate = true;
				}
				yield return new WaitForSeconds(activeWorker.buildTime);
			}
			yield return null;
		}
		yield return null;
	}
}
