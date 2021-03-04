using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Damager _damager;
    // Start is called before the first frame update
    void Start()
    {
        _damager = GetComponent<Damager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTakeDamage(Damager damager, Damageable damageable)
    {
        // print("ENEMY HIT FOR " + damager.damage + " DAMAGE!");
        damageable.TakeDamage(damager, damager.ignoreInvincibility);
    }

    public void OnDie(Damager damager, Damageable damageable)
    {
        print("ENEMY DEAD");
        Destroy(gameObject);
    }
    
    public void OnGainHealth(int val, Damageable damageable) {print("Enemy gained " + val + " health");}
    
    public void OnDamageableEvent( Damager damager, Damageable damageable) {print("ENEMY HITTING SOMETHING!");}
    public void OnNonDamageableEvent(Damager damager) {print("ENEMY HITTING SOMETHING THAT ISN'T DAMAGEABLE");}
}
