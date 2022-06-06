using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	[SerializeField] PlayerData data;
	Rigidbody2D rb;
	[SerializeField] bool onFloor;

	float xMove;
    private void Start()
    {
		rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
		xMove = Input.GetAxis("Horizontal");
	
	}
    private void FixedUpdate()
    {
		Run(1);
	}
    private void Run(float lerpAmount)
	{
		float targetSpeed = xMove * data.runMaxSpeed; //calculate the direction we want to move in and our desired velocity
		float speedDif = targetSpeed - rb.velocity.x; //calculate difference between current velocity and desired velocity

		#region Acceleration Rate
		float accelRate;

		//gets an acceleration value based on if we are accelerating (includes turning) or trying to decelerate (stop). As well as applying a multiplier if we're air borne
		if (!onFloor)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel : data.runDeccel;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel * data.accelInAir : data.runDeccel * data.deccelInAir;

		//if we want to run but are already going faster than max run speed
		if (((rb.velocity.x > targetSpeed && targetSpeed > 0.01f) || (rb.velocity.x < targetSpeed && targetSpeed < -0.01f)) && data.doKeepRunMomentum)
		{
			accelRate = 0; //prevent any deceleration from happening, or in other words conserve are current momentum
		}
		#endregion

		#region Velocity Power
		float velPower;
		if (Mathf.Abs(targetSpeed) < 0.01f)
		{
			velPower = data.stopPower;
		}
		else if (Mathf.Abs(rb.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(rb.velocity.x)))
		{
			velPower = data.turnPower;
		}
		else
		{
			velPower = data.accelPower;
		}
		#endregion

		// applies acceleration to speed difference, then is raised to a set power so the acceleration increases with higher speeds, finally multiplies by sign to preserve direction
		float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
		movement = Mathf.Lerp(rb.velocity.x, movement, lerpAmount); // lerp so that we can prevent the Run from immediately slowing the player down, in some situations eg wall jump, dash 

		rb.AddForce(movement * Vector2.right); // applies force force to rigidbody, multiplying by Vector2.right so that it only affects X axis 

		//if (InputHandler.instance.MoveInput.x != 0)
		//	CheckDirectionToFace(InputHandler.instance.MoveInput.x > 0);
	}

	private void Jump()
	{
		//ensures we can't call a jump multiple times from one press
		//_lastPressedJumpTime = 0;
		//_lastOnGroundTime = 0;

		rb.velocity = new Vector2(rb.velocity.x, data.jumpForce);
	}

	private void OnCollisionEnter2D(Collision2D collision)
    {
		//if (collision.gameObject.layer == 6) onFloor = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
		if (collision.gameObject.layer == 6) onFloor = false;
	}
}
