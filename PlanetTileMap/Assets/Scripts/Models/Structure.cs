using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Models {
	public class Structure {

		//FIXME: put in practical building types
		public enum StructureType { A, B, C, D };

		public StructureType Type { get; protected set; }

		//an array in case a structure occupies more than one surface tile
		List<Models.Surface> surfaceModels;

		//Callbacks
		Action<Models.Structure> LevelChangeCallbacks;

		//Dimensions of the structure
		public int length { get; protected set; }
		public int breadth { get; protected set; }
		public int height { get; protected set; }

		//movement cost is the effort required to move through a structure or on a surface
		//1f => normal speed, 2f => 1/2 speed, nf => 1/n speed, 0f => INF or impassable
		public float movementCost { get; protected set; }

		//Indicates how advanced the building is in its hierarchy
		//This will also determine the amount of resources it produces.

		public int level {
			get{ return level; }
			protected set {
				//TODO: determine later if this needs to be changed
				//levels range from 1 to 4
				if (value <= 1)
					value = 1;
				if (value >= 4)
					value = 4;
				
				level = value; 
				if (LevelChangeCallbacks != null) {
					LevelChangeCallbacks (this);
				}
			}
		}

		//For certain types of structures like walls, roads, etc that create a larger structure.
		public bool linksToNeighbors { get; protected set; }

		protected Structure() {}

		/*
		 * structures will need resource inputs, resource outputs, man power, desireability, etc
		 */
		//TODO: for now the height of all structures is assume to be 1 level only
		public static Models.Structure CreatePrototype(StructureType type, 
			int length = 1, int breadth = 1, int height = 1,
			float movementCost = 1f, int level = 1, bool linksToNeighbors = false) {

			Structure structure = new Structure ();

			structure.Type = type;

			structure.length = length;
			structure.breadth = breadth;
			structure.height = height;
			structure.movementCost = movementCost;
			structure.level = level;
			structure.linksToNeighbors = linksToNeighbors;

			return structure;
		}


		/// <summary>
		/// Places the structure on a surface.
		/// </summary>
		/// <returns>structure model</returns>
		/// <param name="structureProto">Structure prototype</param>
		/// <param name="surfaceModel">Surface model</param>
		public Models.Structure PlaceOnSurface(Models.Structure structureProto, Models.Surface surfaceModel) {

			Structure structure = new Structure ();

			structure.Type = structureProto.Type;

			structure.length = structureProto.length;
			structure.breadth = structureProto.breadth;
			structure.height = structureProto.height;
			structure.movementCost = structureProto.movementCost;
			structure.level = structureProto.level;
			structure.linksToNeighbors = structureProto.linksToNeighbors;

			if (surfaceModel.CanPlace (structure) == false) {
				//Cannot place current structure on at least 1 on of the required surfaces
				return null;
			} else {
				structure.surfaceModels = surfaceModel.Place (structure);

				return structure;
			}
		}


		/* Callback registry */
		public void RegisterLevelChangedCallbacks(Action<Models.Structure> callback) {
			LevelChangeCallbacks += callback;
		}

		public void UnregisterLevelChangedCallbacks(Action<Models.Structure> callback) {
			LevelChangeCallbacks -= callback;
		}
	}
}