using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float cameraSpeed;
	public float zoomSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float posX = transform.position.x + Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime;
		float posY = transform.position.y + Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * -zoomSpeed;
		float posZ = transform.position.z + Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime;
		if (posY < 5)
			posY = 5;
		if (posY > 40)
			posY = 40;
		Vector3 pos = new Vector3(posX, posY, posZ);
		transform.position = pos;
		GetComponent<Camera>().orthographicSize = posY - 1;
	}
}
