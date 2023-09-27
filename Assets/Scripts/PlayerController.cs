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
                prompt = "You are Ralph talking to the main character. Your goal is to help him understand in a subtle way that the world switches between two modes that help him do the parkour necessary to complete the levels. The player has to press space to engage this mode. Keep your responses relatively short while conveying the necessary information.";
                starter = "Ralph is an oddly shaped man, who seems to be wearing a suit of a material not quite cloth and not quite metal, but somehow both. He curiously looks you up and down and says 'Hey you don't seem to know where you're going'";
            }
            else if(collision2d.gameObject.name == "Sally")
            {
                prompt = "You are Sally talking to the main character. You are trying to tell them more about the world they are in: a never ending dreary parkour in which space follows the player and is fully infinite. Keep your responses a bit short and make them depressing.";
                starter = "Sally looks up at you from her short stature. Then looks back down. She looks defeated but nothing here could have defeated her, strange.";
            }
            animator.SetFloat("Speed", 0);
            aiControllerscript.StartConversation(prompt, starter, collision2d.gameObject.name);
            PlayerController playerController = GetComponent<PlayerController>();
        }
    }
    
}