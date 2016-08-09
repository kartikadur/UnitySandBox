using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIStatsForStructure : MonoBehaviour {

	Text uiTextForStructure;
	Controllers.Mouse mouseController;
	Models.Structures structureModel;

	// Use this for initialization
	void Start () {
		mouseController = GameObject.FindObjectOfType<Controllers.Mouse>();
		uiTextForStructure = GetComponent<Text> ();

		if (uiTextForStructure == null) {
			Debug.LogError ("Controllers.UIStats --> Cant find the ui text elements");
			return;
		}

		if (mouseController == null) {
			Debug.LogError ("UISTATS: WTF?");
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
		structureModel = mouseController.GetStructureUnderMouse ();
		if (structureModel != null) {
			uiTextForStructure.text = "Structure: " + structureModel.Type.ToString ();
		} else {
			uiTextForStructure.text = "Structure: Empty";
		}
	}
}
