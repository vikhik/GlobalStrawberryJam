using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DescriptionText : MonoBehaviour {

	Text hovertext;
	public bool displaytext = false;

	// Use this for initialization
	void Start () {
		hovertext = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setString(string newtext) {
		if (displaytext)
			hovertext.text = newtext;
		else
			hovertext.text = null;
	}
}
