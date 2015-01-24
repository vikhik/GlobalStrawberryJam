using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {
	
	public Texture backgroundTexture;
	
	public GUIStyle button1;
	
	void OnGUI(){
		
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height),backgroundTexture, ScaleMode.ScaleToFit);
		
		if (GUI.Button (new Rect (Screen.width * .25f, Screen.height * .5f, Screen.width * .5f, Screen.height * .1f), "", button1)) {
			print ("Clicked Play Game");
		}
		
		if (GUI.Button (new Rect (Screen.width * .25f, Screen.height * .5f + (Screen.height * .15f), Screen.width * .5f, Screen.height * .1f), "", button1)) {
			print ("Clicked Credits");
		}
		
		if (GUI.Button (new Rect (Screen.width * .25f, Screen.height * .5f + (Screen.height * .3f), Screen.width * .5f, Screen.height * .1f), "", button1)) {
			print ("Clicked Exit");
		}
	}
}
