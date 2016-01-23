﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Character movement.
/// 
/// Controls the characters movement from user input.
/// </summary>
public class CharacterMovement : MonoBehaviour 
{
	
	Rigidbody2D _rigidBody;
	
	Vector2 _inputAxes;
	bool _inWater;

	public Animator animator;
	
	public float speed = 1f;
	
	//How long we slow down for when slowed.
	public float slowTime = 1.5f;
	
	//How much we slow down when slowed.
	public float slowPercentage  = 0.4f;

	//For powerups and status effects.
	public float speedModifier = 1.0f;
	
	float slowTimeElapsed = 0.0f;
	bool currentlySlowed;


	//Last direction we sent to the animator
	string lastDirectionSent = "";


	// tracks if the player can or can't move
	// (based on the activation of the dialogue
	// system)
	public bool canMove;


	void Start () 
	{
		_inputAxes = new Vector2 ();
		//StartWalking ();
	}
	
	void Awake()
	{
		_rigidBody = GetComponent<Rigidbody2D> ();
	}


	/// <summary>
	/// Slow down the player due to mud, slime or whatever the case may be.
	/// </summary>
	void Slow()
	{
		currentlySlowed = true;
		slowTimeElapsed = 0.0f;
	}

	void UpdateWalkDirection( Vector2 input)
	{
		string directionToSend = "";
		if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
		{
			if (input.x > 0)
			{
				directionToSend = "walkRight";
			}
			else
			{
				directionToSend = "walkLeft";
			}
		} 
		else
		{
			if (input.y > 0)
			{
				directionToSend = "walkUp";
			}
			else
			{
				directionToSend = "walkDown";
			}
		}

		if (directionToSend != lastDirectionSent)
		{
			animator.SetTrigger(directionToSend);
			lastDirectionSent = directionToSend;
		}

		//If the player doesn't want to go anywhere
		if (input.y == 0 && input.x == 0)
		{
			//This is called every frame so it will stop the
			//animation at the first frame making it look like the
			//duck is standing still.
			lastDirectionSent = "";
			animator.SetTrigger("walkDown");
		}
	}
	
	void FixedUpdate()
	{
		if (!canMove)
		{
			// prevents the rest of the code from
			// executing if the player shouldn't move
			// at the moment
			return;
		}

		Vector2 currentPos = transform.position;

		_inputAxes.x = Input.GetAxis ("Horizontal");
		_inputAxes.y = Input.GetAxis ("Vertical"); 

		UpdateWalkDirection (_inputAxes);

		Vector2 updateDirection = _inputAxes * speed * speedModifier * 0.01f;

		if (currentlySlowed)
		{
			updateDirection *= slowPercentage;
		}
		currentPos += updateDirection;

		_rigidBody.MovePosition (currentPos);
	}

	public void StartSwimming()
	{
		speedModifier = 1.2f;
	}
	
	public void StartWalking()
	{
		speedModifier = 1;
	}

	public void StartFlying () {
		speedModifier = 1.5f;
	}
	
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "water")
		{
			StartSwimming();
		} 
		else if (other.gameObject.tag == "ground")
		{
			StartWalking();
		}
	}
}