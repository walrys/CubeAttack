using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawGrid : MonoBehaviour {
    public int gridSize;
    public Material lineMaterial;
    int rows, columns;
    float height;
	// Use this for initialization
	void Start () {
        rows = (int) GetComponent<Renderer>().bounds.size.x / gridSize;
        columns = (int) GetComponent<Renderer>().bounds.size.z / gridSize;
        height = GetComponent<Renderer>().bounds.size.y / 2.0f;
    }


    void OnRenderObject()
    {
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.Begin(GL.LINES);
            GL.Color(lineMaterial.color);
            /* Horizontal lines. */
            for (int i = -rows / 2; i <= rows / 2; i++)
            {
                GL.Vertex3(-columns / 2, height, i);
                GL.Vertex3(columns / 2, height, i);
            }
            /* Vertical lines. */
            for (int i = -columns / 2; i <= columns / 2; i++)
            {
                GL.Vertex3(i, height, -rows/2);
                GL.Vertex3(i, height, rows/2);
            }
        GL.End();
        GL.PopMatrix();
    }
}
