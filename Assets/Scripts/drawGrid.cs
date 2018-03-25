using UnityEngine;

public class DrawGrid : MonoBehaviour {
    public Material lineMaterial;

    int gridSize = 1;
	int rows, columns;
    float height;

	void Start () {
		rows = (int) GetComponent<Renderer>().bounds.size.x / gridSize;
        columns = (int) GetComponent<Renderer>().bounds.size.z / gridSize;
        height = GetComponent<Renderer>().bounds.size.y / 2.0f;
		DrawLines();
	}

	void DrawLines() {
		GameObject grid = new GameObject("Grid");
		float stroke = 0.01f;
		// Draw horizontal cube lines
		for (int i = -rows / 2; i <= rows / 2; i++) {
			GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
			line.GetComponent<Renderer>().material = lineMaterial;
			line.GetComponent<Collider>().enabled = false;
			line.transform.localScale = new Vector3(stroke + columns, stroke, stroke);
			line.transform.position = new Vector3(0, 0.51f - stroke/2f, i);
			line.transform.SetParent(grid.transform);
		}
		// Draw Vertical cube lines
		for (int i = -columns / 2; i <= columns / 2; i++) {
			GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
			line.GetComponent<Renderer>().material = lineMaterial;
			line.GetComponent<Collider>().enabled = false;
			line.transform.localScale = new Vector3(stroke, stroke, stroke + rows);
			line.transform.position = new Vector3(i, 0.51f - stroke/2f, 0);
			line.transform.SetParent(grid.transform);
		}
	}
}
