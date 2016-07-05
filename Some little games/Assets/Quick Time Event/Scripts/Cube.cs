using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

	public KeyCode key;
	public float speedMin;
	public float speedMax;
	private float speed;

	// Use this for initialization
	void Start () {
		speed = Random.Range(speedMin, speedMax);
		GameObject.Destroy(gameObject, 10);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(0, -(speed / 60), 0));
		if (transform.localPosition.y <= -3.5f && transform.localPosition.y >= -4.5f && Input.GetKeyDown(key))
			GameObject.Destroy(gameObject);
	}
}
