using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour 
{
	private void Awake()
	{
		// Safety switch
		//GameObject.FindGameObjectWithTag ("extracam").GetComponent<Camera> ().enabled = true;
		GameObject.FindGameObjectWithTag ("Cam1").GetComponent<Camera> ().enabled = true;
		GameObject.FindGameObjectWithTag ("Cam2").GetComponent<Camera> ().enabled = false;
	}

}
