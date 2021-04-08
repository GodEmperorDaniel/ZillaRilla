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
	[Range(1, 100)]
	float jumpHeight = 1;

	[SerializeField]
	[Tooltip("Does so tha player can input movement in the air")]
	private bool moveInAir;

	private bool characterGrounded;

	Vector3 movement;
	Vector2 moveXY;

	[SerializeField]
	[Range(1, 100)]
	float characterSpeed = 1;

	[SerializeField]
	bool useDash = true;
	[SerializeField]
	[Tooltip("Also effekts maximum jump-height")]
	float dashSpeed = 2;
	float dashMult = 1;

	public void Start()
	{
		if (character == null)
		{
			character = gameObject.GetComponent<CharacterController>();
		}
	}

	private void Update()
	{
		characterGrounded = character.isGrounded;
		movement.y -= gravity * Time.deltaTime;
		if (!moveInAir)
		{
			if (characterGrounded)
			{
				movement.x = moveXY.x;
				movement.z = moveXY.y;
			}
		}
		else
		{
			movement.x = moveXY.x;
			movement.z = moveXY.y;
		}
		character.Move(movement * Time.deltaTime * characterSpeed * dashMult);
		Debug.DrawRay(transform.position, transform.forward, Color.red);

		if (movement.x != 0 || movement.z != 0)
		{
			transform.forward = new Vector3(movement.x, 0, movement.z).normalized;
		}
	}
	public void Move(InputAction.CallbackContext context)
	{
		//Debug.Log(context.ReadValue<Vector2>());
		moveXY = context.ReadValue<Vector2>();
	}

	public void Jump(InputAction.CallbackContext context)
	{
		float jumpyValue = context.ReadValue<float>();

		if (characterGrounded && jumpyValue > 0)
		{
			//Debug.Log(context.ReadValue<float>());
			//Debug.Log("im starting jump!");
			movement = new Vector3(moveXY.x, jumpHeight, moveXY.y);
		}
	}

	public void Sprinting(InputAction.CallbackContext context)
	{
		if (useDash)
		{
			float sprint = context.ReadValue<float>();
			if (sprint != 0)
			{
				dashMult = dashSpeed;
			}
			else
			{
				dashMult = 1;
			}
		}

	}
}
