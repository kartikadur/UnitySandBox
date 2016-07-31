﻿using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Structures. 
/// for this class do I want to build one prototype and then clone it
/// this would allow for multiple calculations for building inputs and outputs to be handled in a single place
/// but that might not be the best idea in the long run. maybe another class may handle all the calculations
/// </summary>
namespace Models {
	public class Structures : Items {

		public enum StructureType { Empty, House }

		//FIXME: create accessor methods to protect this variable
		StructureType type;

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

		Action<Models.Structures> callBackMethods;

		Models.Surfaces surfaceModel;

		//this will indicate if structure occupied just 1 or more than 1 surface on screen.
		int width = 1;
		int height = 1;

		//1f => normal speed, 2f => half speed, nf => 1/n speed, 0f => impassable
		float movemenCost = 1f;

		public Structures(Models.Surfaces surface, StructureType type, 
			float movementCost = 1f, int width = 1, int height = 1) {

			this.surfaceModel = surface;
			this.type = type;
			this.movemenCost = movemenCost;
			this.width = width;
			this.height = height;
		}

		public void RegisterBuildStructureCallBack(Action<Models.Structures> callback) {
			callBackMethods += callback;
		}

		public void UnregisterBuildStructureCallBack(Action<Models.Structures> callback) {
			callBackMethods -= callback;
		}
	}
}