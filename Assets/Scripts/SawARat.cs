using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SawARat : MonoBehaviour
{
    // this script checkes a circle area and 
    // if there is a gameobject with a "Fare" tag, this scipt disables playercontroller and makes the player chase the rat
    // this script is attached to the player(a fox)
    
    public float radius = 5f; // the radius of the circle area
    public float chaseSpeed = 5f; // the speed of the player when chasing the rat
    public float jumpForce = 10f; // the force of the jump
    public bool grounded = false; // if the player is on the ground
    
    
    private bool isChasing = false; // if the player is chasing the rat
    private GameObject rat; // the rat gameobject
    private GencTilkiController playerController; // the player controller script
    private Rigidbody2D rb; // the rigidbody of the player
   
    
    
    void Start()
    {
        playerController = GetComponent<GencTilkiController>(); // get the player controller script
        rb = GetComponent<Rigidbody2D>(); // get the rigidbody of the player
    }
    
    void Update()
    {
        grounded= Physics2D.Raycast(transform.position, Vector2.down, 1.1f, LayerMask.GetMask("Ground")); // check if the player is on the ground
        
        if (isChasing)
        {
            // check if the rat is still in the scene.
            if (rat == null)
            {
                isChasing = false; // stop chasing the rat
                playerController.enabled = true; // enable the player controller
                return;
            }
            
            //rotate the player to the direction of the rat on y axis
            Vector3 direction = (rat.transform.position - transform.position).normalized; // get the direction to the rat
            if (direction.x > 0)
            {
                transform.rotation=Quaternion.Euler(0,0,0); // set the player to face the rat
            }
            else if (direction.x < 0)
            {
                transform.rotation=Quaternion.Euler(0,180,0); // set the player to face the rat
            }
            // Check for a cliff
            bool isNearCliff = !Physics2D.Raycast(transform.position + new Vector3(transform.localScale.x * 0.5f, 0, 0), Vector2.down, 1.1f, LayerMask.GetMask("Ground"));

            if (isNearCliff && grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); // make the fox jump
            }
            
            rb.velocity = new Vector2(chaseSpeed * direction.x, rb.velocity.y); // move the player towards the rat
            
        }
        else
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius); // check for objects in the circle area

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Fare")) // check if the object has a "Fare" tag
                {
                    rat = collider.gameObject; // get the rat gameobject
                    isChasing = true; // start chasing the rat
                    playerController.enabled = false; // disable the player controller
                    break;
                }
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // set the color of the gizmos
        Gizmos.DrawWireSphere(transform.position, radius); // draw a wire sphere around the player
     
        Gizmos.color = Color.green; // set the color of the gizmos
        Gizmos.DrawLine(transform.position,transform.position+new Vector3(-2,-1,0).normalized*5);
        Gizmos.DrawLine(transform.position,transform.position+new Vector3(2,-1,0).normalized*5);
    }
}
