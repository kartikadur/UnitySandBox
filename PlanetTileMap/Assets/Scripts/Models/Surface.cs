using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Models {
	public class Surface {

		//FIXME: Put actual terrain types in here
		public enum SurfaceType { Empty, B, C, D };


		Action<Models.Surface> TerrainChangedCallbacks;

		//Type of surface 
		// This will determine what type of interaction can take place on this block
		// whether the user can build, mine, fish, etc on this block
		public SurfaceType type {
			get { 
				return type; 
			}
			protected set { 
				type = value; 
				if (TerrainChangedCallbacks != null) {
					TerrainChangedCallbacks (this);
				}
			}
		}
				

		Models.World worldModel;

		/// <summary>
		/// Gets or sets the structure on the surface.
		/// </summary>
		/// <value>The structure model.</value>
		public Models.Structure StructureModel {
			get {
				return StructureModel;
			}
			set {
				StructureModel = value;
			}
		}

		//Location of surface in the world
		public int x{ get; protected set; }
		public int y{ get; protected set; }
		public int z{ get; protected set; }

		//Multiplier value for resource produced by surface or structure on surface
		public float Fertility {
			get {
				return Fertility;
			}
			protected set {
				Fertility = value;
			}
		}

		public Surface(Models.World worldModel, 
			int x, int y, int z = 0, 
			SurfaceType type = SurfaceType.Empty, float fertility = 0f) {

			this.worldModel = worldModel;

			this.x = x;
			this.y = y;
			this.z = z;

			//Starts of as empty surface with no ability to produce resources.
			this.type = type;
			this.Fertility = fertility;
		}

		/*Methods For Surface*/

		/// <summary>
		/// Determines whether the specified structureModel can placed on this surface 
		/// and neighboring surfaces if structureModel larger than 1x1.
		/// </summary>
		/// <returns><c>true</c> if this instance can place the specified structureModel; 
		/// otherwise, <c>false</c>.</returns>
		/// <param name="structureModel">Structure model.</param>
		public bool CanPlace(Models.Structure structureModel) {

			//get x and y coordinates of surface on which the structure is being placed
			int start_x = this.x;
			int start_y = this.y;
			int end_x = this.x + structureModel.length;
			int end_y = this.y + structureModel.breadth;

			return worldModel.CanPlaceStructure (start_x, start_y, end_x, end_y);
		}

		public List<Models.Surface> Place(Models.Structure structureModel) {
			//get x and y coordinates of surface on which the structure is being placed
			int start_x = this.x;
			int start_y = this.y;
			int end_x = this.x + structureModel.length;
			int end_y = this.y + structureModel.breadth;

			return worldModel.PlaceStructure (structureModel, start_x, start_y, end_x, end_y);
		}


		/* Callback registry */
		public void RegisterTerrainChangedCallbacks(Action<Models.Surface> callback) {
			TerrainChangedCallbacks += callback;
		}

		public void UnregisterTerrainChangedCallbacks(Action<Models.Surface> callback) {
			TerrainChangedCallbacks -= callback;
		}
	}
}