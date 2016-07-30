using UnityEngine;
using System;
using System.Collections;

namespace Models {
	public abstract class Items {

		Models.Levels level;

		public Models.Levels Level {
			get {
				return level;
			}
		}

		int x, y;

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

		//NOTE: <T> (generic parameter has to be present next to function/method name as well as ACTION call
		//public abstract void RegisterCallBack (Action<T> callback) ;
		//public abstract void UnregisterCallBack (Action<T> callback);
	}
}