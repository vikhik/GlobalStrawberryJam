using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Triplet : MonoBehaviour {
	public SmartObject element0;
	public ObjectState newElement0State;
	public SmartObject element1;
	public ObjectState newElement1State;
	public SmartObject element2;
	public ObjectState newElement2State;

	/** If this is provided, this object's state will be set to unused after this triplet is performed */
	public SmartObject newElement;

	public bool isCorrectTriplet(List<SmartObject> selected) {
		return selected.Contains(element0) && selected.Contains(element1) && selected.Contains(element2);
	}

	public void useTriplet() {
		element0.state = newElement0State;
		element1.state = newElement1State;
		element2.state = newElement2State;

		if (newElement) {
			newElement.state = ObjectState.unused;
		}
	}
}
