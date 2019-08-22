using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class slimeControl : MonoBehaviour
{
    [Header("Casting")]
    public Transform castorigin;
    public Transform castDestination;
    public LayerMask castMask;

    [Header("Movement")]
    public float moveSpeed = 4;
    bool facingRight = false;
    Vector2 direction;

    Rigidbody2D _rigidbody2D;
    Animator animator;

    Coroutine moveRoutine;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        reset();
    }

    public void reset()
    {
        moveRoutine = StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        DetermineDirection();
        while (true)
        {
            if (GroundCheck())
            {
                flip();
                DetermineDirection();
            }
            else
            {
                Vector2 newPostiion = (Vector2)transform.position + (direction * moveSpeed * Time.deltaTime);
                _rigidbody2D.MovePosition(newPostiion);
            }
            yield return null;
        }
    }

    bool GroundCheck()
    {
        bool endReach = false;
        RaycastHit2D[] hitArray = new RaycastHit2D[1];
        if (Physics2D.LinecastNonAlloc((Vector2)castorigin.position, (Vector2)castDestination.position, hitArray, castMask) > 0)
        {
            Vector2 hitvector = hitArray[0].transform.TransformDirection(hitArray[0].normal);
            if(hitvector.y > 0.9f)
            {
                endReach = false;
            }
            else
            {
                endReach = true;
            }
        }
        else
        {
            endReach = true;
        }
        return endReach;
    }

    void DetermineDirection()
    {
        if (facingRight)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }
    }

    void flip (){
        facingRight = !facingRight;
        transform.Rotate(transform.up, 180f);
    }
}
