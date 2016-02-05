using UnityEngine;
using System.Collections;

public class CarSelect : MonoBehaviour 
{
	public GameObject carSpot;

	
	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(carSpot);
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate (0, 0, 20 * Time.deltaTime);
	}

	public void ChangeCar(bool forward)
	{
		if (forward) 
		{

		} 
		else
		{
	
		}
	}
}
