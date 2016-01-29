﻿using UnityEngine;
using System.Collections;

public class MovementPowerup : Powerup 
{
	private float _speedModifier;

	public MovementPowerup(string name, string description, Sprite sprite, bool available, float speedModifier) 
		: base(name, description, sprite, available)
	{
		_speedModifier = speedModifier;
	}

	public void Enabled (GameObject player)
	{
		var characterMovement = player.GetComponent<CharacterMovement> ();
		characterMovement.speedModifier *= _speedModifier;
	}

	public void Disabled (GameObject player)
	{
		var characterMovement = player.GetComponent<CharacterMovement> ();
		characterMovement.speedModifier /= _speedModifier;
	}
}
