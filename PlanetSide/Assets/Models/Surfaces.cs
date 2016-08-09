using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


namespace Models {
	/// <summary>
	/// Models.Surface class tracks data related to a given surface
	/// Models.Level : the parent in which this surface exists
	/// int x, int y : coordinates in the world / level
	/// enum TerrainType : the type of terrain this surface represents (Plain, Lake, Forest, Mountain, etc.)
	/// Models.Structure : (if any) installed on this surface
	/// Models.Resource : (if any) created on this surface
	/// bool drawTerrain : if something covers this surface completely then there is no need to draw it.
	/// </summary>
	public class Surfaces {

		public enum TerrainType { Empty, Lake, Mountain, Plain };


		//FIXME:create protected accessor methods
		Models.Resources resource;
		Models.Structures structure;
		TerrainType terrain;

		public Models.Structures Structure {
			get {
				return structure;
			}
		}

		public TerrainType Terrain {
			get {
				return terrain;
			}
			set {
				//if the terrain changes to the same type then return without doing anything
				if (terrain == value) {
					return;
				}
				terrain = value;
				if (TerrainCallBackMethods != null) {
					//Console.Write ("Callback called from Models.surface");
					TerrainCallBackMethods (this);
				}
			}
		}

		public Action<Models.Surfaces> TerrainCallBackMethods;

		//Surface's Parent element
		Models.Levels level;
		//	World/Level Coordinates for surface 
		// not isometric or screen coordinates, those can be calculated in the view namespace
		int x, y, z;

		public Models.Levels Level {
			get {
				return level;
			}
			protected set {
				level = value;
			}
		}

		public int X {
			get {
				return x;
			}
			protected set {
				x = value;
			}
		}

		public int Y {
			get {
				return y;
			}
			protected set {
				y = value;
			}
		}

		public int Z {
			get {
				return z;
			}
			protected set {
				z = value;
			}
		}

		// By default the terrain can be draw, 
		// only when structures are added that this value becomes false
		// removing the need to even submit it to the buffer
		bool canDrawTerrain = true;

		public bool CanDrawTerrain {
			get {
				return canDrawTerrain;
			}
			set {
				canDrawTerrain = value;
			}
		}

		public Surfaces (Models.Levels level, int x, int y, int z = 0) {
			this.level = level;
			this.x = x;
			this.y = y;
			this.z = z;

			//TODO: assign basic values to terrain-type, structure, and resource
		}

		public bool PlaceStructure(Models.Structures structureModel) {

			if (structureModel == null) {
				//Removing object from surface
				this.structure = null;
				return true;
			}

			if (structureModel.isPositionValidOnSurface (structureModel.SurfaceModel) == false) {
				Debug.Log ("Models.Surfaces --> PlaceStructureOnSurface : structure at " + X + ", " + Y + " already exists");
				return false;
			} else {
				for (int a = X; a < X + structureModel.Width; a++) {
					for (int b = Y; b < Y + structureModel.Height; b++) {
						level.GetSurfaceAt (a, b).structure = structureModel;
					}
				}

				//FIXME: placeholder cause the compiler throws a tantrum
				return true;	
			}
		}

		//Check if the surface already has an object installed on the surface
		// which means check if structre is not null
		public bool hasStructure() {
			return (this.structure != null);
		}


		/// <description>
		/// Register and Unregister terrain call backs using these functions.
		/// RegisterTerrainCallBack<T> (Action<T> callback)
		/// UnregisterTerrainCallBack<T> (Action<T> callback)
		/// </description>
		public void RegisterTerrainCallBack(Action<Models.Surfaces> callback) {
			//Console.Write("Call back registered");
			TerrainCallBackMethods += callback;
		}

		public void UnregisterChangeCallBack(Action<Models.Surfaces> callback) {
			//Console.Write("Call back unregistered");
			TerrainCallBackMethods -= callback;
		}
	}
}