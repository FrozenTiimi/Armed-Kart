using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

public class CarButtonHandler : MonoBehaviour 
{
	public AudioClip menuSound;
	public string carName;

	private void Start () 
	{
		DontDestroyOnLoad(GameObject.Find ("NameField"));

		GetComponent<Button> ().onClick.AddListener (() =>
		{
			var source = GetComponent<AudioSource>();
			source.PlayOneShot(menuSound);
			
			// = "car_model_Valtteri";
			ApplicationModel.AddCar (GameObject.Find ("NameField").GetComponentsInChildren<Text>()[1].text/* todo: change */, this.carName);
			//ApplicationMode.GetCarMesh("Player");

			EditorUtility.DisplayDialog("PLAYER NAME", GameObject.Find ("NameField").GetComponentsInChildren<Text>()[1].text, "kekmem");
		
			GameObject.Find ("NameField").SetActive(false);

			Application.LoadLevel ("Level1");
		});
	}
}
