using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Interactible {

	protected abstract void Move ();
	protected abstract void Attack ();
}