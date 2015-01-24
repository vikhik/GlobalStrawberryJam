using UnityEngine;
using System.Collections;

public enum ObjectState {
	unused,
	inuse,
	used
}

public class SmartObject : MonoBehaviour {

	public string description;
	public bool selectable = false;
	public bool selected = false;
	static SmartObjectManager manager;

	public ObjectState state = ObjectState.unused;

	// Use this for initialization
	void Start () {
		if (SmartObject.manager == null) {
			SmartObject.manager = GameObject.FindObjectOfType<SmartObjectManager>();
		}
		SmartObject.manager.addObject(this);
	}

	void Init() { 
	}
	
	// Update is called once per frame
	void Update () {
		// if this object is selectable
		if (selectable) {
			if (selected) {
				// activate visual effect for selected object

			}
			else {
				// activate visual effect for selectable object

			}
		}
	}

	void OnMouseDown() {
		FindObjectOfType<AdventureController>().target = this;

	}

	void displayInformation() {

	}

	void hideInformation() {

	}

	// SELECTION BOX HAS THIS:
	//if (this.selected) {
	//	SmartObject.manager.deselectObject(this);
	//}
	//else if (this.selectable) {
	//	SmartObject.manager.selectObject(this);
	//}

	public void touched() {
		displayInformation();
		if (this.selected) {
			SmartObject.manager.deselectObject(this);
		}
		else if (this.selectable) {
			SmartObject.manager.selectObject(this);
		}
	}
}
