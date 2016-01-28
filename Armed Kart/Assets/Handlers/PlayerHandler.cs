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

