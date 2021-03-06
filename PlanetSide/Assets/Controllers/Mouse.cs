﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

namespace Controllers {
	public class Mouse : MonoBehaviour {


		Models.Levels levelModel;
		Vector3 currentMousePosition;
		Vector3 lastMousePosition;
		Vector3 startPoint;
		Vector3 endPoint;

		[HideInInspector]
		public bool isBuildModeActive = false;
		[HideInInspector]
		public Models.Structures.StructureType structureToBuild;
		[HideInInspector]
		public Models.Surfaces.TerrainType terrainToApply;

		// Use this for initialization
		void Start () {

			levelModel = Models.Levels.Instance;
		
		}

		/// <summary>
		/// Gets the mouse position in world.
		/// returns the mouse position converted from screen coordinates to world coordinates
		/// </summary>
		/// <returns>The mouse position in world.</returns>
		Vector3 GetMousePositionInWorld() {
			return Camera.main.ScreenToWorldPoint (Input.mousePosition);
		}

		/// <summary>
		/// Gets the surface under mouse.
		/// </summary>
		/// <returns>The surface under mouse.</returns>
		public Models.Surfaces GetSurfaceUnderMouse() {
			Vector3 point = Utility.ConvertIsometricToCartesian (GetMousePositionInWorld ());
			Models.Surfaces surfaceModel = Models.Levels.Instance.GetSurfaceAt ((int)point.x, (int)point.y);
			return surfaceModel;
		}

		/// <summary>
		/// Gets the structure under mouse.
		/// </summary>
		/// <returns>The structure under mouse.</returns>
		public Models.Structures GetStructureUnderMouse() {
			Models.Structures structureModel;
			Models.Surfaces surfaceModel = GetSurfaceUnderMouse ();
			if (surfaceModel != null && surfaceModel.hasStructure () == true) {
				structureModel = GetSurfaceUnderMouse ().Structure;
				return structureModel;
			} else {
				return null;
			}
		}

		// Update is called once per frame
		void Update () {

			//If user clicks on a button then do nothin on screen
			if (EventSystem.current.IsPointerOverGameObject ()) {
				return;
			}

			currentMousePosition = GetMousePositionInWorld ();

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

				if (startPoint.x > endPoint.x) {
					Utility.Swap (ref startPoint.x, ref endPoint.x);
				}

				if (startPoint.y > endPoint.y) {
					Utility.Swap (ref startPoint.y, ref endPoint.y);
				}

				//FIXME: the logic for this will need to be changed based on what is being built and where
				// or what is changing and where
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
			lastMousePosition = GetMousePositionInWorld();

		}

		/// <summary>
		/// Sets the type of the terrain.
		/// </summary>
		/// <param name="terrain">Terrain.</param>
		/// <param name="surfaceModel">Surface model.</param>
		protected void SetTerrainType(Models.Surfaces.TerrainType terrain, Models.Surfaces surfaceModel) {
			Debug.Log ("Controllers.Mouse --> SetTerrainType : terrain " + terrain);
			surfaceModel.Terrain = terrain;
		}

		/// <summary>
		/// Builds the structure.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="surfaceModel">Surface model.</param>
		protected void BuildStructure(Models.Structures.StructureType type, Models.Surfaces surfaceModel) {
			Debug.Log ("Controllers.Mouse -> BuildStructure : building " + type);
			//TODO: currently it only checks if there is a structure,
			// in the future it should check for null values? or create a different system to remove structures
			levelModel.PlaceStructure (type, surfaceModel);
		}

	}
}