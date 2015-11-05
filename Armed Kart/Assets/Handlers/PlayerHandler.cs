using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

//TODO: This is currently ported for PC. PORT TO ANDROID LATER

/// <summary>
/// Handles the player's movement, rotation, etc.
/// </summary>
public class PlayerHandler : MonoBehaviour 
{
	private CharacterController Player { get; set; }
	
	/// <summary>
	/// Gets or sets a value indicating whether the car is rotating.
	/// </summary>
	/// <value><c>true</c> if the car is rotating; otherwise, <c>false</c>.</value>
	private bool IsRotating { get; set; }
	/// <summary>
	/// Gets or sets a value indicating whether the car is colliding.
	/// TODO: Make this function properly
	/// </summary>
	/// <value><c>true</c> if the car is colliding; otherwise, <c>false</c>.</value>
	private bool IsColliding { get; set; }

	/// <summary>
	/// List of checkpoints.
	/// </summary>
	public List<GameObject> CheckPoints = new List<GameObject>();

	/*
	 * The car's wheels
	 */
	public GameObject frontRight;
	public GameObject frontLeft;
	public GameObject backRight;
	public GameObject backLeft;

	/// <summary>
	/// The type of the car.
	/// </summary>
	public CarTypes CarType;

	/// <summary>
	/// The car's current velocity, limit is car type's maximum speed
	/// </summary>
	public float CurrentVelocity = 0;

	private float LastVelocity = 0;

	private Stopwatch timer = new Stopwatch ();

	/// <summary>
	/// To-be-set in the Start() method. LOOK THERE.
	/// </summary>
	public CarHandler PlayerCar;

	/*
	 * Game constants
	 */
	const float BRAKE_FACTOR = 1.02f; //TODO: OBSOLETE, REMOVE LATER?
	const float COLLISION_SPEED_FACTOR = 8f; //TODO: Fix this
	const float REALISM_FACTOR = 3f; // used to divide the velocity of the car on-screen

	const int SPEED_DROP_FACTOR = 2;
	const int BRAKE_SPEED_DROP_FACTOR = 5;

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
	/// Happens after Update()
	/// </summary>
	private void LateUpdate()
	{

	}

	/// <summary>
	/// Handles the player rotation.
	/// </summary>
	private void HandlePlayerRotation()
	{
		var curMaxSpeed = PlayerCar.GetMaxSpeed ();

		//TODO: Make this rotation more realistic
		// Simple rotation
		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) 
		{
			IsRotating = true;

			var rot = Input.GetAxis("Horizontal");

			// 1 is full, -1 is full

			rot *= curMaxSpeed * 2;
			rot -= ((CurrentVelocity * 2) / 2);
			rot /= curMaxSpeed * 2;

			if (this.CurrentVelocity > 1 || this.CurrentVelocity < -1)
			{
				//if (this.Player.isGrounded)
					transform.Rotate(new Vector3(0, rot, 0));
				//else
					//transform.Rotate(new Vector3(0, rot, rot));
			}
		}
		else
			IsRotating = false;
	}

	/// <summary>
	/// Handles the player movement.
	/// </summary>
	private void HandlePlayerMovement()
	{
		// The max speed. The player's speed should not get above this.
		var curMaxSpeed = PlayerCar.GetMaxSpeed ();
		var curMaxSpeedNoThrust = curMaxSpeed / 2;
		var speedFactors = PlayerCar.GetSpeedFactors ();
		var curMaxReverseSpeed = PlayerCar.GetCarReverseSpeed ();
		var acceleration = PlayerCar.GetCarAcceleration ();
		var turnSpeedFactor = PlayerCar.GetCarTurningSpeedFactor ();
		var carWeight = PlayerCar.GetCarWeight () + 1;

		var speedFactor = 0f;

		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.LeftShift)) // if shift + w is pressed
		{	

			if (!timer.IsRunning && this.CurrentVelocity == 0)
			{
				timer = new Stopwatch();
				timer.Start ();
			}

			if (this.CurrentVelocity > curMaxSpeed) // if current velocity is higher than current max speed
			{
				speedFactor = -SPEED_DROP_FACTOR; // then we slow down the car, stops bugs
			}
			else if (this.CurrentVelocity < curMaxSpeed / 2) // if current velocity is less than current max speed halved.
			{
				speedFactor = (speedFactors[1] * 2 / (acceleration / carWeight));
			}
			else if (this.CurrentVelocity < curMaxSpeed) // if current velocity is less than current max speed
			{
				speedFactor = speedFactors[0] * 2;
			}

			if (this.CurrentVelocity >= 100 && timer.IsRunning)
			{
				timer.Stop ();
				UnityEngine.Debug.Log ("We accelerated to 100km/h in " + timer.ElapsedMilliseconds + " ms");
			}
		}
		else if (Input.GetKey (KeyCode.W)) // if not
		{
			if (this.CurrentVelocity > curMaxSpeedNoThrust)
			{
				speedFactor = -SPEED_DROP_FACTOR;
			}
			else if (this.CurrentVelocity < curMaxSpeedNoThrust / 2)
			{
				speedFactor = speedFactors[1] / (acceleration / carWeight);
			}
			else if (this.CurrentVelocity < curMaxSpeedNoThrust)
			{
				speedFactor = speedFactors[0];
			}
		}
		else
		{
			// This steps in after no key is pressed, so, this stops the car
			if (this.CurrentVelocity > SPEED_DROP_FACTOR)
			{
				speedFactor = -SPEED_DROP_FACTOR;
			}
			else
			{
				if (!Input.GetKey (KeyCode.S))
					NullFloatValues (ref speedFactor, ref this.CurrentVelocity);
			}
		}

		// Handles the braking
		if (Input.GetKey (KeyCode.S)) 
		{
			speedFactor -=  BRAKE_SPEED_DROP_FACTOR / this.CurrentVelocity;

			if (this.CurrentVelocity < 0)
				speedFactor += this.CurrentVelocity;

		}

		if (this.CurrentVelocity < -curMaxReverseSpeed)
			speedFactor = 10;

		if (IsRotating)
			speedFactor /= turnSpeedFactor;

		/*
		 * When collision is detected correctly, use this to slow down the car
		 * if (IsColliding)
			speedFactor /= COLLISION_SPEED_FACTOR;
		*/

		this.CurrentVelocity += speedFactor;
		this.LastVelocity = CurrentVelocity;

		//TODO: make the car act like in real life

		this.CurrentVelocity /= (((2f * curMaxSpeed) - curMaxSpeed) / curMaxSpeed);

		var speed = new Vector3 (0, 0, (this.CurrentVelocity / REALISM_FACTOR) * -1);
		// We cut the velocity there ^ not to make the car too fast
		speed = transform.rotation * speed;
		speed = speed * Time.deltaTime;

		this.Player.Move (speed);
	}

	/// <summary>
	/// Nulls the float values. E.G. sets all the values given into this function to zero
	/// </summary>
	/// <param name="first">First.</param>
	/// <param name="second">Second.</param>
	private void NullFloatValues(ref float first, ref float second)
	{
		first = 0;
		second = 0;
	}

	/// <summary>
	/// Raises the controller collider hit event.
	/// </summary>
	/// <param name="hit">Hit.</param>
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		/* */
	}
}
