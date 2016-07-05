using UnityEngine;
using System.Collections;

public class Ballon : MonoBehaviour {

	public int souffle;
	public int maxSouffle;
	public int souffleUsage;
	public float ballonScale;
	public int looseTime;
	private float lifeTime;
	private bool game;

	// Use this for initialization
	void Start () {
		lifeTime = 0;
		game = true;
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime++;
		if (souffle < maxSouffle)
		{
			souffle++;
		}
		if (Input.GetKeyDown(KeyCode.Space) && game)
		{
			if (souffle >= souffleUsage)
			{
				souffle -= souffleUsage;
				ballonScale += 0.25f;
				transform.localScale = new Vector3(ballonScale, ballonScale, ballonScale);
			}
			else
				souffle = 0;
		}
		if (ballonScale >= 5)
		{
			Debug.Log("Balloon life time: " + Mathf.RoundToInt(lifeTime / 60) + "s");
			GameObject.Destroy(gameObject);
		}
		if (Mathf.RoundToInt(lifeTime / 60) > looseTime && game)
		{
			Debug.Log("Perdu vous n'avez pas gonflee assez vite !");
			game = false;
		}
	}
}
