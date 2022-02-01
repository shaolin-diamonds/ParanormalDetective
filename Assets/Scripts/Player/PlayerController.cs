using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // variable for movement speed
    public float moveSpeed;
    // variable to check if player is moving
    private bool isMoving;
    // getting input to move the player
    private Vector2 input;

    // reference to layer for the function to check if target position is walkable
    public LayerMask solidObjectsLayer;

    // reference to Animator controller
    private Animator animator;

    // we use the reference to the Animator in the Awake function 
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // if player not moving
        if (!isMoving)
        {
            // store input in the variables
            // GetAxisRaw input will always be 1 or -1
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            // remove diagonal movement
            if (input.x != 0) input.y = 0;

            // if input is not zero
            if (input != Vector2.zero)
            {
                // if the input is not zero, then we set the moveX and moveY parameters from the Animator
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                // since we are already setting x, y if input is not zero the player will stay in previous animation 

                // calculate the target position to which the player should move
                var targetPos = transform.position;
                // current position of the player plus the input
                targetPos.x += input.x;
                targetPos.y += input.y;

                // can only call Move() Coroutine IfWalkable is true
                if (IsWalkable(targetPos))
                {
                    // use the Move() Coroutine function we created
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        // set isMoving variable to Animator at end of update function
        animator.SetBool("isMoving", isMoving);
    }

    // write Coroutine function to move player from current position to target position
    // IEnumerator used to do something over period of time
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        // when first called it will check if the difference between target position and player's current position is 
        // greater than a very small value
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            // if there is actually a difference between both positions then we will use the move towards function
            // to move the player towards the target position by a very small amount
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            // this will stop the execution of the Coroutine and resume it in the next update function
            yield return null;
        }
        // set current position to target position
        transform.position = targetPos;

        isMoving = false;
    }

    // before we call the Coroutine to move the player, we have to check if targetPos is a walkable tile
    // this function will check if the target position is walkable
    private bool IsWalkable(Vector3 targetPos)
    {
        // use Physics2D.OverlapCirlce() to check if there is solid object at the target position
        // first parameter is position need to check
        // second parameter is the radius of circle need to check (tile is 1 unit so using 0.2f, also to give some perspective to player)
        // third parameter we can pass the layer of the object we want to check
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null)
        {
            // if not null, it means there is a solid object in the target position
            // which means the tile is not walkable and we return false
            return false;
        }

        // otherwise we return true
        return true;
    }
}
