using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePlayerSpeed : MonoBehaviour {
	public IncreasePlayerSpeed Instance { get; private set; }

	[SerializeField] private PlayerLocomotion playerLocomotion;
	[SerializeField] private float defaultAccelerationMultiplier = 1 + 2f / 10f;
	[SerializeField] private float defaultMaxSpeedMultiplier = 1 + 2f / 15f;

	public void Awake() {
		Instance = this;
	}

	public void IncreaseSpeed() {
		playerLocomotion.Acceleration *= defaultAccelerationMultiplier;
		playerLocomotion.MaxSpeed *= defaultMaxSpeedMultiplier;
	}
}
