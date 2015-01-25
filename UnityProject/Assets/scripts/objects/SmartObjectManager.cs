using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState {
	bool ladyescaped = false;
	bool detectiveescaped = false;
	bool soldierescaped = false;

	public bool hasEscaped(int id) {
		switch (id) {
			case 0:
				return ladyescaped;
			case 1:
				return detectiveescaped;
			case 2:
				return soldierescaped;
			default:
				return false;
		}
	}

	public void escape(int id) {
		switch (id) {
			case 0:
				ladyescaped = true;
				break;
			case 1:
				detectiveescaped = true;
				break;
			case 2:
				soldierescaped = true;
				break;
		}
	}

	public bool allEscaped() {
		return ladyescaped && detectiveescaped && soldierescaped;
	}

	public bool allFailed() {
		return !ladyescaped && !detectiveescaped && !soldierescaped;
	}
}

public class SmartObjectManager : MonoBehaviour {

	private List<SmartObject> collection = new List<SmartObject>();
	private List<SmartObject> selection = new List<SmartObject>();
	private List<Triplet> knowntriplets = new List<Triplet>();
	private SceneFader sceneFader;
	private Animator animator;

	public List<AdventureController> characters = new List<AdventureController>();

	// UNUSED NOW
	//private SelectionUI selectionUI;

	// REPLACED WITH:
	private SelectionText selectionText;

	public List<SpriteRenderer> vatsprites = new List<SpriteRenderer>();

	private static int currentCharacter = -1;
	static List<SmartObjectSnapshot> savestate = new List<SmartObjectSnapshot>();
	public static GameState gamestate = new GameState();

	public GameObject monster;
	public GameObject professor;
	public SmartObject door;

	public static void resetManager() {
		currentCharacter = -1;
		savestate = new List<SmartObjectSnapshot>();
		gamestate = new GameState();
	}

	// Use this for initialization
	void Start () {
		var alltriplets = gameObject.GetComponents<Triplet>();
		foreach (Triplet triplet in alltriplets) {
			knowntriplets.Add(triplet);
		}

		animator = GetComponent<Animator>();
		
		//selectionUI = FindObjectOfType<SelectionUI>();
		selectionText = FindObjectOfType<SelectionText>();
	
		sceneFader = FindObjectOfType<SceneFader>();

		if (savestate.Count > 0) {
			loadFromSaveState();
		}

		setNextCharacter();

		setVatState();
	}

	void setVatState() {
		int charnumber = 0;

		while (charnumber < currentCharacter) {
			if (!gamestate.hasEscaped(charnumber)) {
				vatsprites[charnumber].gameObject.SetActive(true);
			}
			charnumber++;
		}
	}

	void hideAllVats() {
		foreach (SpriteRenderer vat in vatsprites) {
			vat.sprite = null;
		}
	}

	void setNextCharacter() {
		if (currentCharacter >= 0 && characters[currentCharacter]) {
			characters[currentCharacter].gameObject.SetActive(false);
		}

		currentCharacter++;
		if (currentCharacter < characters.Count && characters[currentCharacter]) {
			characters[currentCharacter].gameObject.SetActive(true);
		}
		else {
			// PLAY FINAL CUTSCENE
			if (gamestate.allFailed()) {
				monster.SetActive(true);
				monster.GetComponent<Animator>().SetTrigger("monsterGO");
				hideAllVats();

				StartCoroutine(doorExplosion());

				sceneFader.endGame("Your experiment is a success!", 10f);
			}
			else if (gamestate.allEscaped()) {
				professor.SetActive(true);
				sceneFader.endGame("How can you experiment like this?", 8f);
			}
			else {
				sceneFader.failGame("The experiment must continue... The hunt begins anew!", 3f);
			}
		}
	}

	private IEnumerator doorExplosion() {
		var progress = 0f;

		while (progress < 7f) {
			progress += Time.deltaTime;
			yield return null;
		}

		door.setState(ObjectState.used);
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

			//print("SELECTED: " + smartobject);

			//selectionUI.selectObject(smartobject);
			selectionText.updateSelection(selection);
		}
		else {
			// TODO: make object-tray display flash red or otherwise indicate to the player they are dumb
		}
	}

	public void deselectObject(SmartObject smartobject) {
		selection.Remove(smartobject);
		smartobject.selected = false;
		//print("DESELECTED: " + smartobject);
		//selectionUI.deselectObject(smartobject);
		selectionText.updateSelection(selection);
	}

	public void whatDoWeDoNow() {
		// find a triplet, if any

		if (selection.Count != 3) {
			//print("NOT READY FOR THINGS");
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
		selectionText.updateSelection(selection);

		if (chosentriplet == null) {
			// default failstate
			//print("WE FAILED TO DO THINGS");
			endScene("You waste fruitless hours trying to escape using these items. Eventually, you give into your despair.");
		}
		else {
			//print("WE ARE DOING THE THINGS");
			string sceneEnding = chosentriplet.useTriplet();

			SmartObjectManager.gamestate.escape(SmartObjectManager.currentCharacter);

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
