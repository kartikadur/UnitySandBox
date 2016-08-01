using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class Mouse : MonoBehaviour {


	Models.Levels levelModel;
	Vector3 ISOpoint;
	Vector3 CRTpoint;
	Vector3 currentMousePosition;
	Vector3 lastMousePosition;
	Vector3 startPoint;
	Vector3 endPoint;

	bool isBuildModeActive = false;
	Models.Structures.StructureType structureToBuild;

	// Use this for initialization
	void Start () {

		levelModel = Models.Levels.Instance;
	
	}
	
	// Update is called once per frame
	void Update () {

		//If user clicks on a button then do nothin on screen
		if (EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}

		currentMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		//TODO: right-click to cancel build mode
		if (Input.GetMouseButtonUp (1)) {
			isBuildModeActive = false;
		}

		//If the user clicks on screen (but not on the UI) then do the following
		// check if in building mode (??)
		// register startX and startY to register dragging
		if (Input.GetMouseButtonDown (0)) {
			startPoint = Utility.ConvertIsometricToCartesian (currentMousePosition);
		}


		//If the user releases the button after clicking on it then do the following
		// register endX and endY
		if (Input.GetMouseButtonUp (0)) {
			endPoint = Utility.ConvertIsometricToCartesian (currentMousePosition);
			/*Debug.Log ("Mouse Input Coordinates: (" + ISOpoint.x + ", " + ISOpoint.y + ")");
			Debug.Log ("Surface Coordinates: (" + CRTpoint.x + ", " + CRTpoint.y + ")");
			Debug.Log ("-----------");

			if (levelModel.GetSurfaceAt ((int)CRTpoint.x, (int)CRTpoint.y) != null) {
				levelModel.GetSurfaceAt ((int)CRTpoint.x, (int)CRTpoint.y).Terrain = levelModel.randomizeTerrain ();
			} else {
				Debug.Log ("Mouse Controller - Outside Map Bounds");
			}
			*/

			if (startPoint.x > endPoint.x) {
				Utility.Swap (ref startPoint.x, ref endPoint.x);
			}

			if (startPoint.y > endPoint.y) {
				Utility.Swap (ref startPoint.y, ref endPoint.y);
			}

			for (int x = (int)startPoint.x; x <= (int)endPoint.x; x++) {
				for (int y = (int)startPoint.y; y <= (int)endPoint.y; y++) {
					Models.Surfaces surfaceModel = Models.Levels.Instance.GetSurfaceAt (x, y);
					if (surfaceModel != null) {
						Debug.Log ("Building on surface : " + surfaceModel.X + ", " + surfaceModel.Y);
						if (isBuildModeActive == true) {
							BuildStructure (structureToBuild, surfaceModel);
						} else {	
							surfaceModel.Terrain = Models.Surfaces.TerrainType.Mountain;
						}
					}
					
				}
			}
		}


		//Track where the mouse ends up
		lastMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

	}


	//Build Structures
	public void BuildHouse() {
		//BuildStructure (Models.Structures.StructureType.House);
		isBuildModeActive = true;
		structureToBuild = Models.Structures.StructureType.House;
	}

	protected void BuildStructure(Models.Structures.StructureType type, Models.Surfaces surfaceModel) {
		Debug.Log ("Controllers.Mouse -> BuildStructure : building " + type);
		levelModel.placeStructure (type, surfaceModel);
	}
}
