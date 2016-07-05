using UnityEngine;
using System.Collections;

public class playerScript_ex00 : MonoBehaviour {

	public float speed;
	public float jumpPower;
	public bool selected;
	public GameObject exit;

	// Use this for initialization
	void Start () {
		select (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (selected) {
			Vector3 depl = new Vector3 ();
			if (Input.GetKey (KeyCode.LeftArrow)) {
				depl.x = -(speed / 25);
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				depl.x = (speed / 25);
			} else if (Input.GetKeyDown (KeyCode.Space) && isOnGround ()) {
				gameObject.GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2 (0, jumpPower * 100));
			}
			transform.Translate (depl);
		}
	}

	private bool isOnGround()
	{
		if (gameObject.GetComponent<Rigidbody2D>().velocity.y == 0)
			return true;
		return false;
	}

	public bool onExit()
	{
		if (Vector3.Distance (transform.localPosition, exit.transform.localPosition) < 1) {
			return (true);
		}
		return (false);
	}

	public void select(bool sel)
	{
		if (sel) {
			selected = true;
			//gameObject.GetComponent<Rigidbody2D> ().isKinematic = false;
		} else {
			selected = false;
			//gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;
		}
	}
}
