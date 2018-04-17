using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour, IDamageable {

	[RangeAttribute(1,999999)]
	public int health; 

	protected virtual void Start() {
		LevelManager lm = LevelManager.instance;
		if (lm != null) {
			lm.AddInteractible (this);
		}
	}

	public virtual void ApplyDamage(int dmg) {
		health -= dmg;
		if (health <= 0) {
			health = 0;
			DestroyObj ();
		}
	}

	protected virtual void DestroyObj() {
		LevelManager lm = LevelManager.instance;
		if (lm != null) {
			lm.RemoveInteractible (this);
		}
	}
}
