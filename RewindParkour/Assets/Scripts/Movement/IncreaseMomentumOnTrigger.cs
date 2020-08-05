using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMomentumOnTrigger : MonoBehaviour {
	private void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.GetComponent<PlayerLocomotion>() != null) {
			PlayerMomentumController.Instance.IncreaseMomentum();
		}
	}
}
