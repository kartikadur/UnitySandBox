using UnityEngine;
using System.Collections;

namespace Views {
	public abstract class Items {

		GameObject gameObject;

		Views.Levels level;
		//isometric screen positions?
		float x, y, z;

		public float X {
			get {
				return x;
			}
			protected set {
				x = value;
			}
		}

		public float Y {
			get {
				return y;
			}
			protected set {
				y = value;
			}
		}

		public float Z {
			get {
				return z;
			}
			protected set {
				z = value;
			}
		}
	}
}