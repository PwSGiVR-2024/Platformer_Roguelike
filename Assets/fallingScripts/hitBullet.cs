using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitBullet : MonoBehaviour
{
    [SerializeField] private float force;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            // Zatrzymaj ruch gracza
            rb.velocity = Vector2.zero;

            // Odbij gracza w przeciwnym kierunku z mniejsz� pr�dko�ci�
            rb.velocity = new Vector2(-rb.velocity.x, -rb.velocity.y) * 0.5f;
            rb.AddForce(Vector2.right * 100000);

            // Uruchom efekty wizualne
            // np. zmie� kolor gracza na czerwony
            collision.GetComponent<SpriteRenderer>().color = Color.red;
            //collision.GetComponent<Rigidbody2D>().AddForce(-collision.transform.rotation.eulerAngles);
        }
    }
}