using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Models {
	/// <summary>
	/// Level.
	/// Contains data for the current level being played by the user
	/// 
	/// Data contained
	/// Resources (List)
	/// Structures (List)
	/// Terrain (list)
	/// 
	/// What else?
	/// </summary>
	public class World {

		/* Vars and Accessors */
		//Incase I need to name the level?
		public string name { get; protected set; }

		// length => x units (left and right of surface)
		public int width { get; protected set; }
		//breadth => y units (top and bottom of surface)
		public int height { get; protected set; }
		//height => z units (up and down from surface)
		public int level { get; protected set; }

		Models.Surface[] surfaceModels;


		Dictionary <Models.Structure.StructureType, Models.Structure> structureTypeModelMap;


		/*Constructors*/
		public World(string name, int width, int height, int level = 1) {
			this.name = name;

			//TODO: put a check to see that a world that can support one or more surfaces is created
			this.width = width;
			this.height = height;
			this.level = height;

			this.surfaceModels = new Models.Surface[width * height * level];
		}

		/*Class Methods*/

		/// <summary>
		/// Creates the level.
		/// </summary>
		public void CreateLevel() {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					surfaceModels [x * width + y] = new Models.Surface (this, x, y);
				}
			}
		}

		/// <summary>
		/// Gets the surface at x and y coordinates
		/// </summary>
		/// <returns>The <see cref="Models.Surface"/>.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Models.Surface GetSurfaceAt(int x, int y) {
			return surfaceModels [x * width + y];
		}


		/// <summary>
		/// Determines whether this instance can place a structure between specified start_x start_y and end_x end_y.
		/// </summary>
		/// <returns><c>true</c> if this instance can place structure the specified start_x start_y end_x end_y; otherwise, <c>false</c>.</returns>
		/// <param name="start_x">Start x.</param>
		/// <param name="start_y">Start y.</param>
		/// <param name="end_x">End x.</param>
		/// <param name="end_y">End y.</param>
		public bool CanPlaceStructure(int start_x, int start_y, int end_x, int end_y) {
			for (int x = start_x; x < end_x; x++) {
				for (int y = start_y; y < end_y; y++) {
					//return false if terrain is unbuildable
					if (this.GetSurfaceAt (x, y).type == Surface.SurfaceType.Empty) {
						return false;
					}

					//return false if surface already has structure
					if (this.GetSurfaceAt (x, y).StructureModel != null) {
						return false;
					}
				}				
			}
			return true;
		}

		public List<Models.Surface> PlaceStructure(Models.Structure structureModel, int start_x, int start_y, int end_x, int end_y){
			List<Models.Surface> surfaces = new List<Models.Surface> ();
			for (int x = start_x; x < end_x; x++) {
				for (int y = start_y; y < end_y; y++) {
					Models.Surface surface = this.GetSurfaceAt (x, y);
					surface.StructureModel = structureModel;
					surfaces.Add (surface);
				}
			}
			return surfaces;
		}

		/// <summary>
		/// Instantiates the structures.
		/// </summary>
		public void InstantiateStructures() {

			structureTypeModelMap = new Dictionary<Structure.StructureType, Structure> ();

			foreach (Models.Structure.StructureType type in Enum.GetValues(typeof(Models.Structure.StructureType))) {
				structureTypeModelMap[type] = Models.Structure.CreatePrototype (type, 1, 1);
			}
		}


		/*Callbacks Register and Unregister*/

	}
}