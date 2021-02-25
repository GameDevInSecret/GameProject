using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using Vector2 = UnityEngine.Vector2;

public class Damager : MonoBehaviour
{
    
    [Serializable]
    public class DamageableEvent : UnityEvent<Damager, Damageable>
    { }


    [Serializable]
    public class NonDamageableEvent : UnityEvent<Damager>
    { }
    
    [Header("Properties")]
    public int damage = 10;
    public bool canDamage;
    public bool ignoreInvincibility = false;
    public LayerMask hittableLayers;
    
    [Header("Hit Box")]
    [Tooltip("When true, the hit box will follow a child empty GameObject named 'Hitbox Center'")]
    public bool movable;
    [Tooltip("When movable is false, this offset will be a fixed offset relative to the center of the object.")]
    public Vector2 offset;
    public Vector2 size;
    [Tooltip("After colliding with a damageable, the time before the damager becomes active again")]
    public float backoffTime = 0.25F;
    [Tooltip("If set true, the damager will remain inactive until manually set to active.")]
    public bool indefiniteBackoff = false;
    public bool active = true;
    
    [Header("Events")]
    public DamageableEvent OnDamageableEvent;
    public NonDamageableEvent OnNonDamageableEvent;
    
    [Header("Debugging")]
    public GameObject debugPointPrefab;
    private GameObject pointADebug;
    private GameObject pointBDebug;
    
    protected Transform m_DamagerTransform;
    protected ContactFilter2D m_AttackContactFilter;
    protected Collider2D[] m_AttackOverlapResults = new Collider2D[10];

    [SerializeField] private Vector2 pointA;
    [SerializeField] private Vector2 pointB;
    
    void Awake()
    {
        m_AttackContactFilter.layerMask = hittableLayers;
        m_AttackContactFilter.useLayerMask = true;
        m_DamagerTransform = transform;

        pointADebug = Instantiate(debugPointPrefab, transform.position, debugPointPrefab.transform.rotation);
        pointBDebug = Instantiate(debugPointPrefab, transform.position, debugPointPrefab.transform.rotation);
    }
    
    void FixedUpdate()
    {
        if (!canDamage) return;

        if (movable) offset = transform.Find("Hitbox Center").transform.position - transform.position;

        // Get the scale of the transform
        // Get the direction the hit box is facing and multiply it component-wise with the scale
        // Multiply the size and scale so that if the parent is scaled up or down, the hitbox scales with it.
        Vector2 scale = m_DamagerTransform.lossyScale;
        Vector2 facingOffset = Vector2.Scale(offset, scale);
        Vector2 scaledSize = Vector2.Scale(size, scale);

        // From the hitbox rectangle, get one corner and the corner opposite it for the OverlapArea function
        // Point A is the position of the transform, moved left or right by facingOffset (this is the center of the hitbox
        // then moved to the corned by subtracting half the length of the scaledSize in the x and y directions.
        // Point B is just Point A plus the size in the x and y direction of the scaledSize vector.
        pointA = (Vector2) m_DamagerTransform.position + facingOffset - scaledSize * 0.5F;
        pointB = pointA + scaledSize;

        pointADebug.transform.position = pointA;
        pointBDebug.transform.position = pointB;
        
        // print("A: " + pointA + " B: " + pointB);

        int hitCount = Physics2D.OverlapArea(pointA, pointB, m_AttackContactFilter, m_AttackOverlapResults);

        // print(hitCount);
        
        for ( int i = 0; i < hitCount; ++i )
        {
            Damageable damageable = m_AttackOverlapResults[i].GetComponent<Damageable>();

            if (damageable && active)
            {
                // OnDamageableEvent.Invoke(this, damageable);
                damageable.OnTakeDamage.Invoke(this, damageable);
                active = false;
                if (!indefiniteBackoff)
                {
                    StartCoroutine(BackoffCallback());
                }
            }
            else
            {
                // OnNonDamageableEvent.Invoke(this);
            }
        }
    }

    private void OnDestroy()
    {
        Destroy(pointADebug);
        Destroy(pointBDebug);
    }

    IEnumerator BackoffCallback()
    {
        yield return new WaitForSeconds(backoffTime);
        active = true;
    }

    public bool DamagerActive() { return canDamage; }
    public void EnableDamager() { canDamage = true; }
    public void DisableDamager() { canDamage = false; }
}
