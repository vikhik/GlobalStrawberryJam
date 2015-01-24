using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmartObjectManager : MonoBehaviour {

	private List<SmartObject> collection = new List<SmartObject>();
	private List<SmartObject> selection = new List<SmartObject>();
	private List<Triplet> knowntriplets = new List<Triplet>();
	private SelectionUI selectionUI;
	private SceneFader sceneFader;
	private Animator animator;

	static List<SmartObjectSnapshot> savestate = new List<SmartObjectSnapshot>();

	// Use this for initialization
	void Start () {
		var alltriplets = gameObject.GetComponents<Triplet>();
		foreach (Triplet triplet in alltriplets) {
			knowntriplets.Add(triplet);
		}

		animator = GetComponent<Animator>();
		
		selectionUI = FindObjectOfType<SelectionUI>();
	
		sceneFader = FindObjectOfType<SceneFader>();

		if (savestate.Count > 0) {
			loadFromSaveState();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (selection.Count == 3) {
			// Make NOW button usable looking
			animator.SetBool("active", true);
		}
		else {
			// Make NOW button not-usable looking
			animator.SetBool("active", false);
		}
	}

	public void selectObject(SmartObject smartobject) {
		if (selection.Count < 3) {
			selection.Add(smartobject);
			smartobject.selected = true;

			print("SELECTED: " + smartobject);

			selectionUI.selectObject(smartobject);
		}
		else {
			// TODO: make object-tray display flash red or otherwise indicate to the player they are dumb
		}
	}

	public void deselectObject(SmartObject smartobject) {
		selection.Remove(smartobject);
		smartobject.selected = false;
		print("DESELECTED: " + smartobject);
		selectionUI.deselectObject(smartobject);
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

		while (selection.Count > 0) {
			deselectObject(selection[0]);
		}

		if (chosentriplet == null) {
			// default failstate
			print("WE FAILED TO DO THINGS");
		}
		else {
			print("WE ARE DOING THE THINGS");
			string sceneEnding = chosentriplet.useTriplet();

			endScene(sceneEnding);
		}
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
			// find a usable random smartobject
			int randint = Random.Range(0, collection.Count);
			while (collection[randint].state == ObjectState.used || !collection[randint].enabled) {
				randint = Random.Range(0, collection.Count);
			}

			// and let it be selectable
			collection[randint].selectable = true;
		}
	}
	void OnMouseDown() {
		whatDoWeDoNow();
	}

	void endScene(string sceneEnding) {
		generateSaveState();
		
		if (sceneFader)
			sceneFader.endScene(sceneEnding);
	}

	void generateSaveState() {
		foreach (SmartObject smartobject in collection) {
			savestate.Add(smartobject.getSnapshot());
		}
	}

	void loadFromSaveState() {
		var allsmartobjects = FindObjectsOfType<SmartObject>();
		foreach (SmartObjectSnapshot snapshot in savestate) {
			foreach (SmartObject smartobject in allsmartobjects) {
				smartobject.setFromSnapshot(snapshot);
			}
		}
	}
}
