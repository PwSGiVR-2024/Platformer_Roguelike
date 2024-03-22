using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] healthController health;
    [SerializeField] SpawnSpike spike;
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        //kodzik nie dzia�a przy spadaj�cych kolcach do zrobienia jeszcze spawnowanie przy najblizszej pod�odze
        if (collision.transform.tag == "Player" && health.GetHealth() > 1)
        {
            Debug.Log("uderzono gracza");
            health.MinusHP(1);
        }
        else if (collision.transform.tag == "Player" && health.GetHealth() <= 1)
        {
            health.MinusHP(1);
            health.RestartHealth();
            //TO DO: co po zgnieciu
        }
    }
}
