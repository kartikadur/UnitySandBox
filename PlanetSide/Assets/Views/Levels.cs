using UnityEngine;
using System;
using System.Collections;

namespace Views {
	public class Levels : MonoBehaviour {

		Models.Levels level;

		public Sprite Forest;
		public Sprite Lake;
		public Sprite Mountain;
		public Sprite Plain;

		Views.Surfaces[] surfaceViews;

		//Half width of sprite in unity units
		float xOffset = 0.5f;
		//half height of sprite in unity units
		float yOffset = 0.25f;

		// Use this for initialization
		void Start () {

			//
			int width = 5;
			int height = 5;
			level = new Models.Levels(width, height);
			surfaceViews = new Views.Surfaces[width * height];

			for (int x = 0; x < level.Width; x++) {
				for (int y = level.Height - 1; y >= 0; y--) {
					Models.Surfaces surfaceModel = level.GetSurfaceAt (x, y);
					GameObject gameObject = new GameObject ();
					gameObject.name = "Surface_" + x + "_" + y;
					gameObject.transform.position = Utility.ConvertCartesianToIsometric (new Vector3 (x, y, 0), xOffset, yOffset);
					gameObject.transform.SetParent (this.transform, true);

					surfaceViews [x * level.Width + y] = new Views.Surfaces (this, surfaceModel.X, surfaceModel.Y, gameObject);
					surfaceViews [x * level.Width + y].GameObject.AddComponent<SpriteRenderer> ();
					//Adds or registers the sprite the first time it is created
					OnSurfaceTerrainChanged (surfaceModel, gameObject);

					//Registers a callback so that a surface can handle subsequent terrain changes
					surfaceModel.RegisterCallBack ((surface) => {
						//Debug.Log("Call back register");
						OnSurfaceTerrainChanged (surface, gameObject);
					});
				}
			}


		}
		
		// Update is called once per frame
		void Update () {
			//Debug.Log (randomizeTerrain ());
		
		}

		public void OnSurfaceTerrainChanged(Models.Surfaces surfaceModel, GameObject gameObject) {
			//Debug.Log("On surface changed");

			if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Forest) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Forest;
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Lake) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Lake;
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Mountain) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Mountain;
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Plain) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Plain;
			} else {
				Debug.LogError ("Views.Levels - Trying to assign sprite based on terrain type, but terrain type not found");
			}

		}

		/* Utility functions */
		//FIXME: create body for these functions
		protected Vector3 ConvertIsometrictToCartesian(Vector3 point, float xOffset, float yOffset){
			//TODO: find conversion metrics

			return new Vector3 (point.x, point.y, point.z);
		}

	}//End class
}//End namespace