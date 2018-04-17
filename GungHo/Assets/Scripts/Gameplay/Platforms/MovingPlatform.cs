using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public void AttachObj(Transform t) {
		t.parent = transform;
	}
}