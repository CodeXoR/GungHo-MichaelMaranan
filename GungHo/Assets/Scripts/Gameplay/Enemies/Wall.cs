using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Enemy {

	public float explosionForce = 10000f, explosionRadius = 50f, explosionUpwardsModifier = 8f;

	public Collider mainCollider;
	public Rigidbody[] bodies;

	Vector3 explosionPos;

	void SetBodiesKinematic(bool kinematic) {
		for (int i = 0; i < bodies.Length; ++i) {
			bodies [i].isKinematic = kinematic;
		}
	}

	protected override void DestroyObj () {
		base.DestroyObj ();
		mainCollider.enabled = false;
		SetBodiesKinematic (false);
	}

	void OnCollisionEnter(Collision coll) {
		if (coll != null) {
			Character c = coll.gameObject.GetComponent<Character> ();
			if (c != null) {
				c.ApplyDamage (fallDamage);
				explosionForce /= 4f;
				explosionRadius /= 4f;
				explosionUpwardsModifier /= 4f;
				DestroyObj ();
			}
		}
	}
}