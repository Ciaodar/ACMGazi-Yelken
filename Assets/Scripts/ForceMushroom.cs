using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMushroom : MonoBehaviour
{
    public Vector3 forceDirection = new Vector3(0, 10f, 0);
    private ForceMode2D forceMode = ForceMode2D.Impulse; 
    public bool destroyOnHit = true; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(forceDirection, forceMode);
            }

            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }}
