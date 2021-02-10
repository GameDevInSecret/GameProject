using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public Vector3 forceDir = Vector2.right;
    public ParticleSystem sparks;
    
    private Rigidbody2D shieldRb;
    private Vector3 arrowRot;
    private bool justThrown = true;

    private bool hasSparked = false;
    // Start is called before the first frame update
    void Start()
    {
        // shieldRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Awake()
    {
        shieldRb = GetComponent<Rigidbody2D>();
    }

    public void ThrowShield()
    {
        if (justThrown)
        {
            shieldRb.AddForce(forceDir * 30 , ForceMode2D.Impulse);
            shieldRb.AddTorque(5, ForceMode2D.Impulse);
            justThrown = false;
        }
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
}