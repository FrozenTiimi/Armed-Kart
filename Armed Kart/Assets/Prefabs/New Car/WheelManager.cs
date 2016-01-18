using UnityEngine;
using System.Collections;

public class WheelManager : MonoBehaviour 
{
	public GameObject FRW;
	public GameObject FLW;
	public GameObject RRW;
	public GameObject RLW;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Rotate the wheels;
	void Update () 
	{
		FRW.transform.Rotate (0, 0, 2000 * Time.deltaTime);
		FLW.transform.Rotate (0, 0, -2000 * Time.deltaTime);
		RRW.transform.Rotate (0, 0, 2000 * Time.deltaTime);
		RLW.transform.Rotate (0, 0, -2000 * Time.deltaTime);
	}
}
