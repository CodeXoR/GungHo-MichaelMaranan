using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboManManager : GameManager {

	void Update() {

		if (SceneReady == false) {
			return;
		}

		if (currentScene == "DemoLevel") {
			if (Input.GetButtonDown ("Submit")) {
				ReloadScene ();
			} 
			else if (Input.GetButtonDown ("Cancel")) {
				LoadScene ("StartMenu");
			}
		} 
		else if(currentScene == "StartMenu") {
			if (Input.GetButtonDown ("Submit")) {
				LoadScene ("DemoLevel");
			} 
			else if (Input.GetButtonDown ("Cancel")) {
				#if UNITY_STANDALONE || UNITY_STANDALONE_OSX
				Application.Quit ();
				#endif
			}
		}
	}
}