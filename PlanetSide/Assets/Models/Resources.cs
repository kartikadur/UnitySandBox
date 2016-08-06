using UnityEngine;
using System;
using System.Collections;

namespace Models {
	public class Resources {

		//TODO: Create list of resources and a summary block for the same
		//e.g. values --> wheat, labor, water, etc.
		public enum ResourceType {

		}


		Action<Models.Resources> callBackMethods;

		Models.Levels level;
		int x, y, z;
		ResourceType type;

		public Resources(Models.Levels level, ResourceType type, int x, int y, int z = 0) {
			this.level = level;
			this.x = x;
			this.y = y;
			this.z = z;
			this.type = type;
		}

		public void RegisterCallBack(Action<Models.Resources> callback) {
			callBackMethods += callback;
		}

		public void UnregisterCallBack(Action<Models.Resources> callback) {
			callBackMethods -= callback;
		}
	}
}
