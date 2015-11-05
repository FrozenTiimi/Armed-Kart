using UnityEngine;
using System.Collections;

/// <summary>
/// This is the base-entity handler for all entities. 
/// PLEASE INHERIT THIS TO ALL ENTITY HANDLING CLASSES
/// </summary>
public class BasicEntityHandler : MonoBehaviour 
{

	/// <summary>
	/// Gets or sets the charactercontroller (player)
	/// </summary>
	/// <value>The player.</value>
	private CharacterController Player { get; set; }

	private Vector3 StartingPosition { get; set; }

	/// <summary>
	/// The GRAVITY constant
	/// </summary>
	const float GRAVITY = 55f;
	const float GRAVITY_ADD = 0.5f;

	private float CurrentGravity = 0f;

	// Use this for initialization
	private void Start () 
	{
		this.StartingPosition = transform.position;
	}
	
	// Update is called once per frame
	private void Update () 
	{
		this.Player = GetComponent<CharacterController> ();

		var carWeight = 2f; // default;

		if (GetComponent <PlayerHandler>() ?? null) // checks if PlayerHandler is not found
		{
			carWeight = GetComponent<PlayerHandler> ().PlayerCar.GetCarWeight ();
			carWeight += 1f;
		} 

		var verticalSpeed = 0f;

		// IMPROVED GRAVITY
		if (this.Player.isGrounded)
		{
			if (CurrentGravity > 0)
				CurrentGravity = 0;

			verticalSpeed = -(this.Player.stepOffset * carWeight) / Time.deltaTime;
		}
		else
		{
			if (CurrentGravity <= GRAVITY - GRAVITY_ADD)
				CurrentGravity += GRAVITY_ADD;

			verticalSpeed -= (CurrentGravity * carWeight) * Time.deltaTime;
		}

		this.Player.Move (new Vector3 (0, verticalSpeed, 0));

		if (transform.position.y < 0) // H4X0R DETECTED !!!!!!!!!
			transform.position = this.StartingPosition; // Go back to your corner of shame
	}

	private void LateUpdate()
	{/*
		this.Player.transform.Rotate(new Vector3(
			this.Player.transform.rotation.x, this.Player.transform.rotation.y / 2, this.Player.transform.rotation.z
			));
	*/
	}

	private void OnCollisionEnter(Collision other)
	{
		Debug.Log ("Collision enter !!!!" + other.collider.name);
	}

	private void OnCollisionStay(Collision other)
	{
		Debug.Log ("Collision !!!!" + other.collider.name);
	}
}
