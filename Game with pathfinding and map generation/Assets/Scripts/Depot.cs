using UnityEngine;
using System.Collections;

public class Depot : MonoBehaviour {

	public int stockMax;

	public int stockActuel { get; set; }

	public int stock;

	void Update()
	{
		stock = stockActuel;
	}

	public bool IsFull()
	{
		return stockActuel == stockMax;
	}

	public bool IsEmpty()
	{
		return stockActuel == 0;
	}

	public int GetStockMax()
	{
		return stockMax;
	}
}
