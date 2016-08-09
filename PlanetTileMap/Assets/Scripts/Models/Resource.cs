using UnityEngine;
using System.Collections;

namespace Models {
	public class Resource {

		public enum ResourceType { A, B, C, D }

		public ResourceType Type { get; protected set; }

		//the amount of resource produced by the world? or will this be per structure/surface?
		public float ProducedAmount { get; protected set; }

		protected Resource() {
		}

		public static Models.Resource CreatePrototype (Models.Resource.ResourceType type) {
			Resource resourcePrototype = new Resource ();

			resourcePrototype.Type = type;
			resourcePrototype.ProducedAmount = 0;

			return resourcePrototype;
		}


	}
}