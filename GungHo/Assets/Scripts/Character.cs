using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

	int health;

	protected abstract void Move ();
	protected abstract void Attack ();
	protected abstract void ApplyDamage(int dmg);
}