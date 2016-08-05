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

		public Sprite[] House;
		public Sprite[] Wall;
		public Sprite[] Road;

		public Sprite[] roads;


		Views.Surfaces surfaceViews;

		Dictionary<Models.Surfaces, Views.Surfaces> surfaceModelViewMap;
		Dictionary<Models.Structures, Views.Structures> structureModelViewMap;
		Dictionary<string, Sprite> structureNameSpriteMap;

		//offset holds half the width and height of tiles used in 2d space
		Vector3 offset = new Vector3(0.5f, 0.25f, 0f);
		//float xOffset = 0.5f;
		//half height of sprite in unity units
		//float yOffset = 0.25f;


		//To ensure that all the sprites load before runtime
		void OnEnable() {

			// Temporary measure to ensure I can use named sprites instead of sprite arrays
			structureNameSpriteMap = new Dictionary<string, Sprite> ();

			//Debug.Log ("Runs First");
			roads = Resources.LoadAll<Sprite>("Sprites");


			foreach (var r in roads) {
				//Debug.Log ("Views.Levels --> Start : loaded sprite for - " + r);
				structureNameSpriteMap [r.name] = r;
			}
		}


		// Use this for initialization
		void Start () {

			//
			int width = 20;
			int height = 20;
			level.RegisterStructurePlacedCallBack (OnStructurePlacedOnSurface);
			level.createLevel(width, height);
			//surfaceViews = new Views.Surfaces[width * height];

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
					surfaceViews  = new Views.Surfaces (this, gameObject);

					//Add sprite Renderer Component and then add its sorting order
					surfaceViews.GameObject.AddComponent<SpriteRenderer> ();
					surfaceViews.GameObject.GetComponent<SpriteRenderer> ().sortingOrder = Utility.SortingOrderNumber (level.Width, level.Height, x, y);
					//Place surface on map using its gameobject
					Vector3 point = Utility.ConvertCartesianToIsometric (new Vector3 (x, y, 0), offset);
					surfaceViews .SetPosition (point.x, point.y);

					//Add view and model to dictionary
					surfaceModelViewMap.Add (surfaceModel, surfaceViews);

					//apply the surface terrain for the first time
					OnSurfaceTerrainChanged (surfaceModel, gameObject);

					//Registers a callback so that a surface can handle subsequent terrain changes
					surfaceModel.RegisterTerrainChangeCallBack ((surface) => {
						//Console.Write("Call back register");
						OnSurfaceTerrainChanged (surface, gameObject);
					});
				}
			}


		}
		
		// Update is called once per frame
		void Update () {
			//Console.Write (randomizeTerrain ());
			//FIXME: Everytime a new object is placed on screen check it's x,y coordinates and then place in correct draw order.


		}

		public void OnSurfaceTerrainChanged(Models.Surfaces surfaceModel, GameObject gameObject) {
			//Console.Write("On surface changed");

			if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Forest) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Forest;
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Lake) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Lake[UnityEngine.Random.Range(0, Lake.Length)];
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Mountain) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Mountain[UnityEngine.Random.Range(0, Mountain.Length)];
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Plain) {
				gameObject.GetComponent<SpriteRenderer> ().sprite = Plain[UnityEngine.Random.Range(0, Plain.Length)];
			} else {
				Console.Write ("Views.Levels - Trying to assign sprite based on terrain type, but terrain type not found");
			}

		}

		//Every time a new structure is added call this function to redraw or adjust the sorting layers
		//FIXME: uses naive solution to redraw everything on screen. future iterations will have to look for terrain clashes as well
		// Also a better solution will only look for items before itself in the sorting order or drawing order.
		/*protected void OnNewStructureAddedRedraw() {
			foreach (Models.Structures structureModel in structureModelViewMap.Keys) {
				//Console.Write (structureModelViewMap [structureModel]);
				structureModelViewMap[structureModel].GameObject.GetComponent<SpriteRenderer>().sortingOrder = Utility.SortingOrderNumber(level.Width, level.Height, structureModel.SurfaceModel.X, structureModel.SurfaceModel.Y);
			}
		}*/

		//TODO: once a new structure is created, check its drawing order compared to all other structures allready placed.
		// then rearrange all the structures and place at correct position.
		public void OnStructurePlacedOnSurface(Models.Structures structureModel) {
			Console.Write ("Views.Levels -> OnStructurePlacedOnSurface : placing structure view");

			//Create Game Object
			GameObject gameObject = new GameObject();
			gameObject.name = "Structure_" + structureModel.SurfaceModel.X + "_" + structureModel.SurfaceModel.Y;

			//Set Parent?
			//Views.Surfaces surfaceView = surfaceModelViewMap[structureModel.SurfaceModel];
			gameObject.transform.SetParent (this.transform, true);

			Views.Structures structureView = new Views.Structures (this, gameObject);


			//FIXME: hardcoded for the moment change in the future.

			/*
			 * Structure view currently statically assigned road tile
			 */
			//load structure sprite based on its string name in the structure name sprite map
			structureView.GameObject.AddComponent<SpriteRenderer> ().sprite = SpriteForStructure(structureModel);
			structureView.GameObject.GetComponent<SpriteRenderer> ().sortingOrder = Utility.SortingOrderNumber (level.Width, level.Height, structureModel.SurfaceModel.X, structureModel.SurfaceModel.Y);

			//Position the structure on to its surface
			Vector3 point = Utility.ConvertCartesianToIsometric (new Vector3 (structureModel.SurfaceModel.X, structureModel.SurfaceModel.Y, 0), offset);
			structureView.SetPosition (point.x, point.y, point.z);

			//When this is added check for its position, and then basically redraw all the tiles where overlap may occur.
			structureModelViewMap.Add (structureModel, structureView);

			structureModel.RegisterStructureChangesCallBack (OnStructureChanges);
			Console.Write ("Views.Levels -> OnStructurePlacedOnSurface : structure view placed");
		}

		public void OnStructureChanges(Models.Structures structureModel) {
			//TODO: implement this when a placed structure needs to expand, gets damaged, or destroyed
//			Console.Write("Views.Levels -> OnStructureChanges : Currently unimplementd");
			if (structureModelViewMap.ContainsKey (structureModel) == false) {
				Debug.Log ("Views.Levels --> OnstructureChanges : something went horribly wrong, cant find the structure model in the model-view map");
			} else {
				Views.Structures structureView = structureModelViewMap[structureModel];
				structureView.GameObject.GetComponent<SpriteRenderer> ().sprite = SpriteForStructure (structureModel);
			}
		}

		public Sprite SpriteForStructure(Models.Structures structureModel) {

			int x, y;

			//Currently a hack to get string name from structure.structureType
			string spriteName = structureModel.Type + "";

			if (structureModel.LinksToNeighbor == false) {
				//Since the object does not bother with connectivity to its neighbors it is assumed to have only one sprite
				//later iterations can randomize this to some extent
				return structureNameSpriteMap [spriteName];
			}


			//Get the coordinates for the surface on which the structure is created
			x = structureModel.SurfaceModel.X;
			y = structureModel.SurfaceModel.Y;

			//Get spritename based on links to neighbors
			spriteName += "_";
			spriteName += level.CheckForNeighbors (x, y, structureModel.Type);


			if (structureNameSpriteMap.ContainsKey (spriteName) == false) {
				Debug.Log ("Views.Levels --> SpriteForStructure : Cannot find sprite with name " + spriteName);
				//FIXME: Should return a placeholder of somekind or a null value?
				return null;
			} else {
				return structureNameSpriteMap [spriteName];
			}
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