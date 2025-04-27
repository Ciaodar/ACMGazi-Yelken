using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bogurtlen : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GencTilkiController gtc = other.GetComponent<GencTilkiController>();
            if (gtc)
            {
                gtc.effect = 5f;
            }
            Destroy(gameObject);
        }
    }
}
