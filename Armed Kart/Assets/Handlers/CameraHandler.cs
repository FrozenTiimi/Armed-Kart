﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the player camera. To be changed, A LOT
/// </summary>
public class CameraHandler : MonoBehaviour 
{

	/// <summary>
	/// Do not change these
	/// They handle camera offsets, and they are fine, for the moment.
	/// </summary>
	const int yOffsetMagicNumber = 5;
	const int zOffsetMagicNumber = 20;

	// Do not use this for initialization
	private void Start () 
	{ }
	
	// Update is *ignored* once per frame
	private void Update () 
	{
		// Get the player, is there any other way? This seems dumb and risky.
		var player = transform.parent;

		//transform.position = new Vector3 (player.position.x, player.position.y + yOffsetMagicNumber, player.position.z - zOffsetMagicNumber);

		//TODO: Add something in here?
		//TODO: DO SOMETHING IN HERE
		//TODO: HELP PLS
	}

}