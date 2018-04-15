using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrollerCam : MonoBehaviour {

	public Transform followTarget;

	public float followSpeed = 3f;

	public Vector2 offset = Vector2.zero;

	Vector3 movePos;

	void Start() {
		movePos = transform.position;
	}

	// Update is called once per frame
	void Update () {
		movePos.x = followTarget.position.x + offset.x;
		movePos.y = followTarget.position.y + offset.y;
		movePos.z = -10f;
		transform.position = Vector3.Lerp (transform.position, movePos, Time.deltaTime * followSpeed);
	}
}
