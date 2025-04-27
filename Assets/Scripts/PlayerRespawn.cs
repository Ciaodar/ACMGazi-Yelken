using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 checkpointPosition;
    private RadialFade radialFade;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private Transform camTransform;

    public AudioClip deathSound;

    private Vector3 deathPosition;
    void Start()
    {
        checkpointPosition = transform.position;
        radialFade = FindObjectOfType<RadialFade>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        if (Camera.main != null) camTransform = Camera.main.transform;
    }

    public void SetCheckpoint(Vector2 newCheckpoint)
    {
        checkpointPosition = newCheckpoint;
    }

    public void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = checkpointPosition;
        camTransform.position = new Vector3(checkpointPosition.x, checkpointPosition.y, camTransform.position.z);
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
