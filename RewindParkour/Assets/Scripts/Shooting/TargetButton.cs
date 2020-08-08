using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TargetButton : Target
{
    public UnityEvent onShoot;
    public override void TakeDamage(float amount)
    {
        onShoot.Invoke();
    }

    public override void Die()
    {
       return;
    }
}
