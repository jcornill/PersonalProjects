using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	public playerScript_ex00 red;
	public playerScript_ex00 yellow;
	public playerScript_ex00 blue;
	public Camera camera;

	private int currentLevel;
	private bool finished;

	// Use this for initialization
	void Start () {
		camera = this.GetComponent<Camera>();
		red.select (true);
		finished = false;
		currentLevel = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Alpha1)) {
			red.select (true);
			yellow.select (false);
			blue.select (false);
		} else if (Input.GetKey (KeyCode.Alpha2)) {
			red.select (false);
			yellow.select (true);
			blue.select (false);
		} else if (Input.GetKey (KeyCode.Alpha3)) {
			red.select (false);
			yellow.select (false);
			blue.select (true);
		} else if (Input.GetKey (KeyCode.R)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		} else if (Input.GetKeyDown(KeyCode.L)) {
			finishLevel ();
		}
		moveCamera();
		isLevelFinish ();
	}

	private void moveCamera()
	{
		if (blue.selected)
		{
			camera.transform.localPosition = blue.transform.localPosition;
		}
		else if (red.selected)
		{
			camera.transform.localPosition = red.transform.localPosition;
		}
		else if (yellow.selected)
		{
			camera.transform.localPosition = yellow.transform.localPosition;
		}
		Vector3 newVect = camera.transform.localPosition;
		newVect.z = -10;
		camera.transform.localPosition = newVect; 
	}

	private void isLevelFinish()
	{
		if (red.onExit () && blue.onExit () && yellow.onExit () && !finished)
			finishLevel ();
	}

	private void finishLevel()
	{
		finished = true;
		Debug.Log("Niveau termine bien joue !");
		loadNextLevel ();
	}
	private void loadNextLevel()
	{
		currentLevel++;
		SceneManager.LoadScene("Level" + currentLevel);
	}
}
