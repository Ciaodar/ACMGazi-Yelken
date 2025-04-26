using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertigoMushroom : MonoBehaviour
{
    public float vertigoDuration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerVertigo playerVertigo = other.GetComponent<PlayerVertigo>();
            if (playerVertigo)
            {
                playerVertigo.StartVertigo(vertigoDuration);
            }
            Destroy(gameObject);
        }
    }
}
