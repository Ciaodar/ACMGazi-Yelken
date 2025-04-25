using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    // this script checks a circle area and
    // if there is a gameobject with a player tag, runs away from the player for a certain amount of time. then slowly fades and destroys itself
    // this script is attached to the rat
    public float radius; // the radius of the circle area
    public LayerMask layerMask; // the layer mask to check for objects
    public float speed; // the speed of the rat
    public float runTime; // the time to run away from the player
    public float fadeTime; // the time to fade away
    public float fadeSpeed; // the speed of the fade
    
    private bool isRunning = false; // if the rat is running away from the player
    private GameObject player; // the player gameobject
    private SpriteRenderer spriteRenderer; // the sprite renderer of the rat
    private Rigidbody2D rb;
    private Color originalColor; // the original color of the rat
    private float fadeAmount; // the amount of fade

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // get the sprite renderer of the rat
        rb = GetComponent<Rigidbody2D>(); // get the rigidbody of the rat
        originalColor = spriteRenderer.color; // get the original color of the rat
    }

    private void Update()
    {
        //patrol when player is not around
        if (!isRunning)
        {
            
        }
        
        
        
        // check if the player is in the circle area
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                player = collider.gameObject; // get the player gameobject
                if (!isRunning)
                {
                    isRunning = true; // start running away from the player
                    StartCoroutine(RunAway());
                }
            }
        }
    }
    
    private IEnumerator RunAway()
    {
        float elapsedTime = 0f; // the elapsed time of the run
        Vector2 direction = (transform.position - player.transform.position)*Vector2.right; // get the direction to run away from the player
        while (elapsedTime < runTime)
        {
            rb.velocity = new Vector2(direction.x * speed , rb.velocity.y); // move the rat away from the player
            elapsedTime += Time.deltaTime; // increase the elapsed time
            yield return null; // wait for the next frame
        }
        StartCoroutine(FadeAway()); // start fading away after running
    }
    private IEnumerator FadeAway()
    {
        float elapsedTime = 0f; // the elapsed time of the fade
        while (elapsedTime < fadeTime)
        {
            fadeAmount = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime); // calculate the fade amount
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, fadeAmount); // set the color of the rat
            elapsedTime += Time.deltaTime * fadeSpeed; // increase the elapsed time
            yield return null; // wait for the next frame
        }
        Destroy(gameObject); // destroy the rat gameobject after fading away
    }
}
