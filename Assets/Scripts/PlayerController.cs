using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables del moviemiento del personaje
    public float jumpForce = 6f;
    public float runningSpeed = 2f;

    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer spriteRenderer;

    const string STATE_ALIVE = "isAlive";
    const string STATE_ON_THE_GROUND = "isOnTheGround";
    const string STATE_ON_AIR = "isOnAir";

    public LayerMask groundMask;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());

        Debug.DrawRay(this.transform.position, Vector2.down * 1.5f, Color.red);
    }

    
    void FixedUpdate()
    {

        Walk();

    }
    

    void Walk()
    {

       /* if (rigidBody.velocity.x < runningSpeed)
        {*/
            if (Input.GetKey(KeyCode.D))
            {
                rigidBody.velocity = new Vector2(runningSpeed, // x 
                                            rigidBody.velocity.y // y
                                                );
                spriteRenderer.flipX = false;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                
                rigidBody.velocity = new Vector2(-runningSpeed, // x 
                                               rigidBody.velocity.y // y
                                               );
                spriteRenderer.flipX = true;
            }
            
        /*}*/
    }

    // Hace saltar al personaje
    void Jump()
    {
        if (IsTouchingTheGround())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);            
        }
    }

    // Nos indica is el personaje está o no tocando el suelo
    bool IsTouchingTheGround()
    {
        if (Physics2D.Raycast(this.transform.position,
                                Vector2.down,
                                1.5f,
                                groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
