using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NameFieldHandler : MonoBehaviour 
{
	public int NameMinimumLength = 4;
	public int NameMaximumLength = 15;

	// Update is called once per frame
	private void Update () {
		var textComponents = this.GetComponentsInChildren<Text> ();
		var realTextComponent = textComponents [textComponents.Length - 1];

		if (realTextComponent.text.Length > this.NameMaximumLength) 
			realTextComponent.text = realTextComponent.text.Substring (0, this.NameMaximumLength);

		if (realTextComponent.text.Length < this.NameMinimumLength) 
		{
			for (var i = 0; i < transform.parent.childCount; i++)
			{
				var t = transform.parent.GetChild(i);
				var buttonComponent = t.GetComponent<Button>();

				if (buttonComponent != null)
					buttonComponent.interactable = false;
			}
		} 
		else 
		{
			for (var i = 0; i < transform.parent.childCount; i++)
			{
				var t = transform.parent.GetChild(i);
				var buttonComponent = t.GetComponent<Button>();
				
				if (buttonComponent != null)
					buttonComponent.interactable = true;
			}
		}
	}
}
