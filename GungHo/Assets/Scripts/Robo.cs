using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robo : Character {

	#region CONSTANTS
	const float moveDamper = 1f, jumpAnimSpeed = 3f, punchAnimSpeed = 1.5f;
	#endregion CONSTANTS

	#region PRIVATE VARIABLES
	Animator anim;
	CharacterController controller;
	Vector3 moveDirection = Vector3.zero;
	bool canMove = true, airBorne = false;
	float speed = 1f, lastAnimSpeed = 1f;
	#endregion PRIVATE VARIABLES

	#region PUBLIC VARIABLES
	[RangeAttribute(1,20)]
	public float moveSpeed = 2f, jumpStrength = 6f, glideSpeed = 4f;
	[RangeAttribute(1,100)]
	public float gravity = 20f;
	public LayerMask interactibleLayer;
	#endregion PUBLIC VARIABLES

	#region PROPERTIES
	public bool IsAttacking { get { return anim.GetBool ("attack"); } }
	public bool IsJumping { get; set; }
	#endregion PROPERTIES

	#region BASE OVERRIDES
	protected override void Move() {
		if (canMove) {
			float x = Input.GetAxisRaw ("Horizontal");
			SetOrientation (x);
			moveDirection.x = x * speed;
			anim.SetBool ("walk", x != 0);
		} 
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (controller.isGrounded && !IsAttacking && !IsJumping) {
				anim.speed = jumpAnimSpeed;
				IsJumping = true;
				this.SetBoolAnimEndWait (anim, "jump");
				StartCoroutine (PerfectLanding ());
			}
		} 	
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move (moveDirection * Time.deltaTime);
	}

	protected override void Attack () {
		if(Input.GetKeyDown(KeyCode.P)) {
			if (!IsAttacking && !IsJumping) {
				anim.speed = punchAnimSpeed;
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
		this.speed = Mathf.Max(speed, 1);
	}

	void SetWalkSpeed() {
		SetMoveSpeed (moveSpeed - moveDamper);
		anim.speed = speed;
	}

	void SetJumpSpeed() {
		SetMoveSpeed (glideSpeed);
	}

	void ApplyJump () {
		moveDirection.y = jumpStrength;
		airBorne = true;
	}

	void PauseAnim() {
		lastAnimSpeed = anim.speed;
		anim.speed = 0f;
	}

	void ResumeAnim() {
		anim.speed = lastAnimSpeed;
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
	#endregion PRIVATE FUNCTIONS

	#region IENUMERATORS
	IEnumerator PerfectLanding() {
		yield return new WaitUntil (() => airBorne);
		yield return new WaitUntil (() => controller.isGrounded);
		IsJumping = false;
		airBorne = false;
		ResumeAnim ();
	}
	#endregion IENUMERATORS
}
