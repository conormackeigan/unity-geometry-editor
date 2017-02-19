using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditor : MonoBehaviour
{
    struct Polygon
    {
        public List<Vector2> vertices;
        public Vector2 origin;
        public string name;

        public Polygon(Vector2 position) : this()
        {
            vertices = new List<Vector2>();
            origin = position;
        }
    }

    List<Polygon> Polygons;
    int selected;

    Camera cam;
    float drag = 0f;
    Vector3 prevMousePos;

    // Inspector parameters
    public float minDrag = 0.5f;

    
    // Use this for initialization
    void Start ()
    {      
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        prevMousePos = Input.mousePosition;

        Polygons = new List<Polygon>();
        Polygons.Add(new Polygon(Vector2.zero));
        selected = 0;
	}


    // Update is called once per frame
    void Update()
    {
        bool addedVertThisFrame = false;

        if (Polygons[selected].vertices.Count == 0)
        {
            Debug.DrawLine(Vector3.zero, cam.ScreenToWorldPoint(Input.mousePosition), Color.green);
        }
        else
        {
            // draw current polygon in green
            int v = 0;
            while (v < Polygons[selected].vertices.Count - 1)
            {
                Debug.DrawLine(Polygons[selected].vertices[v], Polygons[selected].vertices[++v], Color.green);
            }

            Debug.DrawLine(Polygons[selected].vertices[Polygons[selected].vertices.Count - 1], Polygons[selected].vertices[0], Color.green);

            // draw next polygon in blue
            Debug.DrawLine(Polygons[selected].vertices[Polygons[selected].vertices.Count - 1], cam.ScreenToWorldPoint(Input.mousePosition), Color.blue);
            Debug.DrawLine(cam.ScreenToWorldPoint(Input.mousePosition), Polygons[selected].vertices[0], Color.blue);
        }

        if (Input.GetMouseButtonDown(0) && !addedVertThisFrame)
        {
            Polygons[selected].vertices.Add(cam.ScreenToWorldPoint(Input.mousePosition));
            addedVertThisFrame = true;
        }

        // drag
        if (Input.GetMouseButton(0))
        {
            if (!addedVertThisFrame)
            {
                drag += (Input.mousePosition - prevMousePos).magnitude;

                if (drag > minDrag)
                {
                    Polygons[selected].vertices.Add(cam.ScreenToWorldPoint(Input.mousePosition));
                    drag = 0f;
                }

                prevMousePos = Input.mousePosition;
                addedVertThisFrame = true;
            }
        }
        else
        {
            drag = 0f;
        }
	}


    public void Export()
    {
        // debug
        foreach (Vector2 vec in Polygons[selected].vertices)
        {
            Debug.Log(vec.ToString());
        }

        foreach (Polygon poly in Polygons)
        {
            GameObject Terrain = new GameObject(poly.name);

            int v = 0;
            while (v < Polygons[selected].vertices.Count)
            {
                EdgeCollider2D edge = Terrain.AddComponent<EdgeCollider2D>();
                Vector2[] newPoints = new Vector2[2];
                newPoints[0] = Polygons[selected].vertices[v];
                v++;
                newPoints[1] = Polygons[selected].vertices[v % Polygons[selected].vertices.Count];
                edge.points = newPoints;
            }

            EdgeCollider2D finalEdge = Terrain.AddComponent<EdgeCollider2D>();
            Vector2[] finalPoints = new Vector2[2];
            finalPoints[0] = Polygons[selected].vertices[Polygons[selected].vertices.Count - 1];
            finalPoints[1] = Polygons[selected].vertices[0];
            finalEdge.points = finalPoints;
        }
    }
}
