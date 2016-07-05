using UnityEngine;
using System.Collections;

public class DamageableEntities : MonoBehaviour {

	public float maxLife;
	public float life;
	public Transform lifeUI;

	Camera cam;
	Transform lifeBar;
	void Start()
	{
		print("UI");
		cam = Camera.main;
		lifeBar = Instantiate(lifeUI, Vector3.zero, Quaternion.identity) as Transform;
		lifeBar.SetParent(transform);
		lifeBar.transform.localPosition = Vector3.zero;
		lifeBar.transform.localScale = Vector3.one;
		UpdateLifeBar();
		if (life == maxLife)
		{
			lifeBar.gameObject.SetActive(false);
		}
	}

	public void Damage(float degats)
	{
		life -= degats;
		UpdateLifeBar();
		if (life < 0)
		{
			life = 0;
		}
		else if (!lifeBar.gameObject.activeSelf)
		{
			lifeBar.gameObject.SetActive(true);
		}
	}

	public void Heal(float lifeHealed)
	{
		life += lifeHealed;
		UpdateLifeBar();
		if (life > maxLife)
		{
			lifeBar.gameObject.SetActive(false);
			life = maxLife;
		}
	}

	public void UpdateLifeBar()
	{
		float scaleX = 1 / maxLife * life;
		if (scaleX > 1)
		{
			scaleX = 1;
		}
		lifeBar.GetChild(1).transform.localScale = new Vector3(scaleX, 1, 1);
	}

	public bool IsAlive()
	{
		return life > 0;
	}

}
