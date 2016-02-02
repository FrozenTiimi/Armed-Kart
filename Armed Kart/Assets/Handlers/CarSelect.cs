using UnityEngine;
using System.Collections;

public class CarSelect : MonoBehaviour 
{

	
	// Use this for initialization
	void Start () 
	{

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
