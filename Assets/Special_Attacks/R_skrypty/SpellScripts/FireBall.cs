using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public class FireBall : Spell
{
    [SerializeField] float damageOnExplode;
    [SerializeField] float pushbackForce;
    [SerializeField] float dmgRadius;
    [SerializeField] float speed;
    [SerializeField] LayerMask enemyLayer;


    void Update()
    {
        rb.velocity = castDirection.normalized * speed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Enemy" && collision.tag != "Player")
        {
            Attack();

            return;
        }


        if (collision.tag != "Enemy")
            return;


        // zadanie dmg dla pierwszy target here
        Attack();

    }

    public override void Attack()
    {
        Collider2D[] closeEnemies = Physics2D.OverlapCircleAll(transform.position, dmgRadius, enemyLayer);

        // przykladowe wartosci
        AttackDetails attackDetails = new AttackDetails();
        attackDetails.position = transform.position;
        attackDetails.damageAmount = 5;
        attackDetails.stunDamageAmount = 5;

        foreach (Collider2D c in closeEnemies)
        {
            c.transform.parent.GetComponent<Entity>().DamageGet(attackDetails);
        }
        
        Destroy(gameObject);
    }

    public void moreDMG( float m)
    {
        damage += m;
    }
}
