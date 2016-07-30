using UnityEngine;
using System.Collections;

namespace Views {
	public abstract class Items {

		GameObject gameObject;

		Views.Levels level;
		//isometric screen positions?
		int x, y, z;

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
	}
}