using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditor : MonoBehaviour
{
    struct Polygon
    {
        public List<Vector2> verts;
        public Vector2 translation;
        public string name;

        public Polygon(Vector2 position) : this()
        {
            verts = new List<Vector2>();
            translation = position;
        }

        public void setName(string name)
        {
            this.name = name;
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
        if (Polygons.Count == 0)
            return;

        bool addedVertThisFrame = false;

        if (Polygons[selected].verts.Count == 0)
        {
            Debug.DrawLine(Vector3.zero, cam.ScreenToWorldPoint(Input.mousePosition), Color.white);
        }
        else
        {
            // draw current polygon in green
            int v = 0;
            while (v < Polygons[selected].verts.Count)
            {
                Debug.DrawLine(Polygons[selected].verts[v], Polygons[selected].verts[++v % Polygons[selected].verts.Count], Color.green);
            }

            // draw next polygon in blue
            Debug.DrawLine(Polygons[selected].verts[Polygons[selected].verts.Count - 1], cam.ScreenToWorldPoint(Input.mousePosition), Color.blue);
            Debug.DrawLine(cam.ScreenToWorldPoint(Input.mousePosition), Polygons[selected].verts[0], Color.blue);
        }

        // add verts:
        if (Input.GetMouseButtonDown(0) && !addedVertThisFrame)
        {
            Polygons[selected].verts.Add(cam.ScreenToWorldPoint(Input.mousePosition));
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
                    Polygons[selected].verts.Add(cam.ScreenToWorldPoint(Input.mousePosition));
                    drag = 0f;
                }
             
                addedVertThisFrame = true;
            }
        }
        else
        {
            drag = 0f;
        }

        prevMousePos = Input.mousePosition;
    }


    public void Export()
    {
        foreach (Polygon poly in Polygons)
        {
            GameObject Terrain = new GameObject(poly.name);

            int v = 0;
            while (v < poly.verts.Count)
            {
                EdgeCollider2D edge = Terrain.AddComponent<EdgeCollider2D>();
                Vector2[] newPoints = { poly.verts[v], poly.verts[++v % poly.verts.Count] };
                edge.points = newPoints;
            }
        }
    }
}
