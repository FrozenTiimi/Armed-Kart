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
	protected CharacterController Player { get; set; }

	private Vector3 StartingPosition { get; set; }

	/// <summary>
	/// The GRAVITY constant, like in real life (9.81 m/s)
	/// </summary>
	const float GRAVITY = 9.81f;

	// Use this for initialization
	protected void Start () 
	{
		this.StartingPosition = transform.position;
	}
	
	// Update is called once per frame
	protected void Update () 
	{
		this.Player = GetComponent<CharacterController> ();

		this.Player.Move (new Vector3 (0, this.Player.isGrounded ? 0 : -GRAVITY, 0) * Time.deltaTime);

		if (transform.position.y < 0) // H4X0R DETECTED !!!!!!!!!
			transform.position = this.StartingPosition; // Go back to your corner of shame
	}
}
