using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robo : Character {

	#region CONSTANTS
	const float MOVE_DAMPER = 1f, JUMP_ANIM_SPEED = 3f, PUNCH_ANIM_SPEED = 1.5f, MIN_MOVE_SPEED = 1f, MAX_ANIM_SPEED = 3f;
	#endregion CONSTANTS

	#region PRIVATE VARIABLES
	Animator anim;
	CharacterController controller;
	Vector3 moveDirection = Vector3.zero;
	bool canMove = true, airBorne = false;
	float speed = 1f, lastSpeed = 1f, lastAnimSpeed = 1f;
	#endregion PRIVATE VARIABLES

	#region PUBLIC VARIABLES
	[RangeAttribute(1,20)]
	public float moveSpeed = 2f, runMultiplier = 2f, jumpStrength = 6f, runJumpMultiplier = 2f, glideSpeed = 4f, runJumpGlideMultiplier = 2f;
	[RangeAttribute(1,100)]
	public float gravity = 20f;
	#endregion PUBLIC VARIABLES

	#region PROPERTIES
	public bool IsRunning { get { return Input.GetButton ("Run"); } }
	public bool IsAttacking { get { return anim.GetBool ("attack"); } }
	public bool IsJumping { get; set; }
	public bool IsGliding { get; set; }
	public bool WillFallHigh { get; set; }
	#endregion PROPERTIES

	#region BASE OVERRIDES
	protected override void Move() {
		if (canMove) {
			float x = Input.GetAxisRaw ("Horizontal");
			SetOrientation (x);
			if (Input.GetButtonDown("Run")) {
				lastSpeed = speed;
			}
			if (Input.GetButton ("Run") && !IsJumping && !IsAttacking) {
				speed = moveSpeed * runMultiplier;
				SetAnimSpeed (speed);               
			} 
			else if (Input.GetButtonUp ("Run")) {
				speed = lastSpeed;
			}
			moveDirection.x = x * speed;
			anim.SetBool ("walk", x != 0);
		} 
		if (Input.GetButtonDown ("Jump")) {
			if (controller.isGrounded && !IsAttacking && !IsJumping) {
				SetAnimSpeed (JUMP_ANIM_SPEED);
				IsJumping = true;
				this.SetBoolAnimEndWait (anim, "jump");
				StartCoroutine (PerfectLanding ());
			}
		} 	
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move (moveDirection * Time.deltaTime);
	}

	protected override void Attack () {
		if(Input.GetButtonDown("Attack")) {
			if (!IsAttacking && !IsJumping) {
				SetAnimSpeed (PUNCH_ANIM_SPEED);
				this.SetBoolAnimEndWait (anim, "attack");
			}
		}
	}

	protected override void ApplyDamage(int dmg) {
	}
	#endregion BASE OVERRIDES

	#region MONOBEHAVIOUR CALLBACKS
	void Start() {
		anim = GetComponent<Animator> ();
		controller = GetComponent<CharacterController> ();
		anim.AddAnimationEvent ("Walk Mod", "SetWalkSpeed", 0f);
		anim.AddAnimationEvent ("Jump Mod", "SetJumpSpeed", .335f);
	}

	void Update() {
		Move ();
		Attack ();
	}
	#endregion MONOBEHAVIOUR CALLBACKS

	#region PRIVATE FUNCTIONS
	void SetMoveSpeed(float speed) {
		this.speed = Mathf.Max(speed, MIN_MOVE_SPEED);
	}

	void SetWalkSpeed() {
		if (!IsAttacking) {
			SetMoveSpeed (moveSpeed - MOVE_DAMPER);
			SetAnimSpeed (speed);
		}
	}

	void SetJumpSpeed() {
		SetMoveSpeed (IsRunning ? glideSpeed * runJumpGlideMultiplier : glideSpeed);
	}

	void SetAnimSpeed(float speed) {
		anim.speed = Mathf.Min (speed, MAX_ANIM_SPEED);
	}

	void SetMovable(int movable) {
		moveDirection.x = 0f;
		canMove = movable == 1;
	}

	void SetOrientation(float dir) {
		Vector3 temp = transform.localEulerAngles;
		if (dir > 0) {
			temp.y = 90;
		} 
		else if(dir < 0) {
			temp.y = -90;
		}
		transform.eulerAngles = temp;
	}

	void ApplyJump () {
		moveDirection.y = IsRunning ? jumpStrength * runJumpMultiplier : jumpStrength;
		WillFallHigh = IsRunning;
		airBorne = true;
	}

	void PauseAnim() {
		lastAnimSpeed = anim.speed;
		SetAnimSpeed (0f);
	}

	void ResumeAnim() {
		SetAnimSpeed (lastAnimSpeed);
	}
	#endregion PRIVATE FUNCTIONS

	#region IENUMERATORS
	IEnumerator PerfectLanding() {
		yield return new WaitUntil (() => airBorne);
		if (WillFallHigh) {
			yield return new WaitForSeconds (.05f);
		}
		IsGliding = true;
		yield return new WaitUntil (() => controller.isGrounded);
		IsJumping = false;
		airBorne = false;
		WillFallHigh = false;
		IsGliding = false;
		ResumeAnim ();
	}
	#endregion IENUMERATORS
}
