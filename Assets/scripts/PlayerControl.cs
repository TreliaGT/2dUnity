using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Input")]
    float HorizontalInput; //Movement
    float LastHorizontalInput; // last frame movement
    public float jumpHoldTime = 0.3f;

    [Header("Basic Movement")]
    bool moving = false;
    public float movespeed = 5;
    Rigidbody2D _rigitbody;
    float currentMove = 0f;
    public bool facingRight = true;

    Vector2 finalMovement;
    bool runMovement = false;
    Animator Animator; 

    [Header("Vertical Movement")]
    public float upforce = 5;
    public bool inAir = true;
    bool jumpHeld = false;

    [Header("Gravity")]
    public float upGravity = -16f;
    public float downGravity = -20f;
    public float jumpHoldGravity = -5f;
    public float MaxFallSpeed = -10f;
    float currentGravity = 0f;

    [Header("Collision Handling")]
    Collider2D CurrentGround;
    Collider2D LeftWall;
    Collider2D RightWall;
    public bool WallRight = false;
    public bool WallLeft = false;



    // Start is called before the first frame update
    void Start()
    {
        _rigitbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        if(HorizontalInput != 0 && LastHorizontalInput == 0)
        {
            moving = true;
            Animator.SetBool("Moving", true);
        }
        if(HorizontalInput == 0 && LastHorizontalInput != 0)
        {
            moving = false;
            Animator.SetBool("Moving", false);
        }
        if(HorizontalInput > 0f && !facingRight || HorizontalInput < 0f && facingRight)
        {
            Flip();
        }
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        LastHorizontalInput = HorizontalInput;

    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(transform.up, 180f);

    }

    void Jump()
    {
        if (inAir)
        {
            Animator.SetTrigger("Jump");
            Animator.SetBool("inAir", true);
            return;
        }
        inAir = true;
        currentGravity = upforce;
        StartCoroutine(JumpHoldRoutine());
    }

    IEnumerator JumpHoldRoutine()
    {
        jumpHeld = true;
        float Timer = 0f;
        while(Timer < jumpHoldTime && Input.GetButton("Jump"))
        {
            Timer += Time.deltaTime;
            yield return null;
        }
        jumpHeld = false;
    }

    private void FixedUpdate()
    {
        if (inAir)
        {
            SetMovement();
            if (jumpHeld)
            {
                currentGravity += jumpHoldGravity * Time.deltaTime;
            }
            else
            {
                if(currentGravity > 0f)
                {
                    currentGravity += upGravity * Time.deltaTime;
                }else if (currentGravity <= 0f)
                {
                    currentGravity += downGravity * Time.deltaTime;
                }
            }
            currentGravity = Mathf.Clamp(currentGravity , MaxFallSpeed , upforce);
            finalMovement.y = currentGravity;
        }
        if (moving)
        {
            SetMovement();
            currentMove = HorizontalInput * movespeed;
            if(currentMove > 0f && RightWall || currentMove < 0f && WallLeft || WallLeft && WallRight)
            {
                currentMove = 0;
            }
            finalMovement.x = currentMove;
        }
        if (runMovement)
        {
            _rigitbody.MovePosition((Vector2)transform.position + finalMovement * Time.deltaTime);
            runMovement = false;
        }
    }

    void SetMovement()
    {
        if (!runMovement)
        {
            runMovement = true;
            finalMovement = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       ColliderDistance2D Coll =  collision.collider.Distance(gameObject.GetComponent<Collider2D>());
       // Debug.DrawRay(Coll.pointA, Coll.normal, Color.black , 1f);
        if (collision.collider.tag == "Enviro")
        {
            if (Coll.normal.y > 0.1f)//Ground
            {
                Ground(collision.collider);

            }

            if (Coll.normal.y < -0.1f )//roof
            {
                currentGravity = 0;
                jumpHeld = false;
            }
            if (Coll.normal.x < -0.9f)// works with straight walls right wall
            {
                WallRight = true;
                RightWall = collision.collider;
            }
            if (Coll.normal.x > 0.9f)//left wall
            {
                WallLeft = true;
                LeftWall = collision.collider;
            }

        }
    }

    void Ground(Collider2D newGround)
    {
        inAir = false;
        currentGravity = 0f;
        CurrentGround = newGround;
        Animator.SetBool("inAir", false);
    }

    private void OnCollisionExit2D(Collision2D collision)//contact is lost with collider
    {
        if(collision.collider.tag == "Enviro")
        {
            if(collision.collider == CurrentGround)
            {
                if (!inAir)
                {
                    inAir = true;
                    CurrentGround = null;
                    Animator.SetTrigger("Jump");
                    Animator.SetBool("inAir", true);
                }
            }
            if(collision.collider == RightWall)
            {
                RightWall = null;
                WallRight = false;
            }

            if(collision.collider == LeftWall)
            {
                LeftWall = null;
                WallLeft = false;
            }
        }
    }
}
