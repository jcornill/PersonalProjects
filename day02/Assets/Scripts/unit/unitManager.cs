using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class unitManager : MonoBehaviour {

	public List<GameObject> selectedUnit = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			if (hit.collider != null && hit.collider.gameObject.tag == "Units" && hit.collider.gameObject.GetComponent<Race>().Alliance) {
				hit.collider.gameObject.GetComponent<unitSound> ().playSong ("selectingSound");
				if (!Input.GetKey (KeyCode.LeftControl)) {
					foreach (GameObject go in selectedUnit){
						if (go != null)
							go.GetComponent<Unit> ().selected = false;
					}
					selectedUnit.Clear ();
				}
				if (!selectedUnit.Contains (hit.collider.gameObject)) {
					selectedUnit.Add (hit.collider.gameObject);
					hit.collider.gameObject.GetComponent<Unit> ().selected = true;
				}
			} else {
				foreach (GameObject go in selectedUnit) {
					if (go != null)
						go.GetComponent<Unit> ().selected = false;
				}
				selectedUnit.Clear ();
			}
		} else if (Input.GetMouseButtonDown (1)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			selectedUnit [Random.Range (0, selectedUnit.Count)].GetComponent<unitSound> ().playSong ("orderSound");
			if (hit.collider != null && hit.collider.gameObject.GetComponent<Health> () != null && hit.collider.gameObject.GetComponent<Race> ().Orc) {
				foreach (GameObject go in selectedUnit) {
					go.GetComponent<Unit> ().target = hit.collider.gameObject;
				}
			} else {
				foreach (GameObject go in selectedUnit) {
					go.GetComponent<Unit> ().moveTo (Input.mousePosition);
					go.GetComponent<Unit> ().target = null;
				}
			}
		}
	}
}
