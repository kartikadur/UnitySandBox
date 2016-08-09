using UnityEngine;
using System;
using System.Collections;

namespace Models {
	/// <summary>
	/// Structures. 
	/// for this class do I want to build one prototype and then clone it
	/// this would allow for multiple calculations for building inputs and outputs to be handled in a single place
	/// but that might not be the best idea in the long run. maybe another class may handle all the calculations
	/// </summary>
	public class Structures {

		//List of all structure types that will be used in the level
		//TODO: Import this list from somewhere to make it more dynamic?
		//Also should this list inclued other characteristics?
		// resources required, resources created, workers required, desirability, No. of surface squares occupied, etc.
		public enum StructureType { Wall, Road, House };

		//FIXME: create accessor methods to protect this variable
		StructureType type;
//		StructureNeighbors neighbors;

		public StructureType Type {
			get {
				return type;
			}
		}
			
		/*
		 * TODO:
		 * if the structure creates a resource then handle that
		 * if the structure needs a resource to function (humans, food, etc.)
		 * track resouces using var/params
		 */

		Action<Models.Structures> StructureCallBacks;

		Func<Models.Surfaces,bool> PositionValidationFunctions;

		//Maybe there should be an array of surface models in case it belongs to more than one surface?
		Models.Surfaces surfaceModel;

		public Models.Surfaces SurfaceModel {
			get {
				return surfaceModel;
			}
			protected set {
				surfaceModel = value;
			}
		}

		//this will indicate if structure occupied just 1 or more than 1 surface on screen.
		int width;

		public int Width {
			get {
				return width;
			}
			protected set {
				width = value;
			}
		}

		int height;

		public int Height {
			get {
				return height;
			}
			protected set {
				height = value;
			}
		}

		//1f => normal speed, 2f => half speed, nf => 1/n speed, 0f => impassable
		float movemenCost = 1f;

		//if individual structures connect with neighbors to create larger structure
		bool linksToNeighbor = false;

		public bool LinksToNeighbor {
			get {
				return linksToNeighbor;
			}
			protected set {
				linksToNeighbor = value;
			}
		}



		protected Structures() {
		}

		static public Models.Structures CreateStructure(StructureType type, 
			float movementCost = 1f, int width = 1, int height = 1, bool linksToNeighbor = false) {

			Models.Structures structure = new Models.Structures ();

			structure.type = type;
			structure.movemenCost = movementCost;
			structure.width = width;
			structure.height = height;
			structure.linksToNeighbor = linksToNeighbor;

			structure.PositionValidationFunctions = structure.isPositionValidOnSurface;

			return structure;
		}

		public static Models.Structures PlaceStructureOnSurface(Models.Structures structureModel,  Models.Surfaces surfaceModel) {
			if(structureModel.PositionValidationFunctions(surfaceModel) == false) {
				Debug.Log("Models.Structures --> place structure on surface : cannot place the structure here");
				return null;
			}

			Models.Structures structure = new Models.Structures();

			structure.type = structureModel.type;
			structure.movemenCost = structureModel.movemenCost;
			structure.width = structureModel.width;
			structure.height = structureModel.height;
			structure.linksToNeighbor = structureModel.linksToNeighbor;

			structure.SurfaceModel = surfaceModel;

			if (surfaceModel.hasStructure ()) {
				Console.Write ("Models.Structure -> placeStructureOnSurface : surface has pre-existing structure on it");
				return null;
			} else {
				Debug.Log ("Models.Structure -> placeStructureOnSurface : structure added to surface");
				surfaceModel.PlaceStructure (structure);
			}


			int x = structure.SurfaceModel.X;
			int y = structure.SurfaceModel.Y;
			Models.Levels levelModel = Models.Levels.Instance;

			if (structure.linksToNeighbor == true) {

				//North
				if (x < levelModel.Width - 1 && levelModel.GetSurfaceAt (x + 1, y).Structure != null && levelModel.GetSurfaceAt (x + 1, y).Structure.Type == structure.type) {
					levelModel.GetSurfaceAt (x + 1, y).Structure.StructureCallBacks (levelModel.GetSurfaceAt (x + 1, y).Structure);
				}
				//East
				if (y > 0 && levelModel.GetSurfaceAt (x, y - 1).Structure != null && levelModel.GetSurfaceAt (x, y - 1).Structure.Type == structure.type) {
					levelModel.GetSurfaceAt (x, y - 1).Structure.StructureCallBacks (levelModel.GetSurfaceAt (x, y - 1).Structure);
				}
				//South
				if (x > 0 && levelModel.GetSurfaceAt (x - 1, y).Structure != null && levelModel.GetSurfaceAt (x - 1, y).Structure.Type == structure.type) {
					levelModel.GetSurfaceAt (x - 1, y).Structure.StructureCallBacks (levelModel.GetSurfaceAt (x - 1, y).Structure);
				}
				//West
				if (y < levelModel.Height - 1 && levelModel.GetSurfaceAt (x, y + 1).Structure != null && levelModel.GetSurfaceAt (x, y + 1).Structure.Type == structure.type) {
					levelModel.GetSurfaceAt (x, y + 1).Structure.StructureCallBacks (levelModel.GetSurfaceAt (x, y + 1).Structure);
				}
			}

			return structure;

		}

		//Checks if the current position of a structure is valid
		//For structures larger than 1x1, it checks its neighbors as well
		//Function returns true only if all surfaces under consideration don'e have structures on them
		public bool isPositionValidOnSurface(Models.Surfaces surfaceModel) {
			
			for (int x = surfaceModel.X; x < surfaceModel.X + this.width; x++) {
				for (int y = surfaceModel.Y; y < surfaceModel.Y + this.height; y++) {
					Models.Surfaces surface = surfaceModel.Level.GetSurfaceAt (x, y);
					//Return false if the floor is empty
					if (surface.Terrain == Surfaces.TerrainType.Empty) {
						return false;
					}

					//return false if there is already a structure on the surface
					if (surface.hasStructure () == true) {
						return false;
					}
				}
			}

			return true;
		}

		public void RegisterStructureChangesCallBack(Action<Models.Structures> callback) {
			StructureCallBacks += callback;
		}

		public void UnregisterStructureChangesCallBack(Action<Models.Structures> callback) {
			StructureCallBacks -= callback;
		}
	}
}