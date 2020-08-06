using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetractableTarget : Target
{  
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private Sound hitSound = default;

    private bool retracting;
    public override void Die()
    {
        StartCoroutine(Retract());
    }

    private IEnumerator Retract()
    {
        if (retracting) yield break;

        transform.Rotate(new Vector3(90, 0, 0));
        retracting = true;
        Managers.AudioManager.PlayOneShot("Ding");

        yield return new WaitForSeconds(waitTime);

        transform.Rotate(new Vector3(-90, 0, 0));
        retracting = false;

    }
}
