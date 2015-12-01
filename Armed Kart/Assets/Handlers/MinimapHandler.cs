using UnityEngine;
using System.Linq;
using System.Collections;

public class MinimapHandler : MonoBehaviour 
{
	private Camera HandledCamera { get; set; }

	private bool FollowingMinimap { get; set; }

	private readonly object[] StaticMinimapProperties = new object[] { new Vector3 (248, 430, 257), 238f };
	
	// Use this for initialization
	private void Start () 
	{
		this.FollowingMinimap = false;
	}
	
	// Update is called once per frame
	private void Update () 
	{
		this.HandledCamera = GetComponent<Camera> ();

		if (Input.GetKeyDown (KeyCode.F2) && Input.GetKey (KeyCode.LeftShift)) 
		{
			if (this.FollowingMinimap) 
			{
				if (this.HandledCamera.orthographicSize > 20)
					this.HandledCamera.orthographicSize -= 10;
			}
		} 
		else if (Input.GetKeyDown (KeyCode.F2) && Input.GetKey (KeyCode.LeftControl)) 
		{
			if (this.FollowingMinimap) 
			{
				if (this.HandledCamera.orthographicSize < 100)
					this.HandledCamera.orthographicSize += 10;
			}
		}
		else if (Input.GetKeyDown (KeyCode.F2)) 
		{
			this.FollowingMinimap = !this.FollowingMinimap;

			if (!this.FollowingMinimap) 
			{
				transform.position = (Vector3)StaticMinimapProperties[0];
				this.HandledCamera.orthographicSize = (float)StaticMinimapProperties[1];
			}
			else
				this.HandledCamera.orthographicSize = 100f;
		}

		// this seems dumb
		var player = GameObject.FindGameObjectWithTag("Player");

		if (this.FollowingMinimap)
			transform.position = new Vector3 (player.transform.position.x, 430, player.transform.position.z);

	}
}
