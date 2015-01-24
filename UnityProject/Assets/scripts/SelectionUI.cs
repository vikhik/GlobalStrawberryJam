using UnityEngine;
using System.Collections;

public class SelectionUI : MonoBehaviour {

	SpriteRenderer selection0;
	SpriteRenderer selection1;
	SpriteRenderer selection2;

	// Use this for initialization
	void Start () {
		var selections = gameObject.GetComponentsInChildren<SpriteRenderer>();
		selection0 = selections[1]; // HARD CODED, SORRY :'(
		selection1 = selections[2];
		selection2 = selections[3];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void selectObject(SmartObject smartobject) {
		Sprite objectsprite = smartobject.GetComponent<SpriteRenderer>().sprite;
		if (selection0.sprite == null) {
			selection0.sprite = objectsprite;
		}
		else if (selection1.sprite == null) {
			selection1.sprite = objectsprite;
		}
		else if (selection2.sprite == null) {
			selection2.sprite = objectsprite;
		}
	}
	public void deselectObject(SmartObject smartobject) {
		Sprite objectsprite = smartobject.GetComponent<SpriteRenderer>().sprite;
		if (selection0.sprite == objectsprite) {
			selection0.sprite = null;
		}
		else if (selection1.sprite == objectsprite) {
			selection1.sprite = null;
		}
		else if (selection2.sprite == objectsprite) {
			selection2.sprite = null;
		}
	}
}
