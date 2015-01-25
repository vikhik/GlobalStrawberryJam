using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {
	
	public Texture backgroundTexture;
	
	public GUIStyle button1;
	public GUIStyle button2;
	
	void OnGUI(){
		
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height),backgroundTexture, ScaleMode.ScaleToFit);
		
		if (GUI.Button (new Rect (Screen.width * .35f, Screen.height * .5f + (Screen.height * .08f), Screen.width * .3f, Screen.height * .1f), "", button1)) {
			SmartObjectManager.resetManager();
			Application.LoadLevel("Room01");
		}
		
		if (GUI.Button (new Rect (Screen.width * .35f, Screen.height * .5f + (Screen.height * .25f), Screen.width * .3f, Screen.height * .1f), "", button2)) {
			Application.LoadLevel ("Credits");
		}
	}
}
