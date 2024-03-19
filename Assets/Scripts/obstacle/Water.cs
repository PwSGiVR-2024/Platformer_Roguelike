using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] healthController health;
    private void OnCollisionEnter2D(Collision2D collision)
    {
    if(collision.transform.name == "Player")
        {
            StartCoroutine(ApplyDamageOverTime());
        }      
    }
    private IEnumerator ApplyDamageOverTime()
    {
        // P�tla niesko�czona
        while (true)
        {
            // Odczekaj okre�lony czas
            yield return new WaitForSeconds(1);

            // Odj�cie �ycia gracza
            health.MinusHP(1);
        }   
    }
}
