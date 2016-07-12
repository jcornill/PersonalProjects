using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float actualLife;
	public float maxLife;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void inflige(float damage)
	{
		actualLife -= damage;
		if (actualLife <= 0)
			actualLife = 0;
	}

	public bool isDead()
	{
		return (actualLife <= 0);
	}

	public void kill()
	{
		GetComponent<unitSound> ().playSong ("deathSound");
		if (GetComponent<Animator> () != null)
			GetComponent<Animator> ().SetTrigger ("death");
		else
			Destroy (this.gameObject, 3);
	}
}
