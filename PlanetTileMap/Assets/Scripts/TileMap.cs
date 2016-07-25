using UnityEngine;
using System.Collections;


[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class TileMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		BuildMesh ();
	}

	void BuildMesh() {

		//Generate mesh data
		Vector3[] vertices = new Vector3[4];
		int[] triangles = new int[2 * 3];
		Vector3[] normals = new Vector3[4];

		vertices [0] = new Vector3 (0, 0, 0);
		vertices [1] = new Vector3 (1, 0, 0);
		vertices [2] = new Vector3 (0, 0, -1);
		vertices [3] = new Vector3 (1, 0, -1);

		triangles [0] = 0;
		triangles [1] = 3;
		triangles [2] = 2;

		triangles [3] = 0;
		triangles [4] = 1;
		triangles [5] = 3;

		normals [0] = Vector3.up;
		normals [1] = Vector3.up;
		normals [2] = Vector3.up;
		normals [3] = Vector3.up;

		//Buid the mesh
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;

		//Create and add mesh
		MeshCollider mCollider = GetComponent<MeshCollider>();
		MeshFilter mFilter = GetComponent<MeshFilter>();
		MeshRenderer mRenderer = GetComponent<MeshRenderer>();

		mFilter.mesh = mesh;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
