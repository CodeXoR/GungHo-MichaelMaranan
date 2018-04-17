using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class PoolCollector : MonoBehaviour {
	public System.Action OnPoolReturn { get; set; }
}

public class LevelManager : MonoBehaviour {

	const int INITIAL_LEVEL_CAPACITY = 10, INITIAL_SPAWN_POOL_CAPACITY = 10;

	public Text scoreCount;
	public Text waveLevel;

	public float levelWidth = 20, spawnInterval = 2f, difficultyInterval = 60f;

	public Transform spawnPoolRoot;
	public List<GameObject> enemiesPrefab;
	public List<Transform> spawnPoints;

	public Dictionary<GameObject, Stack<GameObject>> spawnPool;

	public List<Interactible> interactiblesInScene;

	public static LevelManager instance;

	float wrapWidth = 0, time = 0, waveSpeed = 1, killCount = 0;

	public bool LevelStarted { get; set; }

	void Awake() {
		instance = this;
	}

	void Start() {
		interactiblesInScene.Capacity = INITIAL_LEVEL_CAPACITY;
		wrapWidth = levelWidth / 2f;

		InitSpawnPool ();
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

		if (LevelStarted) {
			time += Time.deltaTime;

			if (time >= difficultyInterval) {
				time = 0f;
				spawnInterval -= .5f;
				waveSpeed++;
				if (spawnInterval < 0) {
					spawnInterval = .5f;
					waveSpeed = 1;
				}
			}

			waveLevel.text = waveSpeed.ToString ();
			scoreCount.text = killCount.ToString ();
		}
	}

	void InitSpawnPool() {
		// Initialize spawn pool
		spawnPool = new Dictionary<GameObject, Stack<GameObject>> ();
		for (int i = 0; i < enemiesPrefab.Count; ++i) {
			GameObject prefab = enemiesPrefab [i]; 
			Stack<GameObject> newPool = new Stack<GameObject> ();
			for (int j = 0; j < INITIAL_SPAWN_POOL_CAPACITY; ++j) {
				GameObject newObj = Instantiate (prefab, spawnPoolRoot);
				newObj.transform.localPosition = Vector3.zero;
				newObj.transform.localRotation = Quaternion.identity;
				newObj.AddComponent<PoolCollector> ().OnPoolReturn = () => spawnPool[prefab].Push(newObj);
				newObj.SetActive (false);
				newPool.Push (newObj);
			}
			spawnPool.Add (prefab, newPool);
		}
	}
		
	void SpawnEnemy() {
		int enemyType = Random.Range (0, enemiesPrefab.Count);
		GameObject enemyPrefab = enemiesPrefab [enemyType];
		Stack<GameObject> enemyStack = spawnPool [enemyPrefab];
		if (enemyStack.Count == 0) {
			for (int j = 0; j < INITIAL_SPAWN_POOL_CAPACITY; ++j) {
				GameObject newObj = Instantiate (enemyPrefab, spawnPoolRoot);
				newObj.transform.localPosition = Vector3.zero;
				newObj.transform.localRotation = Quaternion.identity;
				newObj.AddComponent<PoolCollector> ().OnPoolReturn = () => spawnPool[enemyPrefab].Push(newObj);
				newObj.SetActive (false);
				enemyStack.Push (newObj);
			}
		}
		GameObject newEnemy = spawnPool [enemiesPrefab [enemyType]].Pop ();
		int spawnPointIndex = Random.Range (0, spawnPoints.Count);
		newEnemy.transform.position = spawnPoints [spawnPointIndex].transform.position;
		newEnemy.SetActive (true);
	}

	public void AddInteractible(Interactible interactiveObj) {
		if (interactiblesInScene == null) {
			interactiblesInScene = new List<Interactible> ();
		}
		interactiblesInScene.Add (interactiveObj);
	}

	public void RemoveInteractible(Interactible interactiveObj) {
		interactiblesInScene.Remove (interactiveObj);
		if (interactiveObj.health == 0) {
			killCount++;
		}
	}

	public void StartSpawning() {
		InvokeRepeating ("SpawnEnemy", 1f, spawnInterval);
		LevelStarted = true;
	}

	public void StopSpawning() {
		CancelInvoke ("SpawnEnemy");
		LevelStarted = false;
	}
}
