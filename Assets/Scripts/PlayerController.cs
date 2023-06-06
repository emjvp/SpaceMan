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
    Vector3 startPosition;

    const string STATE_ALIVE = "isAlive";
    const string STATE_ON_THE_GROUND = "isOnTheGround";
    const string STATE_ON_AIR = "isOnAir";

    public LayerMask groundMask;

    public static PlayerController sharedInstance;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        startPosition = this.transform.position;
    }

    public void StartGame()
    {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, true);

        this.transform.position = startPosition;
        this.rigidBody.velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());

        Debug.DrawRay(this.transform.position, Vector2.down * 1.5f, Color.red);
    }


    void FixedUpdate()
    {

        // Walk();
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (rigidBody.velocity.x < runningSpeed)
            {
                rigidBody.velocity = new Vector2(runningSpeed, //x
                                                 rigidBody.velocity.y //y
                                                );
            }
        }
        else
        {// Si no estamos dentro de la partida
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }

    }

    void Walk()
    {

       if (rigidBody.velocity.x < runningSpeed)
       {
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
            
        }
    }

    // Hace saltar al personaje
    void Jump()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame && IsTouchingTheGround())
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

    public void Die()
    {
        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }
}
