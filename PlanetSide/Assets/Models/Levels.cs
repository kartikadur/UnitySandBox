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
		int CurrentDirection;

		//All these are single dimensional as the respective classes store their level coordinates
		//Also individual elements can be accessed using x*width + y
		Models.Surfaces[] surfaceModels;
		Action<Models.Surfaces> surfaceChangedCallBacks;

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
					surfaceModels [x * Width + y].Terrain = Models.Surfaces.TerrainType.Empty;
					surfaceModels [x * Width + y].RegisterTerrainCallBack (surfaceChanged);
				}
			}

			//FIXME:
			// for now this function initializes the level and creates a prototype 
			// of all the structures in this level for tracking purposes

			structurePrototypes = new Dictionary<Structures.StructureType, Structures> ();

			structurePrototypes.Add(Models.Structures.StructureType.Road,
				Models.Structures.createStructure (Models.Structures.StructureType.Road, 
					1f, //movement cost
					1, // surfaces occupied in x dir
					1,  // surfaces occupied in y dir
					true //Links to neighbors
				));

			structurePrototypes.Add(Models.Structures.StructureType.Wall,
				Models.Structures.createStructure (Models.Structures.StructureType.Wall, 
					0f, //movement cost
					1, // surfaces occupied in x dir
					1,  // surfaces occupied in y dir
					true //Links to neighbors
				));

			structurePrototypes.Add(Models.Structures.StructureType.House,
				Models.Structures.createStructure (Models.Structures.StructureType.House, 
					0f, //movement cost
					2, // surfaces occupied in x dir
					2,  // surfaces occupied in y dir
					false //Links to neighbors
				));

//			Debug.Log ("No. of items in structure prototypes: " + structurePrototypes.Count);
//			Debug.Log ("Has Object of type House: " + structurePrototypes.ContainsKey (Models.Structures.StructureType.House));
			Debug.Log ("Models.Levels -> createLevel : created Structure prototypes ");
			//Build prototype for all strucutre types
//			foreach (Models.Structures.StructureType type in Enum.GetValues(typeof(Models.Structures.StructureType))) {
//				structurePrototypes.Add(type,
//					Models.Structures.createStructure (type, 
//						0f, //movement cost
//						1, // surfaces occupied in x dir
//						1,  // surfaces occupied in y dir
//						true //Links to neighbors
//					));
//			}
//			Debug.Log ("Models.Levels -> createLevel : created Structure prototypes ");

			
		}

		public void surfaceChanged(Models.Surfaces surfaceModel) {
			Debug.Log ("Models.Levels --> structure changed : something related to the surface changed updating surface");
			//Check if surface existis ?

			if (surfaceChangedCallBacks != null && surfaceModel != null) {
				surfaceChangedCallBacks (surfaceModel);
			}
		}

		public void placeStructure(Models.Structures.StructureType type, Models.Surfaces surfaceModel) {
			Debug.Log ("Models.Levels -> placeStructure : trying to place structure of type : " + type);
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


		/* FIXME: currently cardinal directions are assumed as follows
		 * North is x - 1
		 * East is y - 1
		 * South is x + 1
		 * West is y + 1
		 * other directions are a combination of the above
		 * handling edge cases x = 0, x = level.width - 1, y = 0, and y = level.height - 1?
		 * sends an int where a 1 in the following positions indicates a neightbor of the same type
		 */
		public string CheckForNeighbors(int x, int y, Models.Structures.StructureType type) {


			//Return this when done
			//Keeps track of directions in which the current tile has neighbors;
			string direction = "";

			//North
			if (x < this.width - 1 && this.GetSurfaceAt (x + 1, y).Structure != null && this.GetSurfaceAt (x + 1, y).Structure.Type == type) {
				direction += "N";
			}
			//East
			if (y > 0 && this.GetSurfaceAt (x, y - 1).Structure != null && this.GetSurfaceAt (x, y - 1).Structure.Type == type) {
				direction += "E";
			}
			//South
			if (x > 0 && this.GetSurfaceAt (x - 1, y).Structure != null && this.GetSurfaceAt (x - 1, y).Structure.Type == type) {
				direction += "S";
			}
			//West
			if (y < this.height - 1 && this.GetSurfaceAt (x, y + 1).Structure != null && this.GetSurfaceAt (x, y + 1).Structure.Type == type) {
				direction += "W";
			}

			//Temp Check for final direction of Structure
			Debug.Log("this structure has same structures in directions " + direction);

//			this.GetSurfaceAt (x, y).Structure = flag;
			//FIXME: Place holder, currently return the structure's direction as is.
			return direction;
		}

		//TODO: temporary for now might be removed later
		public Models.Surfaces.TerrainType randomizeTerrain() {
			return (Models.Surfaces.TerrainType)UnityEngine.Random.Range (0, Enum.GetNames (typeof(Models.Surfaces.TerrainType)).Length);
		}

		//Surface Models Callback Registers
		public void RegisterSurfaceChangedCallBack(Action<Models.Surfaces> callback) {
			surfaceChangedCallBacks += callback;
		}

		public void UnregisterSurfaceChangedCallBack(Action<Models.Surfaces> callback) {
			surfaceChangedCallBacks -= callback;
		}

		//Structure Models Callback Registers
		public void RegisterStructurePlacedCallBack(Action<Models.Structures> callback) {
			structurePlacedCallBacks += callback;
		}

		public void UnregisterStructurePlacedCallBack(Action<Models.Structures> callback) {
			structurePlacedCallBacks -= callback;
		}



	}
}