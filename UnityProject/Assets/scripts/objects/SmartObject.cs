using UnityEngine;
using System.Collections;

public enum ObjectState {
	unused,
	used
}

public struct SmartObjectSnapshot {
	public string id;
	public ObjectState state;
}

public class SmartObject : MonoBehaviour {

	public string unusedDescription;
	public string usedDescription;

	public bool selectable = false;
	public bool selected = false;
	SmartObjectManager manager = null;
	DescriptionText descriptiontext;

	public ObjectState state = ObjectState.unused;

	public Sprite usedSprite = null;
	public Animator animator;

	// Use this for initialization
	void Start () {
		manager = FindObjectOfType<SmartObjectManager>();
		manager.addObject(this);

		if (state != ObjectState.unused) {
			setState (state);
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
		snapshot.id = name + unusedDescription;
		snapshot.state = state;
		return snapshot;
	}

	public void setFromSnapshot(SmartObjectSnapshot snapshot) {
		if (snapshot.id == name + unusedDescription) {
			setState(snapshot.state);
		}
	}

	public void setState(ObjectState newstate) {
		state = newstate;

		switch (state) {
		case ObjectState.used:
			GetComponent<SpriteRenderer>().sprite = usedSprite;
			selectable = false;

			print(animator);

			if (animator) {
				animator.SetBool("used", true);
			}
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
		switch (state) {
		case ObjectState.used:
			descriptiontext.setString(usedDescription);
			break;
		case ObjectState.unused:
			descriptiontext.setString(unusedDescription);
			break;
		}
	}

	void OnMouseExit() {
		descriptiontext.setString(null);
	}
}
