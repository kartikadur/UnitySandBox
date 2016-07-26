using UnityEngine;
using System.Collections;
using System;

/** TODO
 * resources management
 * structure management
 * passable/impassable
 * movement speed (if passable)
 **/

namespace Models {

	public class Surface {

		/// <summary>
		/// Terrain type descriptions: Current list of terrains, list may change or elements may update
		/// Desert, Forest, Hill, Plain, Tundra, Wetland: low elevation passable land terrain
		/// Mountain: high elevation impassable land terrain
		/// Lake, River: shallow passable water terrain (can use boats or bridges)
		/// Ocean: deep impassable water terrain
		/// list from: http://arcana.wikidot.com/terrain
		/// For now it is using only two types, the others may be used in a later edit
		/// </summary>
		public enum TerrainType {
			Forest,
			Plain,
			Lake
		}

		// Default terrain type
		TerrainType terrain = TerrainType.Plain;

		Resource resourceItem;
		Structure structureItem;

		//reference to world in which surface will be installed
		Level level;

		//location of surface tile
		int x;
		int y;

		//Accessors
		public TerrainType Terrain {
			get {
				return terrain;
			}
			set {
				terrain = value;
				if (SurfaceCallbackMethods != null) {
					SurfaceCallbackMethods (this);
				}
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

		//Delegated callbacks
		Action<Surface> SurfaceCallbackMethods;

		//Methods
		public Surface (Level level, int x, int y) {
			this.level = level;
			this.x = x;
			this.y = y;
		}

		public void RegisterSurfaceCallback(Action<Surface> callback) {
			SurfaceCallbackMethods += callback;
		}

		public void UnregisterSurfaceCallback(Action<Surface> callback) {
			SurfaceCallbackMethods -= callback;
		}
	}
}