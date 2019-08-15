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

    // Start is called before the first frame update
    void Start()
    {
        _rigitbody = GetComponent<Rigidbody2D>();
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
        }
        if(HorizontalInput == 0 && LastHorizontalInput != 0)
        {
            moving = false;
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
            if (Coll.normal.y > 0.1f)
            {
                Ground(collision.collider);
            }
        }
    }

    void Ground(Collider2D newGround)
    {
        inAir = false;
        currentGravity = 0f;
        CurrentGround = newGround;

    }
}
