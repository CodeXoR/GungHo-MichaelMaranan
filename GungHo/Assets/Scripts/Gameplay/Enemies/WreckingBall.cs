using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBall : Enemy {

	public Vector3 startVelocity;

	Rigidbody body;

	protected override void Start() {
		base.Start ();
		body = GetComponent<Rigidbody> ();
	}

	void OnEnable() {
		GetComponent<Collider> ().enabled = true;
		float randomScale = Random.Range (.5f, 1.5f);
		transform.localScale = Vector3.one * randomScale;
		int randomDir = Random.Range (-1, 2);
		if (randomDir == 0) {
			randomDir = 1;
		}
		startVelocity *= (float)randomDir;
	}

	protected override void DestroyObj () {
		base.DestroyObj ();
		StartCoroutine (Reset ());
	}

	IEnumerator Reset() {
		GetComponent<Collider> ().enabled = false;
		yield return new WaitForSeconds (2f);
		body.velocity = Vector3.zero;
		gameObject.SetActive (false);
		transform.position = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		PoolCollector pCollector = GetComponent<PoolCollector> ();
		if (pCollector != null) {
			pCollector.OnPoolReturn ();
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (coll != null) {
			Character c = coll.gameObject.GetComponent<Character> ();
			if (c != null) {
				c.ApplyDamage (fallDamage);
				DestroyObj ();
			} 
			else {
				Interactible interactiveObj = coll.gameObject.GetComponent<Interactible> ();
				if (interactiveObj != null) {
					body.velocity = startVelocity * -1f;
				}  
				else {
					if (body.velocity.x == 0f) {
						body.velocity = startVelocity;
					}
				}
			}
		}
	}
}
