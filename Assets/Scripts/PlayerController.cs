using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject aiController;
    private OpenAIControllerScript aiControllerscript;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private float verticalSpeed;


    private Rigidbody2D rb;
    private bool isGrounded = false;
    private SpriteRenderer spriteRenderer;
    public Animator animator;

    private DualWorld dualWorldScript;
    private float timeElapsed;
    private Vector2 previousPosition;

    private string name;
    private string prompt;
    private string starter;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        dualWorldScript = FindObjectOfType<DualWorld>();
        aiControllerscript = aiController.GetComponent<OpenAIControllerScript>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            dualWorldScript.SetScene(other.gameObject.name);
        }
        if (other.gameObject.CompareTag("Death"))
        {
            //destroy the gameobject to which this script is attached
            Destroy(gameObject);
            Debug.Log("Player Destroyed");
        }
    }
        void Update()
        {
            // Move the player horizontally
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
            rb.velocity = movement;
            verticalSpeed = rb.velocity.y;

            animator.SetFloat("VerticalSpeed", verticalSpeed);
            animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));
            animator.SetBool("isGrounded", isGrounded);
            

            // Flip the sprite horizontally if moving left
            if (moveHorizontal < 0)
            {
                spriteRenderer.flipX = true;
            }
            // Flip the sprite horizontally if moving right
            else if (moveHorizontal > 0)
            {
                spriteRenderer.flipX = false;
            }
            // Check if the player is on the ground
            int platformMask = ~(1 << 3); // set the layer mask to ignore layer 3
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, platformMask);
            
            
            // Jump if the player presses the w key and is on the ground
            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {   
                Vector2 jumpVelocity = Vector2.up * jumpForce;
                rb.velocity = jumpVelocity;
            }               
            
        }

    

    private void OnCollisionEnter2D(Collision2D collision2d)
    {
        if (collision2d.gameObject.tag == "Character")
        {
            if (collision2d.gameObject.name == "Ralph")
            {
                name = "Ralph";
                prompt = "You are Ralph talking to a person";
                starter = "Prompt";
            }
                aiControllerscript.StartConversation(prompt, starter, name);
        }
    }
    
}