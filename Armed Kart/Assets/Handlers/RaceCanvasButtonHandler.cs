using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles all the buttons in the main menu
/// </summary>
public class RaceCanvasButtonHandler : MonoBehaviour 
{
	/// <summary>
	/// The next scene to navigate to after pressing the button
	/// </summary>
	public string NextScene;
	public AudioClip menuSound;
	
	/// <summary>
	/// The starting method for this handler
	/// </summary>
	private void Start () 
	{
		GetComponent<Button> ().onClick.AddListener (() =>
        {
			var source = GetComponent<AudioSource>();
			source.PlayOneShot(menuSound);
			
			// load the level based on NextScene
			Application.LoadLevel (NextScene);
		});
	}
}