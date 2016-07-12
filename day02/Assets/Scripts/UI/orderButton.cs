using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class orderButton : MonoBehaviour {

	public string tooltip;

	void OnGUI()
	{
		GUI.Button (new Rect (transform.localPosition.x - 20, transform.localPosition.y, 40, 40), new GUIContent ("", tooltip));
		GUI.Label (new Rect (10, 40, 100, 40), GUI.tooltip);
	}
}
