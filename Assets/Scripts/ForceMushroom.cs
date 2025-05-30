using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMushroom : MonoBehaviour
{
    public Vector2 forceDirection = new Vector2(10, 10f);
    private ForceMode2D forceMode = ForceMode2D.Impulse;
    public float sure=1f;
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
            gameObject.tag="Untagged"; // Change the tag to "Untagged" after the player hits it
            other.GetComponent<SawARat>().isGoingToEdible = false;
            other.GetComponent<GencTilkiController>().isUnderForce = true;
            other.GetComponent<GencTilkiController>().forceSure = sure;
            other.GetComponent<GencTilkiController>().StartCoroutine("ResetForceState");
        }
    }}
