using UnityEngine;
using System.Collections;

public class Mineral : MonoBehaviour {

	public int mutliplierRessourcePerScale;
	float mineralStock;
	float startMineral;
	float startScale;
	bool recolted;

	public Material defaultMaterial;

	GameController game;

	// Use this for initialization
	void Start () {
		mineralStock = (int)(transform.localScale.y * mutliplierRessourcePerScale);
		startMineral = mineralStock;
		startScale = transform.localScale.y;
		game = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	public bool IsAlive()
	{
		return mineralStock > 0;
	}
	
	public int CollectMineral (int quantity) 
	{
		float recolted = quantity;
		mineralStock -= quantity;
		if (mineralStock <= 0)
		{
			recolted = mineralStock * -1;
			mineralStock = 0;
			transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
			game.SetNodeStatus(transform.position, true);
			Destroy(gameObject, 2);
		}
		else
		{
			float newScale = mineralStock / startMineral * startScale;
			transform.localScale = new Vector3(transform.localScale.x, newScale, transform.localScale.z);
		}
		return (int)recolted;
	}

	public void SetRecolted(bool _recolted)
	{
		if (IsAlive())
		{
			recolted = _recolted;
			if (recolted)
			{
				this.GetComponent<Renderer>().material.color = Color.green;
			}
			else
			{
				this.GetComponent<Renderer>().material = defaultMaterial;
			}
		}
	}

	public bool GetRecolted()
	{
		return recolted;
	}
}
