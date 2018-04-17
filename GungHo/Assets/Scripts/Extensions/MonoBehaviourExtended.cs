using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class MonoBehaviourExtended {

	/// <summary>
	/// Gets or add a component to a component
	/// e.g. BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
	/// </summary>
	public static T GetOrAddComponent<T> (this Component child) where T: Component {
		T result = child.GetComponent<T>();
		if (result == null) {
			result = child.gameObject.AddComponent<T>();
		}
		return result;
	}

	/// <summary>
	/// Gets or add a component to a gameObject
	/// e.g BoxCollider boxCollider = gameObject.GetOrAddComponent<BoxCollider>();
	/// </summary>
	public static T GetOrAddComponent<T> (this GameObject child) where T: Component {
		T result = child.GetComponent<T>();
		if (result == null) {
			result = child.gameObject.AddComponent<T>();
		}
		return result;
	}

	public static void SetObjActiveDelayed(this MonoBehaviour mono, bool active, float delay) {
		mono.StartCoroutine (SetActiveRoutine (mono.gameObject, active, delay));
	}

	public static void SetBoolAnimEndWait(this MonoBehaviour mono, Animator anim, string conditionParam) {
		mono.StartCoroutine(SetBoolAndWait (anim, conditionParam));
	}

	static IEnumerator SetBoolAndWait(Animator anim, string conditionParam) {
		anim.SetBool (conditionParam, true);
		yield return new WaitForEndOfFrame ();
		yield return new WaitForSeconds (anim.GetCurrentAnimatorClipInfo (0)[0].clip.length / anim.speed);
		anim.SetBool (conditionParam, false);
	}

	static IEnumerator SetActiveRoutine(GameObject gObj, bool active, float delay) {
		yield return new WaitForSeconds (delay);
		gObj.SetActive (active);
	}
}
