using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    [SerializeField] private GrapplingGun gun = default;
    [SerializeField] private GameObject hook = default;

    [Header("Rope animation variables")]
    [SerializeField] private int quality = default;
    [SerializeField] private float damper = default;
    [SerializeField] private float strength = default;
    [SerializeField] private float velocity = default;
    [SerializeField] private float waveCount = default;
    [SerializeField] private float waveHeight = default;
    [SerializeField] private AnimationCurve affectCurve = default;
    [SerializeField] private Transform hookPos = default;
    [SerializeField] private float pullForce = 250f;

    private LineRenderer lineRenderer = default;
    private Vector3 currentGrapplePos = default;
    private Spring spring = default;
    private Transform hookOrigTransform = default;
    private Rigidbody playerRB;

    // Getters
    public GameObject Hook => hook;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
        hookOrigTransform = hook.transform;
        playerRB = Managers.Player.GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        DrawRope();
        
    }


    private void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!gun.IsGrappling())
        {
            currentGrapplePos = gun.GunTip.position;
            hook.transform.SetParent(hookPos);
            hook.transform.localPosition = Vector3.zero;
            hook.transform.localRotation = new Quaternion(-45, 0, 0, 1);
            spring.Reset();
            if (lineRenderer.positionCount > 0)
                lineRenderer.positionCount = 0;
            return;
        }

        if (lineRenderer.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lineRenderer.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        gun.GrapplePoint = hook.transform.position;

        if (Vector3.Distance(gun.GrapplePoint, transform.position) > gun.maxDistance)
        {
            playerRB.AddForce((gun.GrapplePoint - transform.position).normalized * pullForce);
        }

        var grapplePoint = hook.transform.position;
        var gunTipPosition = gun.GunTip.position;
        var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

        currentGrapplePos = Vector3.Lerp(currentGrapplePos, grapplePoint, Time.deltaTime * 12f);

        for (var i = 0; i < quality + 1; i++)
        {
            var delta = i / (float)quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                         affectCurve.Evaluate(delta);

            lineRenderer.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePos, delta) + offset);
        }

        //hook.transform.position = grapplePoint;
    }
}
