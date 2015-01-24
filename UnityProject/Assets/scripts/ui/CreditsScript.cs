using UnityEngine;
using System.Collections;

public class CreditsScript : MonoBehaviour {
	
	public Texture backgroundTexture;
	
	public GUIStyle button1;
	
	void OnGUI(){
		
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height),backgroundTexture, ScaleMode.ScaleToFit);
		
		if (GUI.Button (new Rect (Screen.width * .375f, Screen.height * .875f, Screen.width * .25f, Screen.height * .1f), "", button1)) {
			print ("Clicked Back");
			Application.LoadLevel ("MainMenu");
		}
	}
}