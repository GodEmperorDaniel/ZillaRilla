using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//adds a character controller to any object the script is added to
[RequireComponent(typeof(CharacterController))]
public class MovementController_Player : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The player will generate this itself")]
    CharacterController character;

    [SerializeField]
    [Tooltip("It's read as a negative number")]
    float gravity = 1;

    [SerializeField]
    [Range(1,100)]
    float jumpSpeed = 1; 

    Vector3 movement;
    Vector2 moveXY;

    [SerializeField]
    [Range(1,100)]
    float characterSpeed = 1;

    public void Start()
    {
        if (character == null)
        { 
            character = gameObject.GetComponent<CharacterController>();
        }
    }

    private void Update()
    {
        character.Move(movement * Time.deltaTime * characterSpeed);
        Debug.DrawRay(transform.position, transform.forward, Color.red);

        if (moveXY != Vector2.zero)
        {
            transform.forward = new Vector3(movement.x, 0, movement.z).normalized;
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        moveXY = context.ReadValue<Vector2>();
        movement = new Vector3(moveXY.x, -gravity , moveXY.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<float>());
        //call ienumarator?
        float jumpyValue = context.ReadValue<float>();
        movement = new Vector3(moveXY.x, jumpyValue * jumpSpeed, moveXY.y);
    }


}
