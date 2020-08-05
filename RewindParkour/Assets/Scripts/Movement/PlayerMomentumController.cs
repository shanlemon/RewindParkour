using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMomentumController : MonoBehaviour {
	public static PlayerMomentumController Instance { get; private set; }

	[SerializeField] private PlayerLocomotion playerLocomotion;
	[SerializeField] private float defaultAccelerationMultiplier = 1 + 2f / 10f;
	[SerializeField] private float defaultMaxSpeedMultiplier = 1 + 2f / 15f;

	private float initialAcceleration;
	private float initialMaxSpeed;

	public void Awake() {
		Instance = this;

		initialAcceleration = playerLocomotion.Acceleration;
		initialMaxSpeed = playerLocomotion.MaxSpeed;
	}

	public void IncreaseMomentum() {
		playerLocomotion.Acceleration *= defaultAccelerationMultiplier;
		playerLocomotion.MaxSpeed *= defaultMaxSpeedMultiplier;
	}

	public void Reset() {
		playerLocomotion.Acceleration = initialAcceleration;
		playerLocomotion.MaxSpeed = initialMaxSpeed;
	}
}
