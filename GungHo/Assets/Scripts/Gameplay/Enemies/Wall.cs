using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Enemy {

	public Collider mainCollider;
	public Rigidbody[] bodies;

	protected override void Start () {
		base.Start ();
	}

	void SetBodiesKinematic(bool kinematic) {
		for (int i = 0; i < bodies.Length; ++i) {
			bodies [i].isKinematic = kinematic;
		}
	}

	protected override void DestroyObj () {
		base.DestroyObj ();
		mainCollider.enabled = false;
		SetBodiesKinematic (false);
		Destroy (gameObject, 1f);
	}

	void OnCollisionEnter(Collision coll) {
		if (coll != null) {
			Character c = coll.gameObject.GetComponent<Character> ();
			if (c != null) {
				c.ApplyDamage (fallDamage);
				DestroyObj ();
				bodies [0].AddForceAtPosition (Vector3.up * 35f, bodies [0].transform.position, ForceMode.Impulse);
			} 
		}
	}
}