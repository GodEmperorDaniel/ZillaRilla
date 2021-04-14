//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

////adds a character controller to any object the script is added to
//[RequireComponent(typeof(CharacterController))]
//public class MovementController_Player : MonoBehaviour
//{
//	[Header("General")]
//	[SerializeField]
//	[Tooltip("The player will generate this itself")]
//	CharacterController character;

//	[Header("Jumping/Falling")]
//	[SerializeField]
//	[Tooltip("It's read as a negative number")]
//	float gravity = 1;

//	[SerializeField]
//	[Range(1, 100)]
//	float jumpHeight = 1;

//	[SerializeField]
//	[Tooltip("Does so tha player can input movement in the air")]
//	private bool moveInAir;

//	private bool characterGrounded;

//	[Header("Movement")]
//	[SerializeField]
//	[Range(1, 100)]
//	float characterSpeed = 1;
//	Vector3 movement;
//	Vector2 moveXY;

//	[Header("Dash")]
//	[SerializeField]
//	bool useDash = true;
//	[SerializeField]
//	[Tooltip("Also effekts maximum jump-height")]
//	float dashSpeed = 2;
//	[SerializeField]
//	float dashTime = 2;

//	[Header("Dash acceleration")]
//	[SerializeField]
//	[Tooltip("Do you want the acceleration to be successive")]
//	bool useLinAcc = false;
//	[SerializeField]
//	[Tooltip("After x secunds the player is at max dash-speed")]
//	float dashAccelTime = 1;
//	[SerializeField]
//	float dashCooldownTime = 2;
//	bool dashCoolDown = false;
//	[SerializeField]
//	float dashMult = 1;

//	//c_ is for corutine in this case!
//	Coroutine c_dashing;
//	Coroutine c_dashCooldown;

//	private void OnGUI()
//	{
//		if (dashTime - dashAccelTime < 0)
//			Debug.LogError("Dash Time can not be lower than the acceleration time!");
//	}
//	public void Start()
//	{
//		if (character == null)
//		{
//			character = gameObject.GetComponent<CharacterController>();
//		}
//	}

//	private void Update()
//	{
//		characterGrounded = character.isGrounded;
//		movement.y -= gravity * Time.deltaTime;
//		if (!moveInAir)
//		{
//			if (characterGrounded)
//			{
//				movement.x = moveXY.x;
//				movement.z = moveXY.y;
//			}
//		}
//		else
//		{
//			movement.x = moveXY.x;
//			movement.z = moveXY.y;
//		}
//		character.Move(movement * Time.deltaTime * characterSpeed * dashMult);
//		Debug.DrawRay(transform.position, transform.forward, Color.red);

//		if (movement.x != 0 || movement.z != 0)
//		{
//			transform.forward = new Vector3(movement.x, 0, movement.z).normalized;
//		}
//	}
//	public void Move(InputAction.CallbackContext context)
//	{
//		//Debug.Log(context.ReadValue<Vector2>());
//		moveXY = context.ReadValue<Vector2>();
//	}

//	//public void Jump(InputAction.CallbackContext context)
//	//{
//	//	float jumpyValue = context.ReadValue<float>();

//	//	if (characterGrounded && jumpyValue > 0)
//	//	{
//	//		//Debug.Log(context.ReadValue<float>());
//	//		//Debug.Log("im starting jump!");
//	//		movement = new Vector3(moveXY.x, jumpHeight, moveXY.y);
//	//	}
//	//}

//	public void Sprinting(InputAction.CallbackContext context)
//	{
//		if (useDash)
//		{
//			float sprint = context.ReadValue<float>();
//			if (sprint != 0 && !dashCoolDown)
//			{
//				Debug.Log("Im sprinting");
//				c_dashing = StartCoroutine(SprintingTime());
//			}
//			else
//			{
//				StopCoroutine(c_dashing);
//				c_dashCooldown = StartCoroutine(SprintCooldown()); //might be a dumb implementation method.. might cause problems
//				dashMult = 1;
//			}
//		}
//	}

//	IEnumerator SprintingTime()
//	{
//		if (useLinAcc)
//		{
//			float t_Elapsed = 0;
//			while (t_Elapsed < dashAccelTime)
//			{
//				t_Elapsed += Time.deltaTime;
//				yield return new WaitForSeconds(Time.deltaTime);
//				dashMult = Mathf.Lerp(1, dashSpeed, t_Elapsed / dashAccelTime);
//			}

//			yield return new WaitForSeconds(dashTime - dashAccelTime);
//		}
//		else
//		{
//			dashMult = dashSpeed;
//			yield return new WaitForSeconds(dashTime);
//		}
//		dashMult = 1;

//		c_dashCooldown = StartCoroutine(SprintCooldown());
//		yield return null;
//	}
//	IEnumerator SprintCooldown()
//	{
//		if (dashCoolDown)
//		{
//			yield return new WaitForSeconds(dashCooldownTime);
//			dashCoolDown = false;
//		}
//		yield return null;
//	}
//}
