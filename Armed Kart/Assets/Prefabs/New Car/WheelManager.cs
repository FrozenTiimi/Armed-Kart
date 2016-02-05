using UnityEngine;
using System.Collections;

public class WheelManager : MonoBehaviour 
{
	private CarEngine engineScript;

	// Use this for initialization
	void Start () 
	{
		this.engineScript = this.GetComponentInParent<CarEngine> ();
	}
	
	// Rotate the wheels;
	void Update () 
	{
		transform.Rotate (new Vector3(0, 0, this.engineScript.GetRB().velocity.magnitude));
	}
}
