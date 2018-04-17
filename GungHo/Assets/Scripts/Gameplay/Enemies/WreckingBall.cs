using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBall : Enemy {

	public float rotateSpeed = 30f;

	public Vector3 startVelocity;

	Rigidbody body;

	protected override void Start() {
		base.Start ();
		body = GetComponent<Rigidbody> ();
	}

	protected override void DestroyObj () {
		base.DestroyObj ();
	}

	void Update () {
		transform.Rotate (Vector3.forward, Time.deltaTime * rotateSpeed);
	}

	void OnCollisionEnter(Collision coll) {
		if (coll != null) {
			Character c = coll.gameObject.GetComponent<Character> ();
			if (c != null) {
				c.ApplyDamage (fallDamage);
				DestroyObj ();
			} 
			else {
				Interactible interactiveObj = coll.gameObject.GetComponent<Interactible> ();
				if (interactiveObj != null) {
					body.velocity = startVelocity * -1f;
					rotateSpeed *= -1f;
				}  
				else {
					if (body.velocity.x == 0f) {
						body.velocity = startVelocity;
					}
				}
			}
		}
	}
}
