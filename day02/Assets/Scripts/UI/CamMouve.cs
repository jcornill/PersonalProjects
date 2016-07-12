using UnityEngine;
using System.Collections;

public class CamMouve : MonoBehaviour {

	public float speed;

	private Camera cam;

	// Use this for initialization
	void Start () {
		cam = gameObject.GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftArrow)) {
			cam.transform.Translate (-speed, 0, 0);
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			cam.transform.Translate (speed, 0, 0);
		} else if (Input.GetKey (KeyCode.UpArrow)) {
			cam.transform.Translate (0, speed, 0);
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			cam.transform.Translate (0, -speed, 0);
		}
	}
}
