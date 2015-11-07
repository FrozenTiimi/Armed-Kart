using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the player camera. To be changed, A LOT
/// </summary>
public class CameraHandler : MonoBehaviour 
{

	/// Do not change these
	/// They handle camera offsets, and they are fine, for the moment.
	/// </summary>
	const int yOffsetMagicNumber = 25;
	const int zOffsetMagicNumber = 35;

	// Do not use this for initialization
	private void Start () 
	{ }
	
	// Update is *ignored* once per frame
	private void Update () 
	{
		// Get the player, is there any other way? This seems dumb and risky.
		var player = GameObject.Find("NewPlayer");

		transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + yOffsetMagicNumber, player.transform.position.z + zOffsetMagicNumber);
		transform.LookAt (player.transform.position);

		var heading = player.transform.position - transform.position;
		var distance = heading.magnitude;
		var direction = heading / distance;

		Debug.DrawRay (transform.position, heading, Color.black);

		var ray = new Ray (transform.position, heading);
		var hit = new RaycastHit ();

		if (Physics.Raycast (ray, out hit)) 
		{
			if (hit.collider.GetType() == typeof(UnityEngine.TerrainCollider))
				transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		}


		//Debug.DrawLine (transform.position, player.transform.position - transform.position, Color.black);
		//Debug.DrawRay (transform.position, (player.transform.position - transform.position).normalized, Color.red);
	}

}
