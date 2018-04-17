﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : Weapon {

	protected override void OnCollisionEnter (Collision coll) {
		Vector3 hitPoint = coll.contacts [0].point;
		hitPoint.y = coll.transform.position.y;
		Vector3 fistPos = transform.position;
		fistPos.y = hitPoint.y;
		Vector3 hitDir = (hitPoint - fistPos).normalized;
//		Debug.DrawRay (fistPos, hitDir * pushForce, Color.green);
		coll.rigidbody.AddForceAtPosition (hitDir * pushForce, hitPoint, ForceMode.Impulse);
		base.OnCollisionEnter (coll);
	}
}