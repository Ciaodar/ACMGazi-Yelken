using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerRespawn>().SetCheckpoint(transform.position);
            animator.SetTrigger("Check");
            GencTilkiController gtc = other.GetComponent<GencTilkiController>();
            gtc.effect = 1f;
            
            enabled = false; // Disable the checkpoint after it's been activated
        }
    }
}
