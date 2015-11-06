using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

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
	public GameObject FrontRight { get { return transform.FindChild ("wheel_frontright").gameObject; } }
	public GameObject FrontLeft { get { return transform.FindChild ("wheel_frontleft").gameObject; } }
	public GameObject BackRight { get { return transform.FindChild ("wheel_backright").gameObject; } }
	public GameObject BackLeft { get { return transform.FindChild ("wheel_backleft").gameObject; } }


	/// <summary>
	/// The type of the car.
	/// </summary>
	public CarTypes CarType;

	/// <summary>
	/// To-be-set in the Start() method. LOOK THERE.
	/// </summary>
	public CarHandler PlayerCar;

	/// <summary>
	/// The car's current velocity, limit is car type's maximum speed
	/// </summary>
	public float CurrentVelocity = 0f;
	public float CurrentRotation = 0f;

	public Vector3 CurrentSpeed;

	private float LastVelocity = 0f; // TODO: Remove this?
	private float CollideAngle = 0f;

	private Stopwatch timer = new Stopwatch ();

	/*
	 * Game constants
	 */
	const float COLLISION_SPEED_FACTOR = 2f;
	const float REALISM_FACTOR = 3f; // used to divide the velocity of the car on-screen
	const int SPEED_DROP_FACTOR = 2;
	const int BRAKE_SPEED_DROP_FACTOR = 5;
	const int FLAT_GROUND = 90; // flat ground collision angle
	const int FULL_CIRCLE = 360;

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
		/*
		var pos = new Vector3(transform.position.x - (transform.lossyScale.x), transform.position.y, transform.position.z - (transform.lossyScale.z)); 
		var pos2 = new Vector3(transform.position.x - (transform.lossyScale.x), transform.position.y, transform.position.z + (transform.lossyScale.z)); 
		var pos3 = new Vector3(transform.position.x + (transform.lossyScale.x), transform.position.y, transform.position.z - (transform.lossyScale.z)); 
		var pos4 = new Vector3(transform.position.x + (transform.lossyScale.x), transform.position.y, transform.position.z + (transform.lossyScale.z)); 

		var nRot = DegToRad(this.CurrentRotation * 90);
		UnityEngine.Debug.Log (this.CurrentRotation * 90);

		pos = pos - new Vector3(Mathf.Cos (nRot), Mathf.Sin (nRot));
		pos2 = pos2 - new Vector3(Mathf.Cos (nRot), Mathf.Sin (nRot));
		pos3 = pos3 - new Vector3(Mathf.Cos (nRot), Mathf.Sin (nRot));
		pos4 = pos4 - new Vector3(Mathf.Cos (nRot), Mathf.Sin (nRot));

		UnityEngine.Debug.DrawRay (pos, transform.TransformDirection(Vector3.down), Color.red);
		UnityEngine.Debug.DrawRay (pos2, transform.TransformDirection(Vector3.down), Color.blue);
		UnityEngine.Debug.DrawRay (pos3, transform.TransformDirection(Vector3.down), Color.black);
		UnityEngine.Debug.DrawRay (pos4, transform.TransformDirection(Vector3.down), Color.green);
		*/
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
			var rot = Input.GetAxis("Horizontal");

			// 1 is full, -1 is full

			rot *= curMaxSpeed * 2;
			rot -= (CurrentVelocity / 2f);
			rot /= curMaxSpeed * 2;

			this.CurrentRotation = rot;

			if (this.CurrentVelocity > 1 || this.CurrentVelocity < -1)
			{
				IsRotating = true;

				//if (this.Player.isGrounded)
					transform.Rotate(new Vector3(0, rot, 0));
				//else
					//transform.Rotate(new Vector3(0, rot, rot));
			}
			else
			{
				IsRotating = false;
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
		var speedFactors = PlayerCar.GetSpeedFactors ();
		var curMaxReverseSpeed = PlayerCar.GetCarReverseSpeed ();
		var acceleration = PlayerCar.GetCarAcceleration ();
		var turnSpeedFactor = PlayerCar.GetCarTurningSpeedFactor ();
		var carWeight = PlayerCar.GetCarWeight ();
		carWeight++; // we make the weight always have an impact on gravity

		var curMaxSpeedNoThrust = HalfOf (curMaxSpeed);

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
			else if (this.CurrentVelocity < HalfOf (curMaxSpeed)) // if current velocity is less than current max speed halved.
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
			else if (this.CurrentVelocity < HalfOf (curMaxSpeedNoThrust))
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
			speedFactor -= this.CalculateBrakingSpeed();

			if (this.CurrentVelocity < 0) // reverse speed modifier
				speedFactor += this.CurrentVelocity;

		}

		if (this.CurrentVelocity < -curMaxReverseSpeed)
			speedFactor = 10; // we do not like magic numbers. replace this with something else.

		if (IsRotating)
			speedFactor /= turnSpeedFactor;

		/*
		 * COLLISION IS BROKEN AS OF 5.11.2015
		 * PLEASE FIX :-(
		if (IsColliding) 
		{
			var collideFactor = (COLLISION_SPEED_FACTOR * (this.CollideAngle / FULL_CIRCLE));

			if (this.CurrentVelocity - collideFactor >= 0)
				speedFactor = -collideFactor;
		}
		*/

		this.CurrentVelocity += speedFactor;
		this.LastVelocity = CurrentVelocity;

		this.CurrentVelocity /= this.CalculateCurrentVelocityAccelerationModifier(curMaxSpeed);

		var zModifier = Flip (this.SetVelocityRealistic (this.CurrentVelocity)); // we flip the current velocity and make it realistic
		var speed = new Vector3 (0, 0, zModifier);
		speed = transform.rotation * speed; // we times the speed by the rotation quaternion to make the car actually move in the right direction

		this.CurrentSpeed = speed;

		speed = speed * Time.deltaTime; // we make the speed not dependent on FPS

		// Get all the car's wheels
		var wheels = this.GetAllWheels ();

		// Loop through them
		wheels.ForEach (w =>
		{
			//TODO: Fix this!!!!!!! THE SECOND PARAMETER IS THE, UHM... 
			//		YOU KNOW... WHEN YOU TURN THE CAR... ROTATION... THINGY... I HOPE YOU KNOW
			w.transform.Rotate (new Vector3(0, 0, this.CurrentVelocity));
		});

		// Finally, move the player
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
		/*
		 * TODO: Fix this
		new Thread (() => 
        {
			var angle = Vector3.Angle (hit.point, hit.normal);
			
			if (angle == FLAT_GROUND)
			{
				this.IsColliding = false;
			}
			else
			{
				this.CollideAngle = this.ParseAngle(angle);
				this.IsColliding = true;
			}
		}).Start ();
		*/
	}

	/// <summary>
	/// Determines whether this car is colliding.
	/// </summary>
	/// <returns><c>true</c> if car's colliding; otherwise, <c>false</c>.</returns>
	public bool IsCarColliding()
	{
		return this.IsColliding;
	}

	/// <summary>
	/// Gets the colliding angle.
	/// </summary>
	/// <returns>The colliding angle.</returns>
	public float GetCollidingAngle()
	{
		return this.CollideAngle;
	}

	/// <summary>
	/// Determines whether the specified float is zer
	/// </summary>
	/// <returns><c>true</c> if the given float is zero; otherwise, <c>false</c>.</returns>
	/// <param name="obj">to-be-checked value.</param>
	private bool IsZero(float obj)
	{
		return obj == 0;
	}

	/// <summary>
	/// This function has no purpose at the moment
	/// ----This function parses the given angle from form -180 -> 180 to 0 -> 360 degrees, and then decreases by 200----
	/// </summary>
	/// <returns>The parsed angle.</returns>
	/// <param name="angle">Angle.</param>
	private float ParseAngle(float angle)
	{
		var nAngle = 0f;

		if (angle > 90)
			nAngle = angle - 90;
		else if (angle < -90)
			nAngle = angle + 90;
		else
			nAngle = angle;

		if (nAngle < 0)
			nAngle *= -1;

		return nAngle;
	}

	/// <summary>
	/// Calculates the braking speed.
	/// DO NOT TOUCH THESE KIND OF FUNCTIONS UNLESS YOU REALLY KNOW WHAT YOU'RE DOING
	/// </summary>
	/// <returns>The braking speed.</returns>
	private float CalculateBrakingSpeed()
	{
		return BRAKE_SPEED_DROP_FACTOR / (IsZero (this.CurrentVelocity) ? 1 : this.CurrentVelocity);
	}

	/// <summary>
	/// Calculates the current velocity acceleration modifier.
	/// DO NOT TOUCH THESE KIND OF FUNCTIONS UNLESS YOU REALLY KNOW WHAT YOU'RE DOING
	/// </summary>
	/// <returns>The current velocity acceleration modifier.</returns>
	/// <param name="curMaxSpeed">Current max speed.</param>
	private float CalculateCurrentVelocityAccelerationModifier(float curMaxSpeed)
	{
		return (((2f * curMaxSpeed) - curMaxSpeed) / curMaxSpeed);
	}

	/// <summary>
	/// Sets the given velocity realistic.
	/// DO NOT TOUCH THESE KIND OF FUNCTIONS UNLESS YOU REALLY KNOW WHAT YOU'RE DOING
	/// </summary>
	/// <returns>The realistic version of the velocity.</returns>
	/// <param name="curVelocity">Current velocity.</param>
	private float SetVelocityRealistic(float curVelocity)
	{
		return (curVelocity / REALISM_FACTOR);
	}

	/// <summary>
	/// Flips the specified value from for example, 1 to -1 and vice versa.
	/// LET THIS FUNCTION ALONE.
	/// </summary>
	/// <param name="val">Value.</param>
	private float Flip(float val)
	{
		return val * -1;
	}

	/// <summary>
	/// Gets half of the given value
	/// </summary>
	/// <returns>Halved value.</returns>
	/// <param name="val">Value.</param>
	private float HalfOf(float val)
	{
		return val / 2;
	}

	private float DegToRad(float value)
	{
		return (value) * (Mathf.PI / 180);
	}
	
	private float RadToDeg(float value)
	{
		//return value;
		return (value) / (Mathf.PI * 180);
	}

	/// <summary>
	/// Gets all the wheels.
	/// </summary>
	/// <returns>All the wheels.</returns>
	private List<GameObject> GetAllWheels()
	{
		return new List<GameObject> () { this.FrontLeft, this.FrontRight, this.BackLeft, this.BackRight };
	}
}
