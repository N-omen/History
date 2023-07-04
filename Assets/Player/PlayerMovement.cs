using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    public float walkingSpeed = 3;
    public float runningSpeed = 6;
    public float gravityAccelaration = 10;

    [Header("Components")]
    public CharacterController characterController;
    public Transform orientation;

    private float verticalSpeed;

    private Vector3 Movement(){
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveVectro = orientation.forward * vertical + orientation.right * horizontal;
        Vector3.Normalize(moveVectro);

        if(Input.GetKey(KeyCode.LeftShift)) moveVectro *= runningSpeed;
        else moveVectro *= walkingSpeed;

        return moveVectro;
    }

    private Vector3 HorizontalMovement(){
        if(!characterController.isGrounded) verticalSpeed -= gravityAccelaration * Time.deltaTime;
        else if(Input.GetKey(KeyCode.Space)) verticalSpeed = 4;
        return orientation.up * verticalSpeed;
    }

    private void Update(){
        Vector3 movementVector = Movement();
        movementVector += HorizontalMovement();
        characterController.Move(movementVector * Time.deltaTime);
    }
}
