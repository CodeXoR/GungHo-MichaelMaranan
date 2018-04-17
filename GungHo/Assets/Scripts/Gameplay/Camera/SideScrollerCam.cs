using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrollerCam : MonoBehaviour {

	public Transform followTarget;

	public float followSpeed = 3f;

	public bool constrainXMovement = false, constrainYMovement = false;

	public Vector2 offset = Vector2.zero;

	protected Vector3 movePos = Vector3.zero;

	protected virtual void Start() {
		movePos = transform.position;
	}
		
	protected virtual void Update () {
		if (followTarget != null) {
			
			if (constrainXMovement) {
				movePos.x = transform.position.x;
			} 
			else {
				movePos.x = followTarget.position.x + offset.x;
			}

			if (constrainYMovement) {
				movePos.y = transform.position.y;
			} 
			else {
				movePos.y = followTarget.position.y + offset.y;
			}

			movePos.z = -10f;
			transform.position = Vector3.Lerp (transform.position, movePos, Time.deltaTime * followSpeed);
		}
	}
}
