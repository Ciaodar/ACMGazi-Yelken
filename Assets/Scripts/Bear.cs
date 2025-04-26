using System;
using UnityEngine;
using System.Collections;
public class Bear : MonoBehaviour
{
    public float radius = 5f; // Detection radius
    public float chaseSpeed = 3f; // Speed of the bear while chasing
    public float runtime = 3f; // Time to chase the player

    private GameObject player; // Reference to the player (fox)
    private bool isChasing = false; // Whether the bear is chasing the player
    private Rigidbody2D rb; // Rigidbody of the bear
    private Animator animator; // Animator of the bear

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        animator.SetBool("isChasing", isChasing); // Set the animator parameter for chasing
        
        
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position,  new Vector2(radius,3) ,0); // Check for objects in the circle area

        bool playerInRange = false;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player")) // Check if the object is the player
            {
                player = collider.gameObject;
                playerInRange = true;
                break;
            }
        }

        if (playerInRange && !isChasing && player!=null)
        {
            isChasing = true; // Start chasing the player
            StartCoroutine(ChasePlayer());
        }

    }
    private IEnumerator ChasePlayer()
    {
        float elapsedTime = 0f; // the elapsed time of the chase
        Vector2 direction =
            (player.transform.position - transform.position).normalized; // get the direction to chase the rat
        while (elapsedTime < runtime)
        {
            elapsedTime += Time.deltaTime; // increase the elapsed time
            rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y); // move the bear towards the player
            yield return null; // wait for the next frame
        }
        isChasing = false; // Stop chasing the player
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set the color of the gizmos
        Gizmos.DrawWireCube(transform.position, new Vector2(radius,3)); // Draw a wire sphere for the detection radius
    }
}