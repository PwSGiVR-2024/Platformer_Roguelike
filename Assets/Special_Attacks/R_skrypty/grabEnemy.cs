using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabEnemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float staminaCost;
    [SerializeField] float damage;
    [SerializeField] float pullForce;
    [SerializeField] float speed;
    [SerializeField] LayerMask enemyLayer;

    Vector2 dir;

    Rigidbody2D rb;

    void Start()
    {
        // TO DO ustawic kierunek rzucania

        dir = player.transform.right;

        rb = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
        rb.velocity = dir.normalized * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Enemy" && collision.tag != "Player")
            Destroy(gameObject);

        if (collision.tag != "Enemy")
            return;

        Vector2 dir2 = transform.position - collision.transform.position;
        collision.GetComponent<Rigidbody2D>().AddForce(dir2 * pullForce, ForceMode2D.Impulse);

        Destroy(gameObject);

        // druga wersja - przyciagniecie do gracza

        /*
         * przyciagniecie do gracza

dotyka enemie
uruchamia korutyne gdzie:
na poczatku wylacza ruch gracza
potem w petli porusza go do punktu przed miejscem gdzie gracz rzucil spella
	ten pkt to pozycja gracza +- wartosc x (lub mo�e y) - odleglosc w ktorej ma byc od gracza
na koniec petli wlacza ruch przeciwnikowi*/


    }

    void PullAlittleToPlayer(float pullForce)
    {
        // pociagniecie w kierunku gracza
    }

    void PullToPlayer()
    {
        // przyciagniecie do gracza
    }
}