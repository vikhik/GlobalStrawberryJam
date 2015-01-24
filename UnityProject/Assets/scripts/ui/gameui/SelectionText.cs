using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SelectionText : MonoBehaviour {

	Text selectiontext;

	// Use this for initialization
	void Start () {
		selectiontext = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateSelection(List<SmartObject> selected) {
		switch (selected.Count) {
			case 0:
				selectiontext.text = "...";
				break;
			case 1:
				selectiontext.text = selected[0].name + " + ..?";
				break;
			case 2:
				selectiontext.text = selected[0].name + " + " + selected[1].name.ToLower() + " + ..?";
				break;
			case 3:
				selectiontext.text = selected[0].name + " + " + selected[1].name.ToLower() + " + " + selected[2].name.ToLower() + " = ..?";
				break;
		}
	}
}
