using UnityEngine;
using System.Collections;

public class GUIHandler : MonoBehaviour 
{
	public RenderTexture Minicam;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		var gS = new GUIStyle ();
		gS.normal.textColor = Color.red;

		var curFPS = 1.0f / Time.deltaTime;

		GUI.Label (new Rect (0, 0, 200, 200), "Armed Kart Pre-Alpha", gS);
		GUI.Label (new Rect (0, 16, 200, 200), "FPS: " + curFPS.ToString(), gS);
		GUI.DrawTexture (new Rect (Screen.width - 200, Screen.height - 200, 175, 175), Minicam);
	}
}
