using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles all the buttons in the main menu
/// </summary>
public class MainMenuButtonHandler : MonoBehaviour 
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

			if (NextScene == "Quit")
			{
				// quit the game
				Application.Quit ();
				return; // stop the application from continuing while in debugging mode
			}
			// load the level based on NextScene
			Application.LoadLevel (NextScene);
		});
	}
}
