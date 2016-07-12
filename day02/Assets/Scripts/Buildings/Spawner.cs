using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public float intervalle;
	public GameObject unit;
	public Vector3 pos;

	private float time;
	// Use this for initialization
	void Start () {
		time = Time.time;;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > time + intervalle) {
			GameObject.Instantiate (unit, pos, Quaternion.identity);
			time = Time.time;
		}
	}
}
