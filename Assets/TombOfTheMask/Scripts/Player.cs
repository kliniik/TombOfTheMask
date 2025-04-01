using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip jumpSound;

    [SerializeField]
    private AudioClip hurtSound;


    // to be used in Raycast
    private Collider2D mCollider;
    public LayerMask Walls;

    public float speed = 5.0f; // can be modified
    public float detectionDistance = 0.1f; // distance from the walls
    
    private int heightReached = 0;
    private float lastBlockY;

    private Vector2 movementDirection = Vector2.zero; // we start with no movement
    private bool isGrounded = true; // we start with the player on the ground
    
   
    void Start()
    {
        mCollider = GetComponent<Collider2D>();
        lastBlockY = Mathf.Floor(transform.position.y);

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("ERROR: Animator not found");
        }

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("ERROR: AudioSource not found on the player.");
        }
    }

    void Update()
    {
        if (!isGrounded) return;

        float hMovement = Input.GetAxis("Horizontal"); // horizontal movement
        float vMovement = Input.GetAxis("Vertical"); // vertical movement

        movementDirection = new Vector2(hMovement, vMovement).normalized;

        if (movementDirection != Vector2.zero)
        {
            //RotatePlayer(movementDirection);
            MovePlayer(movementDirection);
        }
    }

    // function to move the player
    private void MovePlayer(Vector2 moveDirection)
    {
        if (moveDirection == Vector2.zero || !isGrounded) return; // if there's no movement, don't do anything

        if (!DetectWall(moveDirection))
        {
            // play the audio sound
            audioSource.PlayOneShot(jumpSound);

            // it works for both - WSAD and arrow keys
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                StartCoroutine(MoveUntilWall(Vector2.up));
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                StartCoroutine(MoveUntilWall(Vector2.down));
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                StartCoroutine(MoveUntilWall(Vector2.left));
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                StartCoroutine(MoveUntilWall(Vector2.right));
            }
        }
    }

    // funtion to rotate the player 
    private void RotatePlayer(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        // we use the negative value of y to make player's feet touch the ground
        float angle = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private bool DetectWall(Vector2 direction)
    {
        LayerMask layerMask = LayerMask.GetMask("Walls", "Coins", "Spikes"); // we only care about colliding with those 3 layers

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionDistance, layerMask);
        return hit.collider != null;
    }

    // coroutine to move the player until it hits a wall
    private IEnumerator MoveUntilWall(Vector2 direction)
    {
        isGrounded = false;

        RotatePlayer(direction); // rotate the player to the direction it's moving
        
        while (!DetectWall(direction))
        {
            animator.SetTrigger("Move"); // play the moving animation
            transform.position += (Vector3)direction * speed * Time.deltaTime;
            calculateHeight();
            yield return null;
        }

        isGrounded = true;
    }


    // function to detect triggers 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BigCoin") || collision.CompareTag("SmallCoin"))
        {
            Coins coin = collision.GetComponent<Coins>();
            if (coin != null)
            {
                coin.Collect();

                int points = (collision.CompareTag("BigCoin")) ? 5 : 1;
                GameManager.Instance.AddPoints(points);
            }
        }
        else if (collision.CompareTag("Spikes"))
        {
            audioSource.PlayOneShot(hurtSound);
            animator.SetTrigger("Damage");
            GameManager.Instance.GameOver();
        }
    }

    // calculate player's height
    private void calculateHeight()
    {
        float currentBlockY = Mathf.Floor(transform.position.y);

        // moving up
        if (currentBlockY > lastBlockY)
        {
            heightReached++;
            lastBlockY = currentBlockY;
        }
        // moving down
        else if (currentBlockY < lastBlockY)
        {
            heightReached--;
            lastBlockY = currentBlockY;
        }

        // update game manager
        GameManager.Instance.AddHeighReached(heightReached);


    }
}

