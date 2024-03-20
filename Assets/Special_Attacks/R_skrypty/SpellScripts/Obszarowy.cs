using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obszarowy : Spell
{
    [SerializeField] float delay;
    [SerializeField] float radius;
    [SerializeField] LayerMask enemieLayer;


    void Start()
    {
        Invoke(nameof(Attack), delay);
    }



    public override void Attack()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(player.transform.position, radius, enemieLayer);

        foreach (Collider2D target in targets)
        {
            print(target.name);
        }

        Destroy(gameObject);
    }
}