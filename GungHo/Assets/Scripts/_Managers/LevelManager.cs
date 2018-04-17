using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	const int INITIAL_LEVEL_CAPACITY = 10;

	public float levelWidth = 20;
	float wrapWidth = 0;

	public List<Interactible> interactiblesInScene;

	public static LevelManager instance;

	void Awake() {
		instance = this;
	}

	void Start() {
		interactiblesInScene.Capacity = INITIAL_LEVEL_CAPACITY;
		wrapWidth = levelWidth / 2f;
	}

	void OnDestroy() {
		instance = null;
	}

	void Update() {
		// Wrap character positions to level width
		for (int i = 0; i < interactiblesInScene.Count; ++i) { 
			Interactible c = interactiblesInScene [i];
			Vector3 cPos = c.transform.position;
			if (c.transform.position.x < -wrapWidth) {
				cPos.x = wrapWidth;
			} 
			else if (c.transform.position.x > wrapWidth) { 
				cPos.x = -wrapWidth;
			}
			c.transform.position = cPos;
		}
	}

	public void AddInteractible(Interactible interactiveObj) {
		if (interactiblesInScene == null) {
			interactiblesInScene = new List<Interactible> ();
		}
			
		interactiblesInScene.Add (interactiveObj);
	}

	public void RemoveInteractible(Interactible interactiveObj) {
		interactiblesInScene.Remove (interactiveObj);
	}
}
