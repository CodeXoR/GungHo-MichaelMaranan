using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robo : Character {

	Animator anim;

	CharacterController controller;

	Vector3 moveDirection = Vector3.zero;

	bool canMove = true;

	float moveSpeed = 2f;

	void Start() {
		anim = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();
	}

	public void ApplyJump () {
		moveDirection.y = 6.0f;
		canMove = true;
		SetMoveSpeed (4f);
	}

	public void SetMoveSpeed(float speed) {
		moveSpeed = speed;
	}

	protected override void Move() {
		if (!IsAttacking ()) {
			float x = Input.GetAxisRaw ("Horizontal");
			bool walk = x != 0f;
			if (walk) {
				Vector3 temp = transform.localEulerAngles;
				if (x > 0) {
					temp.y = 90;
				} else {
					temp.y = -90;
				}
				transform.eulerAngles = temp;
			}
			if (canMove) {
				moveDirection.x = x * moveSpeed;
			}
			anim.speed = 1.5f;
			anim.SetBool ("walk", walk);
		} 
		else {
			moveDirection.x = 0f;
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (controller.isGrounded && !IsAttacking() && !IsJumping()) {
				moveDirection.x = 0f;
				canMove = false;
				anim.speed = 2f;
				this.SetBoolAnimEndWait (anim, "jump");
			}
		} 	
		moveDirection.y -= 20f * Time.deltaTime;
		controller.Move (moveDirection * Time.deltaTime);
	}

	protected override void Attack () {
		anim.speed = 1.5f;
		this.SetBoolAnimEndWait (anim, "attack");
	}

	protected override void ApplyDamage(int dmg) {
	}

	void Update() {
		Move();
		if(Input.GetKeyDown(KeyCode.P)) {
			if (!IsAttacking() && !IsJumping()) {
				Attack ();
			}
		}
	}

	bool IsAttacking() {
		return anim.GetBool ("attack");
	}

	bool IsJumping() {
		return anim.GetBool ("jump");
	}
}
