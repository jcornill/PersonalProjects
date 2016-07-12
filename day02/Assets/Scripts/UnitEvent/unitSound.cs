using UnityEngine;
using System.Collections;

public class unitSound : MonoBehaviour {

	public AudioClip orderSound;
	public AudioClip selectingSound;
	public AudioClip attackSound;
	public AudioClip deathSound;
	private AudioSource audios;
	private AudioSource unitAudio;
	private GameObject audioPlayer;

	// Use this for initialization
	void Start () {
		audioPlayer = GameObject.Find ("Audio player");
		audios = audioPlayer.GetComponent<AudioSource> ();
		unitAudio = GetComponent<AudioSource> ();
	}
	
	public void playSong(string soundName)
	{
		audioPlayer.transform.position = this.transform.position;
		if (soundName == "orderSound") {
			if (!audios.isPlaying) {
				audios.clip = orderSound;
				audios.Play ();
			}
		} else if (soundName == "selectingSound") {
			if (!audios.isPlaying) {
				audios.clip = selectingSound;
				audios.Play ();
			}
		} else if (soundName == "attackSound") {
			if (!unitAudio.isPlaying) {
				unitAudio.clip = attackSound;
				unitAudio.Play ();
			}
		} else if (soundName == "deathSound") {
			unitAudio.clip = deathSound;
			unitAudio.Play ();
		}
	}
}
