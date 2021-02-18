using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnTakeDamage(Damager damager, Damageable damageable) {print("ENEMY HIT FOR " + damager.damage + " DAMAGE!");}
    public void OnDie(Damager damager, Damageable damageable) {print("Enemy die");}
    public void OnGainHealth(int val, Damageable damageable) {print("Enemy gained " + val + " health");}
    
    public void OnDamageableEvent( Damager damager, Damageable damageable) {print("ENEMY HITTING SOMETHING!");}
    public void OnNonDamageableEvent(Damager damager) {print("ENEMY HITTING SOMETHING THAT ISN'T DAMAGEABLE");}
}
