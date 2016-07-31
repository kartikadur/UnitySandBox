using UnityEngine;
using System;
using System.Collections;

namespace Views {
	public class Levels : MonoBehaviour {

		Models.Levels level;

		public Sprite Forest;
		public Sprite[] Lake;
		public Sprite[] Plain;
		public Sprite[] Mountain;

		Views.Surfaces[] surfaceViews;

		//offset holds half the width and height of tiles used in 2d space
		Vector3 offset = new Vector3(0.5f, 0.25f, 0f);
		//float xOffset = 0.5f;
		//half height of sprite in unity units
		//float yOffset = 0.25f;

		// Use this for initialization
		void Start () {

			//
			int width = 20;
			int height = 20;
			level = Models.Levels.Instance;
			level.createLevel(width, height);
			surfaceViews = new Views.Surfaces[width * height];

			for (int x = 0; x < level.Width; x++) {
				for (int y = 0; y < level.Height; y++) {
					Models.Surfaces surfaceModel = level.GetSurfaceAt (x, y);
					GameObject gameObject = new GameObject ();
					gameObject.name = "Surface_" + x + "_" + y;
					//gameObject.transform.position = Utility.ConvertCartesianToIsometric (new Vector3 (x, y, 0), xOffset, yOffset);
					gameObject.transform.SetParent (this.transform, true);

					surfaceViews [x * level.Width + y] = new Views.Surfaces (this, gameObject);
					surfaceViews [x * level.Width + y].GameObject.AddComponent<SpriteRenderer> ();


					Vector3 point = Utility.ConvertCartesianToIsometric (new Vector3 (x, y, 0), offset);
					surfaceViews [x * level.Width + y].SetPosition (point.x, point.y, point.z);
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
				gameObject.GetComponent<SpriteRenderer> ().sprite = Lake[UnityEngine.Random.Range(0, Lake.Length - 1)];
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Mountain) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Mountain[UnityEngine.Random.Range(0, Mountain.Length - 1)];
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Plain) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Plain[UnityEngine.Random.Range(0, Plain.Length - 1)];
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

		//Test function 
		public void ChangeRandomeSurfaceTerrain() {
			int randomX = UnityEngine.Random.Range (0, level.Width - 1);
			int randomY = UnityEngine.Random.Range (0, level.Height - 1);
			Models.Surfaces surface = level.GetSurfaceAt (randomX, randomY);
			surface.Terrain = level.randomizeTerrain ();
		}

	}//End class
}//End namespace