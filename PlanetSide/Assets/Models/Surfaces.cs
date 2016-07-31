using UnityEngine;
using System.Collections;
using System;


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
	public class Surfaces : Items {

		public enum TerrainType { Forest, Lake, Mountain, Plain };

		//FIXME:create protected accessor methods
		Models.Resources resource;
		Models.Structures structure;
		TerrainType terrain = TerrainType.Plain;

		public TerrainType Terrain {
			get {
				return terrain;
			}
			set {
				terrain = value;
				if (terrainCallBackMethods != null) {
					//Debug.Log ("Callback called from Models.surface");
					terrainCallBackMethods (this);
				}
			}
		}

		public Action<Models.Surfaces> terrainCallBackMethods;

		//Surface's Parent element
		Models.Levels level;
		//	World/Level Coordinates for surface 
		// not isometric or screen coordinates, those can be calculated in the view namespace
		int x, y, z;

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

		public Surfaces (Models.Levels level, int x, int y) {
			this.level = level;
			this.x = x;
			this.y = y;
			this.z = z;

			//TODO: assign basic values to terrain-type, structure, and resource
		}


		/* 
		 * TODO:
		 * addStructure : adds a structure/building to the surface (also sets canDrawTerrain to false)
		 * remStructure : removes a structure/building from surface (aso sets canDrawTerrain to true)
		 * addResource: Adds a resource to surface
		 * remResource: removes (simuates depletion of) resource from surface
		 */

		public void RegisterTerrainChangeCallBack(Action<Models.Surfaces> callback) {
			//Debug.Log("Call back registered");
			terrainCallBackMethods += callback;
		}

		public void UnregisterTerrainChangeCallBack(Action<Models.Surfaces> callback) {
			//Debug.Log("Call back unregistered");
			terrainCallBackMethods -= callback;
		}

	}
}