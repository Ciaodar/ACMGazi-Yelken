using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawARat : MonoBehaviour
{
    // this script checkes a circle area and 
    // if there is a gameobject with a "Fare" tag, this scipt disables playercontroller and makes the player chase the rat
    // this script is attached to the player(a fox)
    
    public float radius = 5f; // the radius of the circle area
    public LayerMask layerMask; // the layer mask to check for objects
    public float speed = 5f; // the speed of the player
    public float chaseTime = 5f; // the time to chase the rat
    public float chaseDistance = 10f; // the distance to chase the rat
    public float chaseSpeed = 10f; // the speed of the player when chasing the rat
    public float jumpForce = 10f; // the force of the jump
    
    private bool isChasing = false; // if the player is chasing the rat
    private GameObject rat; // the rat gameobject
    private GencTilkiController playerController; // the player controller script
    private Rigidbody2D rb; // the rigidbody of the player
    
    
    void Start()
    {
        playerController = GetComponent<GencTilkiController>(); // get the player controller script
    }
    
    void Update()
    {
        if (isChasing)
        {
            // check if the rat is still in the scene.
            if (rat == null)
            {
                isChasing = false; // stop chasing the rat
                playerController.enabled = true; // enable the player controller
                return;
            }

            // check if rat is on the right or on the left.
            Vector2 direction = ((rat.transform.position - transform.position) * Vector2.right).normalized; // get the direction to the rat
            
            //check if the fox is near a cliff
            Physics.Raycast(transform.position, new Vector2(direction.x, -1), out RaycastHit hit, 1f);
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    //standard chase
                    rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y); // move the player towards the rat
                }
                else
                {
                    //jump to the right
                    rb.AddForce(new Vector2(jumpForce, jumpForce), ForceMode2D.Impulse); // jump to the right
                }
            }
        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerMask); // check for objects in the circle area

            foreach (Collider collider in colliders)
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
}
