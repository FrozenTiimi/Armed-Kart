using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Linq;

using ArmedKart.Utilities;

/// <summary>
/// Obsoleted and deprecated.
/// DO NOT USE
/// USE CAR ENGINE
/// </summary>
public class PlayerHandler : MonoBehaviour 
{
	const int ROTATION_MULTIPLIER = 100;

	private Rigidbody Car { get; set; }
	private GameObject PlayerCamera { get; set; }

	private bool IsRotating { get; set; }

	public  bool IsBraking = false;

	public float CurrentVelocity = 0f;
	public float CurrentRotation = 0f;
	public float Friction = 10f; //TODO: This should be a CAR PROPERTY
	public float Health = 0f;

	public CarTypes CarType;

	public CarHandler PlayerCar;

	private bool HasFinishedRace = false;

	/// <summary>
	/// Finishes the race for the player.
	/// </summary>
	public void FinishRace()
	{
		this.HasFinishedRace = true;
	}

	/// <summary>
	/// Damages the car.
	/// </summary>
	/// <param name="damage">Amount of damage.</param>
	public void DamageCar(float damage)
	{
		this.Health -= damage;

		if (this.Health < 0)
			this.Health = 0;
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start()
	{
		//this.name = GameObject.Find ("NameField").GetComponentsInChildren<UnityEngine.UI.Text> () [1].text;
		//var playerCarMeshName = ApplicationModel.GetCarMesh (this.name);


		this.Car = this.GetComponent<Rigidbody> ();

		this.Car.centerOfMass = new Vector3 (0f, 0f, 1.0f);

		var meshFilter = gameObject.AddComponent<MeshFilter> ();

		switch (CarType) 
		{
			case 0:
			default:
//				PlayerCar = new TestCar();
				//meshFilter.sharedMesh = Resources.Load<Mesh>(PlayerCar.ModelLocation);
				break;
		}

		if (meshFilter.mesh != null)
			UnityEngine.Debug.Log ("Player Car Mesh loaded successfully!");
		else
			UnityEngine.Debug.LogError ("Player Car Mesh is null");
	}

	/// <summary>
	/// The time.DeltaTime'd update method
	/// </summary>
	private void FixedUpdate()
	{
	}

	/// <summary>
	/// Update that happens after the real Update
	/// </summary>
	private void LateUpdate()
	{
		this.Car.angularVelocity = Vector3.zero;
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	private void Update()
	{
		this.PlayerCamera = this.PlayerCamera = GameObject.FindGameObjectsWithTag 
			("PlayerCamera").Where (t => t.GetComponentInChildren<CameraHandler> ().AttachedPlayer == transform.name).FirstOrDefault ();

		this.PlayerCamera.GetComponentInChildren<Camera> ().enabled = true;

		if (this.IsDead ()) 
		{
			//TODO: Make le explosions happen
			//Destroy(this.gameObject);
		} 
		else 
		{
			this.HandlePlayerRotation ();
			this.HandlePlayerMovement ();
		}
	}

	/// <summary>
	/// Handles the player rotation.
	/// </summary>
	private void HandlePlayerRotation()
	{
		var curMaxSpeed = PlayerCar.GetMaxSpeed ();
		
		//TODO: Make this rotation more realistic
		// Simple rotation
		//if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) 
		//{
			/*var rot = Input.GetAxis("Horizontal");
			
			// 1 is full, -1 is full
			rot *= curMaxSpeed * ROTATION_FACTOR;
			rot += (CurrentVelocity - (curMaxSpeed / ROTATION_FACTOR)) + (false ? (CurrentVelocity / ROTATION_FACTOR) : 0); // this handles rotation speed
			rot /= curMaxSpeed * ROTATION_FACTOR;
			
			rot *= ROTATION_MULTIPLIER;
			
			// Jerpan driftaus juttuja
			//if (this.IsDrifting)
			//	rot *= 2.5f;
			
			this.CurrentRotation = rot;
			
			if (this.CurrentVelocity > 1 || this.CurrentVelocity < -1)
			{
				IsRotating = true;
				
				this.Car.MoveRotation(Quaternion.Euler (this.Car.rotation.eulerAngles) * Quaternion.Euler (0, rot * Time.deltaTime, 0));
			}
			else
			{
				IsRotating = false;
			}
			*/
		var deltaRotation = (Input.GetAxis ("Horizontal") * Time.deltaTime) * ROTATION_MULTIPLIER;

		//TODO: This is going to be drifting, implement it correctly
		if (false)
			deltaRotation *= 1.5f;

		this.Car.MoveRotation(this.Car.rotation * Quaternion.Euler (0, 0, deltaRotation));
		//}
	}

	/// <summary>
	/// Handles the player movement.
	/// </summary>
	private void HandlePlayerMovement()
	{
		var curMaxSpeed = PlayerCar.GetMaxSpeed ();
		var curMaxReverseSpeed = PlayerCar.GetCarReverseSpeed ();
		var curPower = PlayerCar.GetCarPower ();

		var speedFactor = curPower;

		if (Input.GetKeyDown (KeyCode.S)) 
		{
			this.IsBraking = true;

			speedFactor = -curPower;
		} 
		else
			this.IsBraking = false;

		if (this.CurrentVelocity > 250f)
			speedFactor = -curPower;
		else if (this.CurrentVelocity < -curMaxReverseSpeed)
			speedFactor = curPower;

		this.CurrentVelocity += speedFactor;

		this.CurrentVelocity *= 0.25f;
		//var curMaxSpeed = PlayerCar.GetMaxSpeed ();

		var speedModifier = Vector3.right * this.GetGameVelocity (this.CurrentVelocity, curMaxSpeed);

		this.Car.velocity = transform.forward * this.GetRealisticVelocity(this.CurrentVelocity);
		transform.Translate (this.IsBraking ? speedModifier * -1 : speedModifier);
	}

	/// <summary>
	/// Gets whether the player has finished the race or not
	/// </summary>
	/// <returns><c>true</c>, if has finished race, <c>false</c> otherwise.</returns>
	public bool GetHasFinishedRace()
	{
		return this.HasFinishedRace;
	}

	public bool IsDamaged()
	{
		return this.Health < this.PlayerCar.GetCarHealth ();
	}

	public bool IsDead()
	{
		return this.Health <= 0;
	}

	private float GetRealisticVelocity(float velocity)
	{
		return velocity / 3;
	}

	private float GetGameVelocity(float velocity, float maxVelocity)
	{
		return (velocity / maxVelocity) * Time.deltaTime;
	}
}

//TODO: This is currently ported for PC. PORT TO ANDROID LATER

/*
/// <summary>
/// Handles the player's movement, rotation, etc.
/// </summary>
public class PlayerHandler : MonoBehaviour 
{
	/// <summary>
	/// Gets or sets the player's character controller.
	/// </summary>
	/// <value>The player (character controller).</value>
	private Rigidbody Player { get; set; }
	private GameObject PlayerMinimap { get; set; }
	private MinimapHandler PlayerMinimapHandler { get; set; }
	private GameObject PlayerCamera { get; set; }

	/// <summary>
	/// Set this to TRUE if you want, for example, the race to continue after you've finished all the laps
	/// </summary>
	public bool DebugMode = false;
	
	/// <summary>
	/// Gets the front right wheel.
	/// </summary>
	/// <value>The front right wheel.</value>
	public GameObject FrontRight { get { return transform.FindChild ("wheel_frontright").gameObject; } }
	/// <summary>
	/// Gets the front left wheel.
	/// </summary>
	/// <value>The front left wheel.</value>
	public GameObject FrontLeft { get { return transform.FindChild ("wheel_frontleft").gameObject; } }
	/// <summary>
	/// Gets the back right wheel.
	/// </summary>
	/// <value>The back right wheel.</value>
	public GameObject BackRight { get { return transform.FindChild ("wheel_backright").gameObject; } }
	/// <summary>
	/// Gets the back left wheel.
	/// </summary>
	/// <value>The back left wheel.</value>
	public GameObject BackLeft { get { return transform.FindChild ("wheel_backleft").gameObject; } }

	/// <summary>
	/// The type of the car.
	/// </summary>
	public CarTypes CarType;

	/// <summary>
	/// To-be-set in the Start() method. LOOK THERE.
	/// The player's car.
	/// </summary>
	public CarHandler PlayerCar;

	/// <summary>
	/// The car's current velocity, limit is car type's maximum speed
	/// </summary>
	public float CurrentVelocity = 0f;
	/// <summary>
	/// The current rotation of the car/player.
	/// </summary>
	public float CurrentRotation = 0f;

	/// <summary>
	/// The current X, Y, Z speed of the player
	/// </summary>
	public Vector3 CurrentSpeed;

	/// <summary>
	/// DO NOT >GET< THIS VALUE, ONLY >SET<
	/// FOR GETTING, USE GetHasFinishedRace() function, which also takes into consideration the debug mode, etc.
	/// </summary>
	private bool HasFinishedRace = false;
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
	private bool IsBraking { get; set; }
	private bool IsDrifting { get; set; }

	private float CollideAngle = 0f;

	/// <summary>
	/// Used for debugging purposes, e.g. how fast the player accelerates to 100km/h
	/// </summary>
	private Stopwatch timer = new Stopwatch ();

	const float COLLISION_SPEED_FACTOR = 2f;
	/// <summary>
	/// The realism factor.
	/// Without this, the car would be really fucking fast.
	/// The smaller this is, the faster the car goes.
	/// </summary>
	const float REALISM_FACTOR = 1.5f; // used to divide the velocity of the car on-screen
	/// <summary>
	/// The rotation factor.
	/// Handles the rotation speed.
	/// </summary>
	const float ROTATION_FACTOR = 3f;
	const float FORCE_MULTIPLIER = 7f; // May the Force be with you
	const float REVERSE_FORCE_MULTIPLIER = 20f; // May the... reverse Force...? be with you
	const float GRAVITY_MULTIPLIER = 0.0f; // Used to calculate the intensity of gravity
	const float DRIFTING_SPEED_DROP_FACTOR = 6f; // Used to make the drifting more realistic
	const float SPEED_PENALTY = 10f; // Used to calculate the new speed factor if velocity is too low or high, 
									 // disables the "Big Rigs" style of reverse movement for example

	/// <summary>
	/// The rotation multiplier.
	/// Handles the rotation speed.
	/// </summary>     
	const int ROTATION_MULTIPLIER = 60;
	const int SPEED_DROP_FACTOR = 2;    
	const float BRAKE_SPEED_DROP_FACTOR = 5f;
	const int FLAT_GROUND = 90; // flat ground collision angle
	const int FULL_CIRCLE = 360;
	const int PENALTY_DIVIDER = 14;  

	/// <summary>
	/// Why is this here?
	/// Because publics first.
	/// </summary>
	public void FinishRace()
	{
		this.HasFinishedRace = true;
	}

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
		/*
		 * THIS IS FOR JANNE:
		 * Where(Action)-funktio looppaa arrayn läpi, jonka tuo FindGameObjectsWithTag(string)-funktio saa,
		 * ja palauttaa jokaisen objektin sieltä arraysta, jonka esim. noissa tapauksissa ForPlayer on yhtäsuuri kuin transform.name
		 * ja FirstOrDefault() ottaa joko ensimmäisen itemin Where(Action)-funktion palauttamasta arraysta, tai jos ei itemejä ole, palauttaa defaulttiarvon.
		 */ 
/*
		this.Player = GetComponentInChildren<Rigidbody> ();
		
		this.PlayerMinimap = GameObject.FindGameObjectsWithTag ("Minicamera").Where (t => t.GetComponentInChildren<MinimapHandler> ().ForPlayer == transform.name).FirstOrDefault();
		this.PlayerMinimapHandler = this.PlayerMinimap.GetComponentInChildren<MinimapHandler> ();
		this.PlayerCamera = GameObject.FindGameObjectsWithTag ("PlayerCamera").Where (t => t.GetComponentInChildren<CameraHandler> ().AttachedPlayer == transform.name).FirstOrDefault ();
		
		this.PlayerCamera.GetComponentInChildren<Camera> ().enabled = true;
		
		this.HandlePlayerMinimap ();
		this.HandlePlayerDrifting ();
		this.HandlePlayerRotation ();
		this.HandlePlayerMovement ();
	}

	/// <summary>
	/// Happens after Update()
	/// </summary>
	private void LateUpdate()
	{
		this.Player.angularVelocity = Vector3.zero;		
	}

	private void HandlePlayerDrifting()
	{
		this.IsDrifting = this.CurrentVelocity > this.PlayerCar.GetMaxSpeed () * 0.75;
	}

	private void HandlePlayerMinimap()
	{
		if (Input.GetKeyDown (KeyCode.F2) && Input.GetKey (KeyCode.LeftShift)) 
		{
			this.PlayerMinimapHandler.ZoomIn();
		} 
		else if (Input.GetKeyDown (KeyCode.F2) && Input.GetKey (KeyCode.LeftControl)) 
		{
			this.PlayerMinimapHandler.ZoomOut();
		}
		else if (Input.GetKeyDown (KeyCode.F2)) 
		{
			this.PlayerMinimapHandler.SwitchFollowingMinimap();
		}
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
			rot *= curMaxSpeed * ROTATION_FACTOR;
			rot += (CurrentVelocity - (curMaxSpeed / ROTATION_FACTOR)) + (this.IsBraking ? (CurrentVelocity / ROTATION_FACTOR) : 0); // this handles rotation speed
			rot /= curMaxSpeed * ROTATION_FACTOR;
			
			rot *= ROTATION_MULTIPLIER;

			// Jerpan driftaus juttuja
			if (this.IsDrifting)
				rot *= 2.5f;

			this.CurrentRotation = rot;

			if (this.CurrentVelocity > 1 || this.CurrentVelocity < -1)
			{
				IsRotating = true;

				this.Player.MoveRotation(Quaternion.Euler (Player.rotation.eulerAngles) * Quaternion.Euler (0, rot * Time.deltaTime, 0));
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

		var curMaxSpeedNoThrust = MathUtils.HalfOf (curMaxSpeed);

		var speedFactor = 0f;

		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.S) && !GetHasFinishedRace()) // if shift + w is pressed
		{	

			if (!timer.IsRunning && this.CurrentVelocity.IsZero())
			{
				timer = new Stopwatch();
				timer.Start ();
			}

			if (this.CurrentVelocity > curMaxSpeed) // if current velocity is higher than current max speed
			{
				speedFactor = -SPEED_DROP_FACTOR; // then we slow down the car, stops bugs
			}
			else if (this.CurrentVelocity <  curMaxSpeed.Half ()) // if current velocity is less than current max speed halved.
			{
				speedFactor = (speedFactors[1].Multiply() / (acceleration / carWeight));
			}
			else if (this.CurrentVelocity < (curMaxSpeed - curMaxSpeed.Quarter()))
			{
				speedFactor = speedFactors[0].Multiply() / (acceleration / carWeight);
			}
			else if (this.CurrentVelocity < curMaxSpeed) // if current velocity is less than current max speed
			{
				var penalty = speedFactors[0] / PENALTY_DIVIDER;
				speedFactor = (speedFactors[0].Multiply() / (acceleration / carWeight)) - penalty;
			}

			if (this.CurrentVelocity >= 100 && timer.IsRunning)
			{
				timer.Stop ();
				UnityEngine.Debug.Log ("We accelerated to 100km/h in " + timer.ElapsedMilliseconds + " ms");
			}
		}
		else if (Input.GetKey (KeyCode.W) && !GetHasFinishedRace() && !Input.GetKey (KeyCode.S)) // if not
		{
			if (this.CurrentVelocity > curMaxSpeedNoThrust)
			{
				speedFactor = -SPEED_DROP_FACTOR;
			}
			else if (this.CurrentVelocity < MathUtils.HalfOf (curMaxSpeedNoThrust))
			{
				speedFactor = speedFactors[1] / (acceleration / carWeight);
			}
			else if (this.CurrentVelocity < (curMaxSpeedNoThrust - curMaxSpeedNoThrust.Quarter()))
			{
				speedFactor = speedFactors[0] / (acceleration / carWeight);
			}
			else if (this.CurrentVelocity < curMaxSpeedNoThrust)
			{
				var penalty = speedFactors[0] / PENALTY_DIVIDER;
				speedFactor = (speedFactors[0] / (acceleration / carWeight)) - penalty;
			}
		}
		else
		{
			/*
			 * This steps in after no key is pressed, so, this stops the car
			 */
/*
			if (this.CurrentVelocity > SPEED_DROP_FACTOR)
			{
				speedFactor = -SPEED_DROP_FACTOR;
			}
			else
			{
				if (!Input.GetKey (KeyCode.S))
				{
					speedFactor.SetToZero();
					CurrentVelocity.SetToZero();
				}
			}
		}

		/*
		 * Handles the braking
		 */
/*
		if (Input.GetKey (KeyCode.S) && !GetHasFinishedRace()) 
		{
			this.IsBraking = true;

			speedFactor -= this.CalculateBrakingSpeed ();

			if (this.CurrentVelocity < 0) // reverse speed modifier
				speedFactor += this.CurrentVelocity;
		} 
		else
			this.IsBraking = false;

		if (IsRotating)
			speedFactor /= turnSpeedFactor;

		if (IsRotating && IsDrifting)
			speedFactor = MathUtils.Flip (speedFactor) / DRIFTING_SPEED_DROP_FACTOR;

		if (this.CurrentVelocity < -curMaxReverseSpeed)
			speedFactor = SPEED_PENALTY;
		else if (this.CurrentVelocity > this.PlayerCar.GetMaxSpeed ())
			speedFactor = -SPEED_PENALTY;

		this.CurrentVelocity += speedFactor;

		this.CurrentVelocity /= this.CalculateCurrentVelocityAccelerationModifier(curMaxSpeed);

		var zModifier = MathUtils.Flip (this.SetVelocityRealistic (this.CurrentVelocity.Round ())); // we flip the current velocity and make it realistic
		var speed = new Vector3 ( 0, 0/*zModifier*//*, /*0 );
		speed = transform.FindChild("Model").rotation * speed; // we times the speed by the rotation quaternion to make the car actually move in the right direction

		this.CurrentSpeed = speed;

		speed = speed * Time.deltaTime; // we make the speed not dependent on FPS

		// Get all the car's wheels
		var wheels = this.GetAllWheels ();

		// Loop through them
		wheels.ForEach (w =>
		                {
			//TODO: Fix this!!!!!!! THE SECOND PARAMETER IS THE, UHM... a
			//		YOU KNOW... WHEN YOU TURN THE CAR... ROTATION... THINGY... I HOPE YOU KNOW
			w.transform.Rotate (new Vector3(0, this.CurrentVelocity, 0 ));
		});

		this.Player.velocity = speed;
		var rotQuaternion = this.Player.rotation * Vector3.forward;
		this.Player.AddForce (rotQuaternion * (this.CurrentVelocity < 0 ? 
		                                                                MathUtils.Flip (this.CurrentVelocity) * REVERSE_FORCE_MULTIPLIER : 
		                                                                (-this.CurrentVelocity * FORCE_MULTIPLIER)));
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
	/// Gets whether the player has finished the race or not
	/// </summary>
	/// <returns><c>true</c>, if has finished the race, <c>false</c> otherwise.</returns>
	public bool GetHasFinishedRace()
	{
		if (this.DebugMode)
			return false;

		return this.HasFinishedRace;
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
	/// Calculates the braking speed.
	/// DO NOT TOUCH THESE KIND OF FUNCTIONS UNLESS YOU REALLY KNOW WHAT YOU'RE DOING
	/// </summary>
	/// <returns>The braking speed.</returns>
	private float CalculateBrakingSpeed()
	{
		return BRAKE_SPEED_DROP_FACTOR / (this.CurrentVelocity.IsZero() ? 1 : this.CurrentVelocity);
	}

	/// <summary>
	/// Calculates the current velocity acceleration modifier.
	/// DO NOT TOUCH THESE KIND OF FUNCTIONS UNLESS YOU REALLY KNOW WHAT YOU'RE DOING
	/// </summary>
	/// <returns>The current velocity acceleration modifier.</returns>
	/// <param name="curMaxSpeed">Current max speed.</param>
	private float CalculateCurrentVelocityAccelerationModifier(float curMaxSpeed)
	{
		return ((curMaxSpeed.Multiply() - curMaxSpeed) / curMaxSpeed);
	}

	/// <summary>
	/// Sets the given velocity realistic.
	/// DO NOT TOUCH THESE KIND OF FUNCTIONS UNLESS YOU REALLY KNOW WHAT YOU'RE DOING
	/// </summary>
	/// <returns>The realistic version of the velocity.</returns>
	/// <param name="curVelocity">Current velocity.</param>
	private float SetVelocityRealistic(float curVelocity)
	{
		return (curVelocity / (REALISM_FACTOR));
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
*/
