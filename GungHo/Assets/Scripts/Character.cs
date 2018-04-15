using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

	[RangeAttribute(1,999999)]
	public int health;

	protected abstract void Move ();
	protected abstract void Attack ();
	protected abstract void ApplyDamage(int dmg);
}