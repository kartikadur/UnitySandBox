using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Views {
	public class Levels : MonoBehaviour {

		private static readonly Levels _instance = null;
		static Levels() {}
		private Levels() {}

		public static Levels viewLevelInstance {
			get;
			protected set;
		}


		Models.Levels level = Models.Levels.Instance;

		public Sprite Empty;
		public Sprite[] Lake;
		public Sprite[] Plain;
		public Sprite[] Mountain;

		public Sprite[] sprites;


		Views.Surfaces surfaceViews;

		Dictionary<Models.Surfaces, Views.Surfaces> surfaceModelViewMap;
		Dictionary<Models.Structures, Views.Structures> structureModelViewMap;
		Dictionary<string, Sprite> SpriteNameMap;

		//offset holds half the width and height of tiles used in 2d space
		Vector3 offset = new Vector3(0.5f, 0.25f, 0f);
		//float xOffset = 0.5f;
		//half height of sprite in unity units
		//float yOffset = 0.25f;


		//To ensure that all the sprites load before runtime
		void OnEnable() {

			// Temporary measure to ensure I can use named sprites instead of sprite arrays
			SpriteNameMap = new Dictionary<string, Sprite> ();

			//Debug.Log ("Runs First");
			sprites = Resources.LoadAll<Sprite>("Sprites");


			foreach (var s in sprites) {
				//Debug.Log ("Views.Levels --> Start : loaded sprite for - " + r);
				SpriteNameMap [s.name] = s;
			}

			if (viewLevelInstance != null) {
				Debug.Log ("Views.Level --> on enable : cant have two instances of levelView");
			}
			viewLevelInstance = this;
		}


		// Use this for initialization
		void Start () {

			//
			int width = 20;
			int height = 20;
			level.RegisterStructurePlacedCallBack (OnStructurePlacedOnSurface);
//			level.RegisterSurfaceChangedCallBack (OnSurfaceTerrainCreated);
			level.CreateEmptyLevel(width, height);
			//surfaceViews = new Views.Surfaces[width * height];

			//Create Dictionary Maps
			surfaceModelViewMap = new Dictionary<Models.Surfaces, Surfaces>();
			structureModelViewMap = new Dictionary<Models.Structures, Structures> ();

			for (int x = 0; x < level.Width; x++) {
				for (int y = 0; y < level.Height; y++) {
					//Get Surface Model
					Models.Surfaces surfaceModel = level.GetSurfaceAt (x, y);
					this.OnSurfaceTerrainCreated (surfaceModel);
				}
			}

			//Tell the level model about any changes to individual surfaces instead of registering with each surface
			level.RegisterSurfaceChangedCallBack (OnSurfaceTerrainChanged);


		}
		
		// Update is called once per frame
		void Update () {
			//Console.Write (randomizeTerrain ());
			//FIXME: Everytime a new object is placed on screen check it's x,y coordinates and then place in correct draw order.


		}


		/// <description>
		/// Surface Methods for all surface models in level view
		/// </description>

		public void OnSurfaceTerrainCreated(Models.Surfaces surfaceModel) {
			
			int x = surfaceModel.X;
			int y = surfaceModel.Y;
			GameObject gameObject = new GameObject ();
			Vector3 point = Utility.ConvertCartesianToIsometric (new Vector3 (x, y, 0), offset);

			surfaceModel.Terrain = Models.Surfaces.TerrainType.Empty;

//			Debug.Log ("Views.Level --> OnSurfaceTerrainCreated : Creating surface view for surface models");
			//Create an object of Views.Surface class
			Views.Surfaces surfaceView = new Views.Surfaces(gameObject);
			surfaceView.GameObject.name = "Surface_" + x + "_" + y;
			surfaceView.GameObject.transform.SetParent (viewLevelInstance.transform, true);

			surfaceView.GameObject.AddComponent<SpriteRenderer> ();
			surfaceView.GameObject.GetComponent<SpriteRenderer> ().sprite = Empty;
			surfaceView.GameObject.GetComponent<SpriteRenderer> ().sortingOrder = Utility.SortingOrderNumber (level.Width, level.Height, x, y);
			surfaceView.SetPosition (point.x, point.y);

			//FIXME: for now all the sprite tiles will be empty

			surfaceModelViewMap.Add (surfaceModel, surfaceView);

//			surfaceModel.RegisterTerrainCallBack (OnSurfaceTerrainChanged);
//			Debug.Log ("Views.Level --> OnSurfaceTerrainCreated : SurfaceModel Created and displayed on screen " + surfaceModelViewMap.Count);
		}

		public void OnSurfaceTerrainChanged(Models.Surfaces surfaceModel) {
			Debug.Log ("On surface changed");

			if (surfaceModelViewMap.ContainsKey (surfaceModel) == false) {
				Debug.Log ("Views.Levels --> On surface Changed : surface model does not exist");
				return;
			}

			Views.Surfaces surfaceView = surfaceModelViewMap [surfaceModel];

			//FIXME: Major Re-Write Needed
			if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Empty) {
				surfaceView.GameObject.GetComponent<SpriteRenderer> ().sprite = Empty;
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Lake) {
				surfaceView.GameObject.GetComponent<SpriteRenderer> ().sprite = Lake[UnityEngine.Random.Range(0, Lake.Length)];
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Mountain) {
				surfaceView.GameObject.GetComponent<SpriteRenderer> ().sprite = Mountain[UnityEngine.Random.Range(0, Mountain.Length)];
			} else if (surfaceModel.Terrain == Models.Surfaces.TerrainType.Plain) {
				surfaceView.GameObject.GetComponent<SpriteRenderer> ().sprite = Plain[UnityEngine.Random.Range(0, Plain.Length)];
			} else {
				Debug.Log ("Views.Levels - Trying to assign sprite based on terrain type, but terrain type not found");
			}

		}


		/// <description>
		/// Methods for all structures on surfaces in the level view
		/// </description>


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

			Views.Structures structureView = new Views.Structures (viewLevelInstance, gameObject);

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
				return SpriteNameMap [spriteName + "_1"];
			}


			//Get the coordinates for the surface on which the structure is created
			x = structureModel.SurfaceModel.X;
			y = structureModel.SurfaceModel.Y;

			//Get spritename based on links to neighbors
			spriteName += "_";
			spriteName += level.CheckForStructureConnections (x, y, structureModel.Type);


			if (SpriteNameMap.ContainsKey (spriteName) == false) {
				Debug.Log ("Views.Levels --> SpriteForStructure : Cannot find sprite with name " + spriteName);
				//FIXME: Should return a placeholder of somekind or a null value?
				return null;
			} else {
				return SpriteNameMap [spriteName];
			}
		}

	}//End class
}//End namespace