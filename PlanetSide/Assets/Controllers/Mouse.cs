using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;


public class Mouse : MonoBehaviour {


	Models.Levels levelModel;
	Vector3 currentMousePosition;
	Vector3 lastMousePosition;
	Vector3 startPoint;
	Vector3 endPoint;

	bool isBuildModeActive = false;
	Models.Structures.StructureType structureToBuild;
	Models.Surfaces.TerrainType terrainToApply;

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
			/* ("Mouse Input Coordinates: (" + ISOpoint.x + ", " + ISOpoint.y + ")");
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
							if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Mountain) {
								Debug.Log ("Mouse Controller -> Update : cannot build on mountains");
							} else {
								BuildStructure (structureToBuild, surfaceModel);
							}
						} else {
							SetTerrainType (terrainToApply, surfaceModel);
						}
					}
					
				}
			}
		}


		//Track where the mouse ends up
		lastMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);

	}

	/// <summary>
	/// Build calls for establishing terrain on surface
	/// Set[TerrainType] will allow the user change the surface from any terrain to TerrainType
	/// </summary>
	public void SetPlain() {
		isBuildModeActive = false;
		terrainToApply = Models.Surfaces.TerrainType.Plain;
	}

	public void SetMountain() {
		isBuildModeActive = false;
		terrainToApply = Models.Surfaces.TerrainType.Mountain;
	}

	public void SetLake() {
		isBuildModeActive = false;
		terrainToApply = Models.Surfaces.TerrainType.Lake;
	}

	public void SetTerrainType(Models.Surfaces.TerrainType terrain, Models.Surfaces surfaceModel) {
		Debug.Log ("Controllers.Mouse --> SetTerrainType : terrain " + terrain);
		surfaceModel.Terrain = terrain;
	}

	/// <summary>
	/// Build calls for establishing structures on surface
	/// Build[StructureType] will allow the user to create a structure of BuildingType on the surface
	/// </summary>
	public void BuildWall() {
		isBuildModeActive = true;
		structureToBuild = Models.Structures.StructureType.Wall;
	}

	//Function call to set road building mode
	public void BuildRoad() {
		isBuildModeActive = true;
		structureToBuild = Models.Structures.StructureType.Road;
	}

	//Function call to set house building mode
	public void BuildHouse() {
		isBuildModeActive = true;
		structureToBuild = Models.Structures.StructureType.House;
	}

	protected void BuildStructure(Models.Structures.StructureType type, Models.Surfaces surfaceModel) {
		Debug.Log ("Controllers.Mouse -> BuildStructure : building " + type);
		levelModel.placeStructure (type, surfaceModel);
	}
}
