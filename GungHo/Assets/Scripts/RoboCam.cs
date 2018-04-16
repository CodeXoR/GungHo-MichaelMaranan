using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboCam : SideScrollerCam {

	Robo rob;
	float lookOffsetY = 0f;

	protected override void Start () {
		base.Start ();
		rob = followTarget.GetComponent<Robo> ();
		lookOffsetY = offset.y;
	}

	protected override void Update () {
		if (rob != null) {
			offset.y = rob.WillFallHigh && rob.IsGliding ? lookOffsetY - 1f : lookOffsetY;
		} 
		base.Update ();
	}
}
