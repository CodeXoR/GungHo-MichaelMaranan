using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoboManManager : GameManager {

	void Update() {

		if (SceneReady == false) {
			return;
		}

		if (currentScene == "DemoLevel") {
			if (Input.GetButtonDown ("Submit")) {
				if (!GamePaused && !GameOver) {
					LevelManager.instance.StopSpawning ();
					GamePaused = true;
				} 
				else {
					ReloadScene ();
					GameOver = false;
				} 
			} 
			else if (Input.GetButtonDown ("Cancel")) {
				if (GamePaused) {
					HidePrompt ();
					GamePaused = false;
					LevelManager.instance.StartSpawning ();
				} 
				else {
					HidePrompt ();
					LoadScene ("StartMenu");
				}
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

	protected override void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		base.OnLevelFinishedLoading (scene, mode);
		gameOver = false;
		gamePaused = false;
		if(currentScene != "StartMenu") {
			GamePaused = true;
		}
	}

	protected override void ShowControls() {
		ShowPrompt("Controls", "A ~ hold to run\nX ~ jump            \nB ~ punch          \n");
	}

	protected override void ShowGameOver() {
		ShowPrompt("Game Over", "Start ~ restart game\nBack ~ return to start menu");
	}
}