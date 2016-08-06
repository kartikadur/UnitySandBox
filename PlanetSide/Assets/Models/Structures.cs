using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Structures. 
/// for this class do I want to build one prototype and then clone it
/// this would allow for multiple calculations for building inputs and outputs to be handled in a single place
/// but that might not be the best idea in the long run. maybe another class may handle all the calculations
/// </summary>
namespace Models {
	public class Structures {

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

		Action<Models.Structures> structureCallBacks;

		Func<Models.Surfaces,bool> positionValidationFunctions;

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
		int height;

		public int Width {
			get {
				return width;
			}
			protected set {
				width = value;
			}
		}

		public int Height {
			get {
				return height;
			}
			protected set {
				height = value;
			}
		}

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

		//1f => normal speed, 2f => half speed, nf => 1/n speed, 0f => impassable
		float movemenCost = 1f;

		protected Structures() {
		}

		static public Models.Structures createStructure(StructureType type, 
			float movementCost = 1f, int width = 1, int height = 1, bool linksToNeighbor = false) {

			Models.Structures structure = new Models.Structures ();

			structure.type = type;
			structure.movemenCost = movementCost;
			structure.width = width;
			structure.height = height;
			structure.linksToNeighbor = linksToNeighbor;

			structure.positionValidationFunctions = structure.isPositionValid;

			return structure;
		}

		public static Models.Structures placeStructureOnSurface(Models.Structures structureModel,  Models.Surfaces surfaceModel) {
			if(structureModel.positionValidationFunctions(surfaceModel) == false) {
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

			if (surfaceModel.hasStructureOnSurface ()) {
				Console.Write ("Models.Structure -> placeStructureOnSurface : surface has pre-existing structure on it");
				return null;
			} else {
				Debug.Log ("Models.Structure -> placeStructureOnSurface : structure added to surface");
				surfaceModel.PlaceStructureOnSurface (structure);
			}


			int x = structure.SurfaceModel.X;
			int y = structure.SurfaceModel.Y;
			Models.Levels levelModel = Models.Levels.Instance;

			if (structure.linksToNeighbor == true) {

				//North
				if (x < levelModel.Width - 1 && levelModel.GetSurfaceAt (x + 1, y).Structure != null && levelModel.GetSurfaceAt (x + 1, y).Structure.Type == structure.type) {
					levelModel.GetSurfaceAt (x + 1, y).Structure.structureCallBacks (levelModel.GetSurfaceAt (x + 1, y).Structure);
				}
				//East
				if (y > 0 && levelModel.GetSurfaceAt (x, y - 1).Structure != null && levelModel.GetSurfaceAt (x, y - 1).Structure.Type == structure.type) {
					levelModel.GetSurfaceAt (x, y - 1).Structure.structureCallBacks (levelModel.GetSurfaceAt (x, y - 1).Structure);
				}
				//South
				if (x > 0 && levelModel.GetSurfaceAt (x - 1, y).Structure != null && levelModel.GetSurfaceAt (x - 1, y).Structure.Type == structure.type) {
					levelModel.GetSurfaceAt (x - 1, y).Structure.structureCallBacks (levelModel.GetSurfaceAt (x - 1, y).Structure);
				}
				//West
				if (y < levelModel.Height - 1 && levelModel.GetSurfaceAt (x, y + 1).Structure != null && levelModel.GetSurfaceAt (x, y + 1).Structure.Type == structure.type) {
					levelModel.GetSurfaceAt (x, y + 1).Structure.structureCallBacks (levelModel.GetSurfaceAt (x, y + 1).Structure);
				}
			}

			return structure;

		}

		//Checks if the current position of a structure is valid
		//For structures larger than 1x1, it checks its neighbors as well
		//Function returns true only if all surfaces under consideration don'e have structures on them
		public bool isPositionValid(Models.Surfaces surfaceModel) {
			
			for (int x = surfaceModel.X; x < surfaceModel.X + this.Width; x++) {
				for (int y = surfaceModel.Y; y < surfaceModel.Y + this.Height; y++) {
					Models.Surfaces surface = surfaceModel.Level.GetSurfaceAt (x, y);
					//Return false if the floor is empty
					if (surface.Terrain == Surfaces.TerrainType.Empty) {
						return false;
					}

					//return false if there is already a structure on the surface
					if (surface.hasStructureOnSurface () == true) {
						return false;
					}
				}
			}

			return true;
		}

		public void RegisterStructureChangesCallBack(Action<Models.Structures> callback) {
			structureCallBacks += callback;
		}

		public void UnregisterStructureChangesCallBack(Action<Models.Structures> callback) {
			structureCallBacks -= callback;
		}
	}
}