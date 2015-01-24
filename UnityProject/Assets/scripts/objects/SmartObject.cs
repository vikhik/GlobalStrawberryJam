using UnityEngine;
using System.Collections;

public enum ObjectState {
	unused,
	inuse,
	used
}

public struct SmartObjectSnapshot {
	public string id;
	public ObjectState state;
}

public class SmartObject : MonoBehaviour {

	public string description;
	public bool selectable = false;
	public bool selected = false;
	SmartObjectManager manager = null;
	DescriptionText descriptiontext;

	public ObjectState state = ObjectState.unused;

	public Sprite unusedSprite = null;
	public Sprite inuseSprite = null;
	public Sprite usedSprite = null;

	// Use this for initialization
	void Start () {
		manager = FindObjectOfType<SmartObjectManager>();
		manager.addObject(this);

		if (state != ObjectState.unused) {
			setState (state);
		}
		else {
			// The default sprite for any SmartObject is the sprite that it is when unused
			unusedSprite = GetComponent<SpriteRenderer>().sprite;
		}

		descriptiontext = FindObjectOfType<DescriptionText>();
	}

	// Update is called once per frame
	void Update () {
		// if this object is selectable
		if (selectable) {
			if (selected) {
				// activate visual effect for selected object
				
				renderer.material.color = Color.yellow;
				
			}
			else {
				// activate visual effect for selectable object
				
				renderer.material.color = Color.white;
				
			}
		}
	}


	void OnMouseDown() {
		if (selectable)
			FindObjectOfType<AdventureController>().target = this;
	}

	// SELECTION BOX HAS THIS:
	//if (this.selected) {
	//	SmartObject.manager.deselectObject(this);
	//}
	//else if (this.selectable) {
	//	SmartObject.manager.selectObject(this);
	//}

	public SmartObjectSnapshot getSnapshot() {
		SmartObjectSnapshot snapshot = new SmartObjectSnapshot();
		snapshot.id = name + description;
		snapshot.state = state;
		return snapshot;
	}

	public void setFromSnapshot(SmartObjectSnapshot snapshot) {
		if (snapshot.id == name + description) {
			setState(snapshot.state);
		}
	}

	private void setState(ObjectState newstate) {
		state = newstate;

		switch (state) {
		case ObjectState.inuse:
			GetComponent<SpriteRenderer>().sprite = inuseSprite;
			break;

		case ObjectState.used:
			GetComponent<SpriteRenderer>().sprite = usedSprite;
			selectable = false;
			break;
		}

		if (GetComponent<SpriteRenderer>().sprite == null) {
			enabled = false;
			selectable = false;
		}
	}

	public void touched() {
		if (this.selected) {
			manager.deselectObject(this);
		}
		else if (this.selectable) {
			manager.selectObject(this);
		}
	}

	void OnMouseOver() {
		descriptiontext.setString(description);
	}

	void OnMouseExit() {
		descriptiontext.setString(null);
	}
}
