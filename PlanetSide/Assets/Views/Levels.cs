using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Views {
	public class Levels : MonoBehaviour {

		Models.Levels level = Models.Levels.Instance;

		public Sprite Forest;
		public Sprite[] Lake;
		public Sprite[] Plain;
		public Sprite[] Mountain;

		public Sprite House;

		Views.Surfaces[] surfaceViews;

		Dictionary<Models.Surfaces, Views.Surfaces> surfaceModelViewMap;
		Dictionary<Models.Structures, Views.Structures> structureModelViewMap;

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
			level.RegisterStructurePlacedCallBack (OnStructurePlacedOnSurface);
			level.createLevel(width, height);
			surfaceViews = new Views.Surfaces[width * height];

			//Create Dictionary Maps
			surfaceModelViewMap = new Dictionary<Models.Surfaces, Surfaces>();
			structureModelViewMap = new Dictionary<Models.Structures, Structures> ();

			for (int x = 0; x < level.Width; x++) {
				for (int y = 0; y < level.Height; y++) {
					//Get Surface Model
					Models.Surfaces surfaceModel = level.GetSurfaceAt (x, y);

					//Create Game object for this surface view
					GameObject gameObject = new GameObject ();
					gameObject.name = "Surface_" + x + "_" + y;
					//gameObject.transform.position = Utility.ConvertCartesianToIsometric (new Vector3 (x, y, 0), xOffset, yOffset);
					gameObject.transform.SetParent (this.transform, true);

					//Create Surface View
					surfaceViews [x * level.Width + y] = new Views.Surfaces (this, gameObject);
					//Add sprite Renderer Component
					surfaceViews [x * level.Width + y].GameObject.AddComponent<SpriteRenderer> ();
					surfaceViews [x * level.Width + y].GameObject.GetComponent<SpriteRenderer> ().sortingOrder = Utility.SortingOrderNumber (level.Width, level.Height, x, y);
					//Place surface on map using its gameobject
					Vector3 point = Utility.ConvertCartesianToIsometric (new Vector3 (x, y, 0), offset);
					surfaceViews [x * level.Width + y].SetPosition (point.x, point.y);

					//Add view and model to dictionary
					surfaceModelViewMap.Add (surfaceModel, surfaceViews [x * level.Width + y]);

					//apply the surface terrain for the first time
					OnSurfaceTerrainChanged (surfaceModel, gameObject);

					//Registers a callback so that a surface can handle subsequent terrain changes
					surfaceModel.RegisterTerrainChangeCallBack ((surface) => {
						//Console.WriteLine("Call back register");
						OnSurfaceTerrainChanged (surface, gameObject);
					});
				}
			}


		}
		
		// Update is called once per frame
		void Update () {
			//Console.WriteLine (randomizeTerrain ());
			//FIXME: Everytime a new object is placed on screen check it's x,y coordinates and then place in correct draw order.


		}

		public void OnSurfaceTerrainChanged(Models.Surfaces surfaceModel, GameObject gameObject) {
			//Console.WriteLine("On surface changed");

			if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Forest) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Forest;
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Lake) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Lake[UnityEngine.Random.Range(0, Lake.Length)];
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Mountain) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Mountain[UnityEngine.Random.Range(0, Mountain.Length)];
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Plain) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Plain[UnityEngine.Random.Range(0, Plain.Length)];
			} else {
				Console.WriteLine ("Views.Levels - Trying to assign sprite based on terrain type, but terrain type not found");
			}

		}

		//Every time a new structure is added call this function to redraw or adjust the sorting layers
		//FIXME: uses naive solution to redraw everything on screen. future iterations will have to look for terrain clashes as well
		// Also a better solution will only look for items before itself in the sorting order or drawing order.
		protected void OnNewStructureAddedRedraw() {
			foreach (Models.Structures structureModel in structureModelViewMap.Keys) {
				//Debug.Log (structureModelViewMap [structureModel]);
				structureModelViewMap[structureModel].GameObject.GetComponent<SpriteRenderer>().sortingOrder = Utility.SortingOrderNumber(level.Width, level.Height, structureModel.SurfaceModel.X, structureModel.SurfaceModel.Y);
			}
		}

		//TODO: once a new structure is created, check its drawing order compared to all other structures allready placed.
		// then rearrange all the structures and place at correct position.
		public void OnStructurePlacedOnSurface(Models.Structures structureModel) {
			Console.WriteLine("Views.Levels -> OnStructurePlacedOnSurface : placing structure view");

			//Create Game Object
			GameObject gameObject = new GameObject();
			gameObject.name = "Structure_" + structureModel.SurfaceModel.X + "_" + structureModel.SurfaceModel.Y;

			//Set Parent?
			//Views.Surfaces surfaceView = surfaceModelViewMap[structureModel.SurfaceModel];
			gameObject.transform.SetParent (this.transform, true);

			Views.Structures structureView = new Views.Structures (this, gameObject);

			//FIXME: hardcoded for the moment change in the future.
			structureView.GameObject.AddComponent<SpriteRenderer> ().sprite = House;

			//Position the structure on to its surface
			Vector3 point = Utility.ConvertCartesianToIsometric (new Vector3 (structureModel.SurfaceModel.X, structureModel.SurfaceModel.Y, 0), offset);
			structureView.SetPosition (point.x, point.y, point.z);

			//When this is added check for its position, and then basically redraw all the tiles where overlap may occur.
			structureModelViewMap.Add (structureModel, structureView);
			//Redraw all the structures
			this.OnNewStructureAddedRedraw ();



			structureModel.RegisterStructureChangesCallBack ( (structure) => {
				OnStructureChanges(structure); 
			});
			Console.WriteLine ("Views.Levels -> OnStructurePlacedOnSurface : structure view placed");
		}

		public void OnStructureChanges(Models.Structures structureModel) {
			//TODO: implement this when a placed structure needs to expand, gets damaged, or destroyed
			Console.WriteLine("Views.Levels -> OnStructureChanges : Currently unimplementd");
		}

		/* Utility functions 
		//FIXME: create body for these functions
		protected Vector3 ConvertIsometrictToCartesian(Vector3 point, float xOffset, float yOffset){
			//TODO: find conversion metrics

			return new Vector3 (point.x, point.y, point.z);
		}

		//Test function : Not needed anymore
		public void ChangeRandomSurfaceTerrain() {
			int randomX = UnityEngine.Random.Range (0, level.Width - 1);
			int randomY = UnityEngine.Random.Range (0, level.Height - 1);
			Models.Surfaces surface = level.GetSurfaceAt (randomX, randomY);
			surface.Terrain = level.randomizeTerrain ();
		}*/

	}//End class
}//End namespace