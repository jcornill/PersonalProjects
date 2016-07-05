using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {
	public string name;
	public int width;
	public int height;
	public int cost;
	public int buildingNumber;
	public bool operate;

	public void deconstruct()
	{
		Destroy(gameObject);
	}
}
