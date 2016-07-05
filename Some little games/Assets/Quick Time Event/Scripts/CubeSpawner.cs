using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour {

	public float spawnSpeed;
	public int time;
	public GameObject cubeA;
	public GameObject cubeD;
	public GameObject cubeS;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int rand;
		time++;
		if ((time / 60) >= spawnSpeed)
		{
			rand = Random.Range(1, 4);
			if (rand == 1)
			{
				GameObject.Instantiate(cubeA, new Vector3(-1.2f, 5, -1), Quaternion.identity);
			}
			else if (rand == 2)
			{
				GameObject.Instantiate(cubeS, new Vector3(0, 5, -1), Quaternion.identity);
			}
			else if (rand == 3)
			{
				GameObject.Instantiate(cubeD, new Vector3(1.2f, 5, -1), Quaternion.identity);
			}
			time -= (int)(spawnSpeed * 60);
		}
	}
}
