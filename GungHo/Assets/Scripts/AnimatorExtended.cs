using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorExtended {

	/// <summary>
	/// Adds an animation event at the specified keyframe time.<br>
	/// Set keyframe time to -1 to add the event at the end of the animation.<br>
	/// Note: The function to be triggered in the animation has to exist in the script where this function is called.
	/// </summary>
	/// <param name="anim">animator component of gameObject.</param>
	/// <param name="clipName">name of animation clip as defined in the animator controller.</param>
	/// <param name="functionName">name of function to be called in the event.</param>
	/// <param name="keyframeTime">time in the animation keyframe when to trigger the event.</param>
	public static void AddAnimationEvent(this Animator anim, string clipName, string functionName, float keyframeTime) {
		AnimationClip action = null;
		foreach(AnimationClip clip in anim.runtimeAnimatorController.animationClips) {
			if (clip.name == clipName) {
				action = clip;
				break;
			}
		}
		if (action != null) {
			bool triggerEventSet = false;
			foreach (AnimationEvent evt in action.events) {
				if (evt.functionName == functionName) {
					triggerEventSet = true;
					break;
				}
			}
			if (!triggerEventSet) {
				AnimationEvent evt = new AnimationEvent ();
				evt.time = keyframeTime > -1 ? keyframeTime : action.length;
				evt.functionName = functionName;
				action.AddEvent (evt);
			}
		}
	}

	public static void ClearAnimationEvents(this Animator anim, string clipName) {
		AnimationClip action = null;
		foreach(AnimationClip clip in anim.runtimeAnimatorController.animationClips) {
			if (clip.name == clipName) {
				action = clip;
				break;
			}
		}
		if (action != null) {
			action.events = null;
		}
	}

	/// <summary>
	/// Adds an animation event at the specified keyframe time.<br>
	/// Set keyframe time to -1 to add the event at the end of the animation.<br>
	/// Note: Use this to add an event needed to be called from another gameObject.
	/// </summary>
	/// <param name="anim">animator component of gameObject.</param>
	/// <param name="clipName">name of animation clip as defined in the animator controller.</param>
	/// <param name="functionName">name of function to be called in the event.</param>
	/// <param name="keyframeTime">time in the animation keyframe when to trigger the event.</param>
	public static void AddAnimationEventFromAnotherGameObject(this Animator anim, string clipName, System.Action functionToCall, float keyframeTime) {
		AnimationClip action = null;
		foreach(AnimationClip clip in anim.runtimeAnimatorController.animationClips) {
			if (clip.name == clipName) {
				action = clip;
				break;
			}
		}
		if (action != null) {
			bool triggerEventSet = false;
			string functionName = "PlayActionToTrigger";
			foreach (AnimationEvent evt in action.events) {
				if (evt.functionName == functionName) {
					triggerEventSet = true;
					break;
				}
			}
			if (!triggerEventSet) {
				AnimationHelper animHelper = anim.gameObject.GetOrAddComponent<AnimationHelper> ();
				animHelper.ActionToTrigger += functionToCall;
				AnimationEvent evt = new AnimationEvent ();
				evt.time = keyframeTime > -1 ? keyframeTime : action.length;
				evt.functionName = functionName;
				action.AddEvent (evt);
			}
		}
	}
}