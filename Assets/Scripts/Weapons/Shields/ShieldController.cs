using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ShieldController : MonoBehaviour
{
    private Vector3 forceDirection;
    
    private Rigidbody2D shieldRb;
    private Vector3 arrowRot;

    public ParticleSystem sparks;
    //private bool hasSparked = false;

    private void Awake()
    {
        shieldRb = GetComponent<Rigidbody2D>();
        
        forceDirection = Vector3.right;
    }

    public void ThrowShield()
    {
        //shieldRb.AddForce(forceDir * 30, ForceMode2D.Impulse);
        shieldRb.AddForce(forceDirection * 30, ForceMode2D.Impulse);
        shieldRb.AddTorque(5, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if (other.gameObject.CompareTag("Ground"))
        // {
        //     ContactPoint2D contactPoint = other.GetContact(0);
        //
        //     Vector3 lerpedPos = Vector3.Lerp(contactPoint.point, transform.position, 0.2F);
        //     
        //     Quaternion rot = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
        //
        //     Instantiate(sparks, lerpedPos, rot);
        //     // hasSparked = true;
        // }
    }

    public void SetForceDirection(Vector3 direction)
    {
        forceDirection = direction;
    }
}