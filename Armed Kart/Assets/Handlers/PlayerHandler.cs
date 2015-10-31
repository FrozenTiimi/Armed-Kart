using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Handles the player's movement, rotation, etc.
/// </summary>
public class PlayerHandler : MonoBehaviour 
{
	//TODO: This is currently ported for PC. PORT TO ANDROID LATER

	/// <summary>
	/// Gets or sets the charactercontroller (player)
	/// </summary>
	/// <value>The player.</value>
	private CharacterController Player { get; set; }

	/// <summary>
	/// The type of the car.
	/// </summary>
	public CarTypes CarType;
	/// <summary>
	/// To-be-set in the Start() method. LOOK THERE.
	/// </summary>
	private CarHandler PlayerCar;

	/// <summary>
	/// The car's current velocity, limit is car type's maximum speed
	/// </summary>
	private int CurrentVelocity = 0;

	/// <summary>
	/// The GRAVITY constant, like in real life (9.81 m/s)
	/// </summary>
	const float GRAVITY = 9.81f;
	
	/// <summary>
	/// Used to initialize the player
	/// </summary>
	private void Start () 
	{
		//TODO: Add all the car types. 
		// Initializes the car based on the type.
		switch (CarType) 
		{
			case 0:
			default:
				PlayerCar = new TestCar();
			break;
		}
	}
	
	/// <summary>
	/// Handles the player updates
	/// </summary>
	private void Update () 
	{
		this.Player = GetComponent<CharacterController> ();

		this.HandlePlayerRotation ();
		this.HandlePlayerMovement ();
	}

	/// <summary>
	/// Handles the player rotation.
	/// </summary>
	private void HandlePlayerRotation()
	{
		//TODO: Make this rotation more realistic
		// Simple rotation
		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) 
		{
			var rot = Input.GetAxis("Horizontal");

			transform.Rotate(new Vector3(0, rot, 0));
		}
	}

	/// <summary>
	/// Handles the player movement.
	/// </summary>
	private void HandlePlayerMovement()
	{
		// The max speed. The player's speed should not get above this.
		var curMaxSpeed = PlayerCar.GetMaxSpeed ();

		//TODO: My fiddly doos. You can remove these if you see fit.
		if (this.CurrentVelocity < curMaxSpeed / 2)
			this.CurrentVelocity += 10;
		else if (this.CurrentVelocity < curMaxSpeed)
			this.CurrentVelocity += 5;

		//TODO: Make the character faster, make it "realistic", e.g. the car starts off slow, but gets faster and faster, 
		//		and nearing the end, gets faster by smaller amounts.
		//		like this: ___--¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯--_
		//		where _ = 0 - 10, - = 10 - 40, ¯ = 40 - 100, - = 100 - 120, _ = 120 - 130
		//		you should know this. BASICALLY, MAKE IT LIKE IN REAL LIFE

		this.CurrentVelocity /= this.CurrentVelocity - (this.CurrentVelocity / 2); // to make it not so fast. fix these magic numbers please

		var speed = new Vector3 (0, (this.Player.isGrounded) ? 0 : -GRAVITY /* handles the gravity */, this.CurrentVelocity * -1);
		speed = transform.rotation * speed;

		// This works because I removed the rigidbody component from the character. 
		// We do not need that, terrain handles collision fine already.
		this.Player.Move (speed * Time.deltaTime);
	}
}
