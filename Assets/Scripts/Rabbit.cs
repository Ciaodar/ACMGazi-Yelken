using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Rabbit : MonoBehaviour
{
    public float radius; // the radius of the circle area
    public float speed; // the speed of the rat
    public float runTime; // the time to run away from the player
    public float fadeTime; // the time to fade away
    public float fadeSpeed; // the speed of the fade
    public float jumpForce; // the jump force of the rat

    private bool grounded = false;
    private bool isRunning = false; // if the rat is running away from the player
    private GameObject player; // the player gameobject
    private SpriteRenderer spriteRenderer; // the sprite renderer of the rat
    private Rigidbody2D rb;
    private Animator _animator; // Animator of the fox
    private GencTilkiController playerController;
    private Color originalColor; // the original color of the rat
    private float fadeAmount;
    private CliffController CC;
    
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    
    BoxCollider2D touchingCol;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // get the sprite renderer of the rat
        touchingCol = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>(); // get the rigidbody of the rat
        playerController = GetComponent<GencTilkiController>();
        CC = GetComponentInChildren<CliffController>();
        _animator = GetComponent<Animator>(); // get the animator of the rat
        playerController = GetComponent<GencTilkiController>();
        originalColor = spriteRenderer.color; // get the original color of the rat
    }

    private void Update()
    {
        _animator.SetBool("isRunning", isRunning);
        _animator.SetBool("grounded", grounded);
        
        if (rb.velocity.x < 0) transform.rotation=Quaternion.Euler(0,180,0);
        else transform.rotation=Quaternion.Euler(0,0,0);
        
        // check if the player is in the circle area
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(radius,3),0);
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
        Vector2 direction = ((transform.position - player.transform.position) * Vector2.right).normalized; // get the direction to run away from the player
        while (elapsedTime < runTime)
        {
            rb.velocity = new Vector2(direction.x * speed , rb.velocity.y); // move the rat away from the player
            elapsedTime += Time.deltaTime; // increase the elapsed time
            if (CC.nearCliff && grounded)
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                playerController.IsGrounded = false;
                grounded = false;
                _animator.SetTrigger("jumpTrigger");
            }
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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // set the color of the gizmos
        Gizmos.DrawWireCube(transform.position, new Vector2(radius,3)); // draw a wire sphere around the player
        
        Gizmos.color = Color.gray; // set the color of the gizmos
        
    }
}
