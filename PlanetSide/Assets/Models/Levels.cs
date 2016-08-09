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
		//FIXME: Currently not implemented but may be used in map rotations later
		int CurrentDirection;

		//All these are single dimensional as the respective classes store their level coordinates
		//Also individual elements can be accessed using x*width + y
		Models.Surfaces[] surfaceModels;
		Action<Models.Surfaces> SurfaceChangedCallBacks;

		//These can be created on as needed basis
		Dictionary<Models.Structures.StructureType, Models.Structures> structurePrototypes;
		Action<Models.Structures> StructurePlacedCallBacks;

		//These can be created on as needed basis
		Dictionary<Models.Resources.ResourceType, Models.Resources> resourcePrototypes;
		Action<Models.Resources> resourcePlaceCallBacks; //or should this be used for extraction?
		//Something to track resouces extracted or collected for processing

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

		public void CreateEmptyLevel(int width = 10, int height = 10, string seed = "") {
			this.width = width;
			this.height = height;

			surfaceModels = new Models.Surfaces[Width * Height];

			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					surfaceModels [x * Width + y] = new Models.Surfaces (this, x, y);
					surfaceModels [x * Width + y].Terrain = Models.Surfaces.TerrainType.Empty;
					surfaceModels [x * Width + y].RegisterTerrainCallBack (SurfaceChanged);
				}
			}

			//FIXME:
			// for now this function initializes the level and creates a prototype 
			// of all the structures in this level for tracking purposes

			structurePrototypes = new Dictionary<Structures.StructureType, Structures> ();

			structurePrototypes.Add(Models.Structures.StructureType.Road,
				Models.Structures.CreateStructure (Models.Structures.StructureType.Road, 
					1f, //movement cost
					1, // surfaces occupied in x dir
					1,  // surfaces occupied in y dir
					true //Links to neighbors
				));

			structurePrototypes.Add(Models.Structures.StructureType.Wall,
				Models.Structures.CreateStructure (Models.Structures.StructureType.Wall, 
					0f, //movement cost
					1, // surfaces occupied in x dir
					1,  // surfaces occupied in y dir
					true //Links to neighbors
				));

			structurePrototypes.Add(Models.Structures.StructureType.House,
				Models.Structures.CreateStructure (Models.Structures.StructureType.House, 
					0f, //movement cost
					2, // surfaces occupied in x dir
					2,  // surfaces occupied in y dir
					false //Links to neighbors
				));

			Debug.Log ("Models.Levels -> CreateEmptyLevel : created Structure prototypes ");
		}


		/// <description>
		/// This subsection includes all function related to surfaces listed as follows
		/// GetSurfaceAt(int, int): get surface model at coordinate x, y
		/// SurfaceChanged(Models.Surfaces): callback triggering for surfaces that have changed
		/// 
		/// </description>

		public Models.Surfaces GetSurfaceAt(int x, int y) {
			if (x < width && x > -1 && y < height && y > -1) {
				return surfaceModels [x * width + y];
			}
			return null;
		}

		public void SurfaceChanged(Models.Surfaces surfaceModel) {
			Debug.Log ("Models.Levels --> structure changed : something related to the surface changed updating surface");
			//Check if surface existis ?

			if (SurfaceChangedCallBacks != null && surfaceModel != null) {
				SurfaceChangedCallBacks (surfaceModel);
			}
		}


		/// <description>
		/// This subsection includes all functions related to surfaces listed as follows
		/// PlaceStructure(Models.Structure.StructureType, Models.Surfaces): 
		/// 		creates and places a structure of type structuretype on surface.
		/// CheckForStructureConnections(int, int, Models.Structure.StructureType):
		/// 		chedk if the given structure type has connectable neighbors
		/// </description>
		public void PlaceStructure(Models.Structures.StructureType type, Models.Surfaces surfaceModel) {
			Debug.Log ("Models.Levels -> placeStructure : trying to place structure of type : " + type);
			if (structurePrototypes.ContainsKey (type) == false) {
				Debug.Log ("Models.Levels -> placeStructure : Cannot create structure of type " + type);
				return;
			}

			Models.Structures structureModel = Models.Structures.PlaceStructureOnSurface (structurePrototypes [type], surfaceModel);

			//either there are no callbacks or 
			//structureModel returns null as there is already a structure on the surface
			if (StructurePlacedCallBacks != null && structureModel != null) {
				Debug.Log ("Models.Levels -> placeStructure : callback for showing on screen");
				StructurePlacedCallBacks (structureModel);
			}
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

		public string CheckForStructureConnections(int x, int y, Models.Structures.StructureType type) {


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

		/// <description>
		/// Callbacks for associated classes are registered and unregistered here
		/// 1. Surface Changed
		/// 2. Structure Placed
		/// </description>

		//Surface Models Callback Registers
		public void RegisterSurfaceChangedCallBack(Action<Models.Surfaces> callback) {
			SurfaceChangedCallBacks += callback;
		}

		public void UnregisterSurfaceChangedCallBack(Action<Models.Surfaces> callback) {
			SurfaceChangedCallBacks -= callback;
		}

		//Structure Models Callback Registers
		public void RegisterStructurePlacedCallBack(Action<Models.Structures> callback) {
			StructurePlacedCallBacks += callback;
		}

		public void UnregisterStructurePlacedCallBack(Action<Models.Structures> callback) {
			StructurePlacedCallBacks -= callback;
		}
	}
}