using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 checkpointPosition;
    private RadialFade radialFade;
    private AudioSource audioSource;

    public AudioClip deathSound;

    private Vector3 deathPosition;
    void Start()
    {
        checkpointPosition = transform.position;
        radialFade = FindObjectOfType<RadialFade>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetCheckpoint(Vector2 newCheckpoint)
    {
        checkpointPosition = newCheckpoint;
    }

    public void Respawn()
    {
        transform.position = checkpointPosition+ new Vector2(0, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Deadly"))
        {
            Die();
        }
    }
    
    private void Die()
    {
        deathPosition = transform.position;

        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);

        if (radialFade != null)
            radialFade.StartFadeOut(deathPosition);

        Invoke(nameof(RespawnAfterFade), 1.5f);
    }
    
    private void RespawnAfterFade()
    {
        Respawn();
        if (radialFade != null)
            radialFade.StartFadeIn(transform.position);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Respawn();
        }
    }
}
