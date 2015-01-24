using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmartObjectManager : MonoBehaviour {

	private List<SmartObject> collection = new List<SmartObject>();
	private List<SmartObject> selection = new List<SmartObject>();
	private List<Triplet> knowntriplets = new List<Triplet>();

	// Use this for initialization
	void Start () {
		var alltriplets = gameObject.GetComponents<Triplet>();
		foreach (Triplet triplet in alltriplets) {
			knowntriplets.Add(triplet);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (selection.Count == 3) {
			// Make NOW button usable looking
		}
		else {
			// Make NOW button not-usable looking
		}
	}

	public void selectObject(SmartObject smartobject) {
		if (selection.Count < 3) {
			selection.Add(smartobject);
			smartobject.selected = true;

			print("SELECTED" + smartobject);

			// TODO: add smartobject to object-tray display
		}
		else {
			// TODO: make object-tray display flash red or otherwise indicate to the player they are dumb
		}
	}

	public void deselectObject(SmartObject smartobject) {
		selection.Remove(smartobject);
		smartobject.selected = false;
		print("DESELECTED" + smartobject);
	}

	public void whatDoWeDoNow() {
		// find a triplet, if any

		if (selection.Count != 3) {
			print("NOT READY FOR THINGS");
			return;
		} 

		Triplet chosentriplet = null;
		foreach (Triplet triplet in knowntriplets) {
			if (triplet.isCorrectTriplet(selection)) {
				chosentriplet = triplet;
				break;
			}
		}

		if (chosentriplet != null) {
			chosentriplet.useTriplet();
			print("WE DID THE THINGS");
		}
		else {
			// default failstate
			print("WE FAILED TO DO THINGS");
		}

		selection.Clear();
	}

	public void addObject(SmartObject smartobject) {
		collection.Add(smartobject);
	}

	public void setActive(int number) {
		foreach (SmartObject smartobject in collection) {
			smartobject.selectable = false;
			smartobject.selected = false;
		}

		for (var i = 0; i < number; i++) {
			int randint = Random.Range(0, collection.Count);

			// find a random unused/inuse object
			while (collection[randint].state == ObjectState.used) {
				randint = Random.Range(0, collection.Count);
			}
			
			collection[randint].selectable = true;
		}
	}
	void OnMouseDown() {
		whatDoWeDoNow();
	}
}
