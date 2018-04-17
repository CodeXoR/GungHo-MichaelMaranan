using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

	public int damage;
	public float pushForce;

	protected virtual void DoDamage(IDamageable damageable) {
		damageable.ApplyDamage (damage);
	}

	protected virtual void OnCollisionEnter(Collision coll) {
		if (coll != null) {
			IDamageable damageable = coll.gameObject.GetComponent<IDamageable> ();
			if (damageable != null) {
				DoDamage (damageable);
			}
		}
	}
}