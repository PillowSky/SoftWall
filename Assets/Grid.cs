using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {
	
	public int xSize, ySize;
	
	private Vector3[] vertices;
	private Mesh mesh;
	
	private RaycastHit target;
	private Vector3 targetV;
	private bool active;
	private int selected;
	
	private void Awake () {
		Generate();
	}
	
	public void Start () {
		
	}
	
	public void Update () {
		if (active) {
			if (Input.GetMouseButtonUp(0)) {
				active = false;
			} else {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				float step = - ray.origin[2] / ray.direction[2];
				Vector3 current = new Vector3 (
					ray.origin[0] + ray.direction[0] * step,
					ray.origin[1] + ray.direction[1] * step,
					ray.origin[2] + ray.direction[2] * step
					);
				//Debug.Log(current);
				target.transform.position = current;
				vertices[selected] = current;
				mesh.vertices = vertices;
			}
		} else {
			if (Input.GetMouseButton(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit)){
					target = hit;
					active = true;
					Vector3 pos = hit.transform.position;
					for (int i = 0; i < vertices.Length; i++) {
						if (pos.Equals(vertices[i])) {
							selected = i;
							break;
						}
					}
				}
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
		mesh.RecalculateNormals();
		
		Vector3 sphereScale = new Vector3(0.25f, 0.25f, 0.25f);
		for (int i = 0, y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++, i++) {
				GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				sphere.transform.position = vertices[i];
				sphere.transform.localScale = sphereScale;
			}
		}
	}
	
}
