using UnityEngine;
using System.Collections;

public class TerrainHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerExit(Collider other)
	{
		if (other.transform.parent.tag == "Player") 
		{
			other.transform.parent.GetComponent<CarEngine>().Reset();
		}
	}
}
