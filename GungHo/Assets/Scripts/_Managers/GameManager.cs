using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	#region Events
	event System.Action<bool> _onGamePaused = null;
	public event System.Action<bool> OnGamePaused {
		add {
			_onGamePaused -= value;
			_onGamePaused += value;
		}
		remove {
			_onGamePaused -= value;
		}
	}

	event System.Action<bool> _onGameOver = null;
	public event System.Action<bool> OnGameOver {
		add {
			_onGameOver -= value;
			_onGameOver += value;
		}
		remove {
			_onGameOver -= value;
		}
	}
	#endregion

	#region STATIC VARIABLES
	public static GameManager instance;
	#endregion STATIC VARIABLES

	#region PUBLIC VARIABLES
	public Image transitionCover;
	public Text promptTitle, promptMessage;
	public float fadeSpeed = .02f;
	#endregion PUBLIC VARIABLES

	#region PROTECTED VARIABLES
	protected AsyncOperation async;
	protected Color fadeTransparency = new Color(0, 0, 0, .04f);
	protected string currentScene;
	protected bool gameOver = false, gamePaused = false;
	#endregion PROTECTED VARIABLES

	#region PROPERTIES
	// Get the current scene name
	public string CurrentSceneName { get { return currentScene; } }
	public bool SceneReady { get { return transitionCover.gameObject.activeInHierarchy == false; } }
	public bool GamePaused { 
		get {
			return gamePaused;
		}
		set {
			gamePaused = value;
			if (gamePaused) {
				ShowControls ();
			}
			if (_onGamePaused != null) {
				_onGamePaused (gamePaused);
			}
		}
	}
	public bool GameOver { 
		get {
			return gameOver;
		}
		set {  
			gameOver = value;
			if (gameOver) {
				ShowGameOver ();
			}
			if (_onGameOver != null) {
				_onGameOver (gameOver);
			}
		}
	}
	#endregion PROPERTIES

	#region MONOBEHAVIOUR CALLBACKS
	protected virtual void Awake() {
		// Only 1 Game Manager can exist at a time
		if (instance == null) {
			DontDestroyOnLoad(gameObject);
			instance = GetComponent<GameManager>();
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
		} 
		else {
			Destroy(gameObject);
		}
	}
	#endregion MONOBEHAVIOUR CALLBACKS

	#region PROTECTED FUNCTIONS
	protected virtual void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		currentScene = scene.name;
		instance.StartCoroutine(FadeIn(instance.transitionCover));
	}

	protected virtual void ShowControls() {}
	protected virtual void ShowGameOver() {}

	//Iterate the fader transparency to 100%
	protected IEnumerator FadeOut(Image transitionCover) {
		transitionCover.gameObject.SetActive(true);
		while (transitionCover.color.a < 1) {
			transitionCover.color += fadeTransparency;
			yield return new WaitForSeconds(fadeSpeed);
		}
		ActivateScene(); //Activate the scene when the fade ends
	}

	// Iterate the fader transparency to 0%
	protected IEnumerator FadeIn(Image transitionCover) {
		transitionCover.gameObject.SetActive(true);
		while (transitionCover.color.a > 0) {
			transitionCover.color -= fadeTransparency;
			yield return new WaitForSeconds(fadeSpeed);
		}
		transitionCover.gameObject.SetActive(false);
	}

	// Begin loading a scene with a specified string asynchronously
	protected IEnumerator Load(string sceneName) {
		async = SceneManager.LoadSceneAsync(sceneName);
		async.allowSceneActivation = false;
		yield return async;
	}
	#endregion PROTECTED FUNCTIONS

	#region PUBLIC FUNCTIONS
	// Load a scene with a specified string name
	public void LoadScene(string sceneName) {
		_onGameOver = null;
		_onGamePaused = null;
		instance.StartCoroutine(Load(sceneName));
		instance.StartCoroutine(FadeOut(instance.transitionCover));
	}

	// Reload the current scene
	public void ReloadScene() {
		LoadScene(SceneManager.GetActiveScene().name);
	}
		
	// Allows the scene to change once it is loaded
	public void ActivateScene() {
		async.allowSceneActivation = true;
	}

	public void HidePrompt() {
		promptTitle.transform.parent.gameObject.SetActive (false);
		if (GamePaused) {
			GamePaused = false;
		}
	}

	public void ShowPrompt(string title, string message) {
		promptTitle.text = title;
		promptMessage.text = message;
		promptTitle.transform.parent.gameObject.SetActive (true);
	}
	#endregion PUBLIC FUNCTIONS
}