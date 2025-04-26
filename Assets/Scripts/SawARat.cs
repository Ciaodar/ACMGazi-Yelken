using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class SawARat : MonoBehaviour
{
    public float radius = 5f; // Detection radius
    public float chaseSpeed = 5f; // Speed when chasing the rat
    public float runAwaySpeed = 7f; // Speed when running away from the bear
    public float jumpForce = 10f; // Jump force
    public float runawaytime = 3f;
    
    private bool grounded = false; // Whether the fox is on the ground
    private bool isChasingRat = false; // Whether the fox is chasing a rat
    private bool isRunningFromBear = false; // Whether the fox is running from a bear
    
    private GameObject rat; // Reference to the rat
    private GameObject bear; // Reference to the bear
    private Rigidbody2D rb; // Rigidbody of the fox
    private GencTilkiController playerController;
    private PlayerInput _playerInput;// Player controller script
    private CliffController CC;
    private Animator _animator; // Animator of the fox

    void Start()
    {
        playerController = GetComponent<GencTilkiController>();
        rb = GetComponent<Rigidbody2D>();
        CC = GetComponentInChildren<CliffController>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        grounded = playerController.IsGrounded;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(radius,3),0);

        bool ratInRange = false;
        bool bearInRange = false;

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
        }

        if (bearInRange && !isChasingRat && !isRunningFromBear)
        {
            isChasingRat = false;
            isRunningFromBear = true;
            playerController.enabled = false;
            _playerInput.enabled = false;
            playerController.IsMoving = true;
            playerController.IsRunning = true;
            StartCoroutine(RunAwayFromBear());
        }
        else if (ratInRange && !isChasingRat && !isRunningFromBear)
        {
            isRunningFromBear = false;
            isChasingRat = true;
            playerController.enabled = false;
            _playerInput.enabled = false;
            playerController.IsMoving = true;
            playerController.IsRunning = true;
            StartCoroutine(ChaseTheRat());
        }
    }

    private IEnumerator ChaseTheRat()
    {
        float elapsedTime = 0f; // the elapsed time of the chase
        float runtime = rat.GetComponent<Rat>().runTime;
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