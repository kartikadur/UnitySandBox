using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Mouse : MonoBehaviour {


	Models.Levels levelModel;
	Vector3 ISOpoint;
	Vector3 CRTpoint;


	// Use this for initialization
	void Start () {

		levelModel = Models.Levels.Instance;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonUp (0)) {
			ISOpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			CRTpoint = Utility.ConvertIsometricToCartesian (ISOpoint);

			Debug.Log ("Mouse Input Coordinates: (" + ISOpoint.x + ", " + ISOpoint.y + ")");
			Debug.Log ("Surface Coordinates: (" + CRTpoint.x + ", " + CRTpoint.y + ")");
			Debug.Log ("-----------");

			if (levelModel.GetSurfaceAt ((int)CRTpoint.x, (int)CRTpoint.y) != null) {
				levelModel.GetSurfaceAt ((int)CRTpoint.x, (int)CRTpoint.y).Terrain = levelModel.randomizeTerrain ();
			} else {
				Debug.Log ("Mouse Controller - Outside Map Bounds");
			}
		}
	}
}
