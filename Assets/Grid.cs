using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {
	
	public int xSize, ySize;
	
	private Vector3[] vertices;
	private Mesh mesh;
	
	private Vector3 src;
	
	private void Awake () {
		Generate();
	}
	
	public void Start () {
		
	}
	
	public void Update () {
		if (Input.GetMouseButtonDown(0)) {
			// begin drag
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			src = ray.origin + ray.direction * (- ray.origin.z / ray.direction.z);
		} else {
			//render stage
			if (Input.GetMouseButton(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Vector3 dest = ray.origin + ray.direction * (- ray.origin.z / ray.direction.z);
				Vector3 diff = dest - src;

				for (int i = 0; i < vertices.Length; i++) {
					vertices[i] += diff / (1 + (src - vertices[i]).sqrMagnitude); 
				}
				mesh.vertices = vertices;

				src = dest;
			}
		}
	}
	
	private void Generate () {
		mesh = GetComponent<MeshFilter>().mesh;
		mesh.name = "Procedural Grid";
		
		vertices = new Vector3[(xSize + 1) * (ySize + 1)];
		
		for (int i = 0, y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++, i++) {
				vertices[i] = new Vector3(x, y);
			}
		}
		mesh.vertices = vertices;
		
		int[] triangles = new int[xSize * ySize * 6];
		for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
			for (int x = 0; x < xSize; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
				triangles[ti + 5] = vi + xSize + 2;
			}
		}
		mesh.triangles = triangles;
		mesh.RecalculateNormals();;
	}
	
}
