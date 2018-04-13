using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class MonoBehaviourExtended {

	public static void SetBoolAnimEndWait(this MonoBehaviour mono, Animator anim, string conditionParam) {
		mono.StartCoroutine(SetBoolAndWait (anim, conditionParam));
	}

	static IEnumerator SetBoolAndWait(Animator anim, string conditionParam) {
		anim.SetBool (conditionParam, true);
		yield return new WaitForEndOfFrame ();
		yield return new WaitForSeconds (anim.GetCurrentAnimatorClipInfo (0)[0].clip.length / anim.speed);
		anim.SetBool (conditionParam, false);
	}
}
