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
	/// The GRAVITY constant, like in real life (9.81 m/s)
	/// </summary>
	const float GRAVITY = 9.81f;

	// Use this for initialization
	private void Start () 
	{
		this.StartingPosition = transform.position;
	}
	
	// Update is called once per frame
	private void Update () 
	{
		this.Player = GetComponent<CharacterController> ();

		var verticalSpeed = 0f;

		// IMPROVED GRAVITY
		if (this.Player.isGrounded)
		{
			verticalSpeed = -this.Player.stepOffset / Time.deltaTime;
		}
		else
		{
			verticalSpeed -= GRAVITY * Time.deltaTime;
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
