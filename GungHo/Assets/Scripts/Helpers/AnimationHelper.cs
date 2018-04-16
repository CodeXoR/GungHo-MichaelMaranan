using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour {

	private System.Action _actionToTrigger = null;
	public event System.Action ActionToTrigger {
		add { 
			_actionToTrigger -= value;
			_actionToTrigger += value;
		}
		remove {
			_actionToTrigger -= value;
		}
	}

	void OnDestroy() {
		_actionToTrigger = null;
	}

	public void PlayActionToTrigger() {
		if (_actionToTrigger != null && _actionToTrigger.GetInvocationList ().Length > 0) {
			_actionToTrigger ();
		}
	}
}