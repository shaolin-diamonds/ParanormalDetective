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
                // calculate the target position to which the player should move
                var targetPos = transform.position;
                // current position of the player plus the input
                targetPos.x += input.x;
                targetPos.y += input.y;

                // use the Coroutine function we created below
                StartCoroutine(Move(targetPos));
            }

        }
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
}
