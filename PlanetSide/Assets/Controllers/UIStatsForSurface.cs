using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Controllers {
	/// <summary>
	/// User interface stats. 
	/// This class shows what tile and structure (if present) are under the mouse pointer
	/// </summary>
	public class UIStatsForSurface : MonoBehaviour {

		Text uiTextForSurface;
		Controllers.Mouse mouseController;
		Models.Surfaces surfaceModel;

		// Use this for initialization
		void Start () {

			mouseController = GameObject.FindObjectOfType<Controllers.Mouse>();
			uiTextForSurface = GetComponent<Text> ();

			if (uiTextForSurface == null) {
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
			
			surfaceModel = mouseController.GetSurfaceUnderMouse ();
			if (surfaceModel != null) {
				uiTextForSurface.text = "Terrain: " + surfaceModel.Terrain.ToString ();
			} else {
				uiTextForSurface.text = "Terrain: Empty";
			}
		}
	}
}