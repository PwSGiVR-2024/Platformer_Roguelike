using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightingChain : Spell
{
    [SerializeField] float searchRadius;
    [SerializeField] float jumpsNumberMax;
    [SerializeField] float speed;
    [SerializeField] LayerMask enemyLayer;

    [SerializeField]float jumpsNumberCurrent;

    List<string> attacked = new List<string>();

    Collider2D closestEnemy = null;

    // TO DO        PIERWSZY ENEMY TEZ WYSZUKIWANY (LUB NIE)

    void Update()
    {
        if (closestEnemy != null)
        {
            castDirection = closestEnemy.transform.position - transform.position;
            print("awd");
        }

        rb.velocity = castDirection.normalized * speed * Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag != "Enemy" && collision.tag != "Player")
        //Destroy(gameObject);
        
        if (collision.tag != "Enemy")            
            return;

        foreach (string nm in attacked)
        {
            if (collision.name == nm)
            {
                return;
            }
        }

        jumpsNumberCurrent++;

        if (jumpsNumberCurrent >= jumpsNumberMax)
        {
            Destroy(gameObject);
            return;
        }


        // funkcja zadajaca dmg przeciwnikowi here

        attacked.Add(collision.name);

        Collider2D[] closeEnemies = Physics2D.OverlapCircleAll(transform.position, searchRadius, enemyLayer);

        float minDistance = 100;
        bool goOn = false;
        

        foreach (Collider2D col in closeEnemies)
        {
            //print("hre");
            foreach (string nm in attacked)
            {
                if (col.name == nm)
                {
                    goOn = true;
                    print(col.name);
                    break;
                }      
            }

            if (goOn)
            {
                goOn = false;
                continue;
            }
            print(minDistance);
            if ((transform.position - col.transform.position).magnitude < minDistance)
            {
                print("ad");
                minDistance = (transform.position - col.transform.position).magnitude;
                closestEnemy = col;
                
            }
        }

    }

    public override void Attack()
    {
        // tutaj funkcja z OnTrigger
    }
}
