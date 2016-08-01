using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Models {
	/// <summary>
	/// Models.Level tracks data related to the level as a whole and for surfaces, buildins, and resources as a set rather than individuals
	/// int height, int width : Dimensions of the level
	/// int ID or string name: level identifier in case there are multiple levels in a game.
	/// []/Dictionary surfaceModels : list of surfaces in the level (create a dictionary?)
	/// []/Dictionary structureModels : list of structures in the level (create a dictionary?)
	/// []/Dictionary resourceModels : list of resources in the level (create a dictionary?)
	/// </summary>

	//This class should essentially behave as a singleton to prevent mutiple copies of the same level or different levels.
	public class Levels {

		private static readonly Levels _instance = new Levels();
		static Levels() {}
		private Levels() {}

		public static Levels Instance {
			get {
				return _instance;
			}
		}

		int width, height;

		//All these are single dimensional as the respective classes store their level coordinates
		//Also individual elements can be accessed using x*width + y
		Models.Surfaces[] surfaceModels;

		//These can be created on as needed basis
		Dictionary<Models.Structures.StructureType, Models.Structures> structurePrototypes;
		Action<Models.Structures> structurePlacedCallBacks;

		Models.Resources[] resourceModels;

		public int Width {
			get {
				return width;
			}
		}

		public int Height {
			get {
				return height;
			}
		}

		public void createLevel(int width = 10, int height = 10, string seed = "") {
			this.width = width;
			this.height = height;

			surfaceModels = new Models.Surfaces[Width * Height];

			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					surfaceModels [x * Width + y] = new Models.Surfaces (this, x, y);
					surfaceModels [x * Width + y].Terrain = randomizeTerrain ();
				}
			}

			//FIXME:
			// for now this function initializes the level and creates a prototype 
			// of all the structures in this level for tracking purposes

			structurePrototypes = new Dictionary<Structures.StructureType, Structures> ();

			//Build prototype for all strucutre types
			foreach (Models.Structures.StructureType type in Enum.GetValues(typeof(Models.Structures.StructureType))) {
				structurePrototypes.Add(type,
					Models.Structures.createStructure (type, 
						0f, //movement cost
						1, // surfaces occupied in x dir
						1  // surfaces occupied in y dir
					));
			}
			Debug.Log("Models.Levels -> createLevel : created Structure prototypes ");

			
		}

		//TODO: Currently empty function, but could be used to make surface tiles
		// in the future when this functionality is required in the editor in the future
		public void createSurface() {
		}

		public void placeStructure(Models.Structures.StructureType type, Models.Surfaces surfaceModel) {
			Debug.Log ("Models.Levels -> placeStructure : trying to place structure on surface");
			if (structurePrototypes.ContainsKey (type) == false) {
				Debug.Log ("Models.Levels -> placeStructure : Cannot create structure of type " + type);
				return;
			}

			Models.Structures structureModel = Models.Structures.placeStructureOnSurface (structurePrototypes [type], surfaceModel);

			//either there are no callbacks or 
			//structureModel returns null as there is already a structure on the surface
			if (structurePlacedCallBacks != null && structureModel != null) {
				Debug.Log ("Models.Levels -> placeStructure : callback for showing on screen");
				structurePlacedCallBacks (structureModel);
			}
		}

		public Models.Surfaces GetSurfaceAt(int x, int y) {
			if (x < width && x > -1 && y < height && y > -1) {
				return surfaceModels [x * width + y];
			}
			return null;
		}

		//TODO: temporary for now might be removed later
		public Models.Surfaces.TerrainType randomizeTerrain() {
			return (Models.Surfaces.TerrainType)UnityEngine.Random.Range (0, Enum.GetNames (typeof(Models.Surfaces.TerrainType)).Length);
		}

		public void RegisterStructurePlacedCallBack(Action<Models.Structures> callback) {
			structurePlacedCallBacks += callback;
		}

		public void UnregisterStructurePlacedCallBack(Action<Models.Structures> callback) {
			structurePlacedCallBacks -= callback;
		}



	}
}