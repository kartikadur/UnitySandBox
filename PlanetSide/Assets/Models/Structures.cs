using UnityEngine;
using System;
using System.Collections;

namespace Models {
	public class Structures : Items {

		public enum StructureType {

		}

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

		Models.Levels level;
		int x, y;

		public Structures(Models.Levels level, int x, int y, StructureType type) {
			this.level = level;
			this.x = x;
			this.y = y;
			this.type = type;
		}

		public void RegisterCallBack(Action<Models.Structures> callback) {
			callBackMethods += callback;
		}

		public void UnregisterCallBack(Action<Models.Structures> callback) {
			callBackMethods -= callback;
		}
	}
}