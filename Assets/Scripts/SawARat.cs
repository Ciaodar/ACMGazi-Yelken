using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class SawARat : MonoBehaviour
{
    public float radius = 5f; // Detection radius
    public float chaseSpeed = 5f; // Speed when chasing the rat
    public float runAwaySpeed = 7f; // Speed when running away from the bear
    public float jumpForce = 10f; // Jump force
    
    private bool grounded = false; // Whether the fox is on the ground
    private bool isChasingRat = false; // Whether the fox is chasing a rat
    private bool isRunningFromBear = false; // Whether the fox is running from a bear
    [NonSerialized]public bool isGoingToEdible = false; // Whether the fox is going to an edible item
    
    private GameObject rat; // Reference to the rat
    private GameObject bear; // Reference to the bear
    private Rigidbody2D rb; // Rigidbody of the fox
    private GencTilkiController playerController;
    private PlayerInput _playerInput;// Player controller script
    private CliffController CC;
    private Animator _animator; // Animator of the fox

    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    
    BoxCollider2D touchingCol;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    
    void Start()
    {
        playerController = GetComponent<GencTilkiController>();
        rb = GetComponent<Rigidbody2D>();
        CC = GetComponentInChildren<CliffController>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        touchingCol = GetComponent<BoxCollider2D>();

    }

    private void FixedUpdate()
    {
        grounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        playerController.IsGrounded = grounded;
    }

    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(radius, 3), 0);

        bool ratInRange = false;
        bool bearInRange = false;
        bool edibleInRange = false;
        GameObject edible = null;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Fare"))
            {
                rat = collider.gameObject;
                ratInRange = true;
            }
            else if (collider.CompareTag("Bear"))
            {
                bear = collider.gameObject;
                bearInRange = true;
            }
            else if (collider.CompareTag("Edible"))
            {
                edible = collider.gameObject;
                edibleInRange = true;
            }
        }

        if (!isChasingRat && !isRunningFromBear && !isGoingToEdible)
        {
            if (edibleInRange )
            {
                isGoingToEdible = true;
                playerController.enabled = false;
                _playerInput.enabled = false;
                playerController.IsMoving = true;
                playerController.IsRunning = true;
                StartCoroutine(GoToEdible(edible));
            }
            else if (bearInRange)
            {
                isRunningFromBear = true;
                playerController.enabled = false;
                _playerInput.enabled = false;
                playerController.IsMoving = true;
                playerController.IsRunning = true;
                StartCoroutine(RunAwayFromBear());
            }
            else if (ratInRange)
            {
                isChasingRat = true;
                playerController.enabled = false;
                _playerInput.enabled = false;
                playerController.IsMoving = true;
                playerController.IsRunning = true;
                StartCoroutine(ChaseTheRat());
            }
        }
    }
    
    private IEnumerator GoToEdible(GameObject edible)
    {
        float distance = (edible.transform.position - transform.position).magnitude;
        while (edible != null && distance<radius && isGoingToEdible )
        {
            if (!isGoingToEdible) // ekstra güvenlik
                yield break;
            
            Vector2 direction = (edible.transform.position - transform.position).normalized;
            if (direction.x < 0)
                playerController.IsFacingRight = false;
            else
                playerController.IsFacingRight = true;

            rb.velocity = new Vector2(chaseSpeed * direction.x, rb.velocity.y);

            if (CC.nearCliff && grounded)
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                playerController.IsGrounded = false;
                grounded = false;
                _animator.SetTrigger("jumpTrigger");
            }

            yield return null;
        }

        isGoingToEdible = false;
        playerController.enabled = true;
        _playerInput.enabled = true;
        playerController.IsMoving = false;
        playerController.IsRunning = false;
    }

    private IEnumerator ChaseTheRat()
    {
        float elapsedTime = 0f; // the elapsed time of the chase
        float runtime=3f;
        if(rat.GetComponent<Rat>()!=null)
            runtime = rat.GetComponent<Rat>().runTime; // the time to chase the rat
        if (rat.GetComponent<Rabbit>()!=null)
            runtime = rat.GetComponent<Rabbit>().runTime; // the time to chase the rabbit
        Vector2 direction =
            (rat.transform.position - transform.position).normalized; // get the direction to chase the rat
        while (elapsedTime < runtime)
        {
            if (direction.x < 0)
                playerController.IsFacingRight = false;
            else playerController.IsFacingRight = true;
            playerController.enabled = false;
            _playerInput.enabled = false;
            playerController.IsMoving = true;
            playerController.IsRunning = true;
            {
                rb.velocity = new Vector2(chaseSpeed * direction.x, rb.velocity.y); // move the fox towards the rat
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
            rb.velocity= new Vector2(0, rb.velocity.y); // move the fox towards the rat
            playerController.enabled = true;
            _playerInput.enabled = true;
            isChasingRat = false;
            playerController.IsMoving = false;
            playerController.IsRunning = false;
        }
    }

    private IEnumerator RunAwayFromBear()
        {
            float elapsedTime = 0f; // the elapsed time of the run
            float runtime = bear.GetComponent<Bear>().runtime;
            Vector2 direction = ((transform.position - bear.transform.position) * Vector2.right).normalized; // get the direction to run away from the player
            if (direction.x < 0)
                playerController.IsFacingRight = false;
            else playerController.IsFacingRight = true;
            while (elapsedTime < runtime)
            {
                playerController.enabled = false;
                _playerInput.enabled = false;
                playerController.IsMoving = true;
                playerController.IsRunning = true;
            
                rb.velocity = new Vector2(direction.x * runAwaySpeed , rb.velocity.y); // move the rat away from the player
                elapsedTime += Time.deltaTime; // increase the elapsed time
                if (CC.nearCliff && grounded)
                {
                    rb.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
                    playerController.IsGrounded = false;
                    grounded = false;
                    _animator.SetTrigger("jumpTrigger");
                }
                yield return null; // wait for the next frame
            }
            playerController.enabled = true;
            _playerInput.enabled = true;
            isRunningFromBear = false;
            playerController.IsMoving = false;
            playerController.IsRunning = false;
        }
    

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector2(radius,3));
        }
}