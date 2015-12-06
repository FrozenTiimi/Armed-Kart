using UnityEngine;
using System.Linq;
using System.Collections;

public class MinimapHandler : MonoBehaviour 
{
	public string ForPlayer;

	private GameObject Player { get; set; }

	private Camera HandledCamera { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the minimap is a following minimap or not.
	/// </summary>
	/// <value><c>true</c> if following minimap; otherwise, <c>false</c>.</value>
	private bool FollowingMinimap { get; set; }

	private readonly object[] StaticMinimapProperties = new object[] { new Vector3 (248, 430, 257), 238f };
	
	// Use this for initialization
	private void Start () 
	{
		this.FollowingMinimap = false;

		var rText = new RenderTexture (256, 256, 32);
		transform.GetComponent<Camera> ().targetTexture = rText;

		this.Player = GameObject.Find (ForPlayer);
		this.Player.transform.FindChild ("Overlay").GetComponent<GUIHandler> ().Minicam = rText;
	}
	
	// Update is called once per frame
	private void Update () 
	{
		this.HandledCamera = GetComponent<Camera> ();

		if (this.FollowingMinimap)
			transform.position = new Vector3 (this.Player.transform.position.x, 430, this.Player.transform.position.z);

	}

	public void ZoomIn()
	{
		if (this.FollowingMinimap) 
		{
			if (this.HandledCamera.orthographicSize > 20)
				this.HandledCamera.orthographicSize -= 10;
		}
	}

	public void ZoomOut()
	{
		if (this.FollowingMinimap) 
		{
			if (this.HandledCamera.orthographicSize < 100)
				this.HandledCamera.orthographicSize += 10;
		}
	}

	public void SwitchFollowingMinimap()
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
}
