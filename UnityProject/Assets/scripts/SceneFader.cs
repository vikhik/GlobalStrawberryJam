using UnityEngine;
using System.Collections;

public class SceneFader : MonoBehaviour {

	bool ending = false;

	string endingtext;
	public float endTime = 5.0f;
	public string nextScene = "MainMenu";

	float timer = 0.0f;

	// Use this for initialization
	void Start () {
		fadeIn ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ending) {
			doEnding();
		}
	}

	void fadeIn() {

	}

	void fadeOut() {

	}

	void doEnding() {
		timer += Time.deltaTime;

		if (timer > endTime) {
			Application.LoadLevel(nextScene);
			ending = false;
		}
	}

	public void endScene(string endingtext) {
		this.endingtext = endingtext;
		fadeOut ();
	}
}
