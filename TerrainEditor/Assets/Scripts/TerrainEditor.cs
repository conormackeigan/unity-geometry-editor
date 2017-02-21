using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEditor : MonoBehaviour
{
    struct Polygon
    {
        public List<Vector2> verts;
        public Vector2 origin;
        public string name;
        

        public Polygon(Vector2 position) : this()
        {
            verts = new List<Vector2>();
            origin = position;
        }

        public void setName(string name)
        {
            this.name = name;
        }
    }


    List<Polygon> Polygons;
    public int selected = -1;

    Camera cam;
    float drag = 0f;
    Vector3 prevMousePos;

    // Inspector parameters
    public float minDrag = 0.5f;

    
    void Start ()
    {      
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        prevMousePos = Input.mousePosition;

        Polygons = new List<Polygon>();
        Polygons.Add(new Polygon(Vector2.zero));
        selected = 0;
	}


    void Update()
    {
        if (Polygons.Count == 0)
            return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        { // force horizontal/vertical
            Vector3 from = Polygons[selected].verts.Count > 0 ? Polygons[selected].verts[Polygons[selected].verts.Count - 1] : Polygons[selected].origin;
            if (Mathf.Abs(from.x - mousePos.x) > Mathf.Abs(from.y - mousePos.y))
            { 
                mousePos.y = from.y; // further horizontal distance, snap to x axis
            }
            else
            { 
                mousePos.x = from.x; // further vertical distance, snap to y axis
            }
        }

        bool addedVertThisFrame = false;

        if (Polygons[selected].verts.Count == 0)
        {
            Debug.DrawLine(Polygons[selected].origin, mousePos, Color.white);
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
            Debug.DrawLine(Polygons[selected].verts[Polygons[selected].verts.Count - 1], mousePos, Color.blue);
            Debug.DrawLine(mousePos, Polygons[selected].verts[0], Color.blue);
        }


        // add verts:
        if (Input.GetMouseButtonDown(0) && !addedVertThisFrame)
        {
            Polygons[selected].verts.Add(mousePos);
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
                    Polygons[selected].verts.Add(mousePos);
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


    void OnDrawGizmos()
    {
        if (selected >= 0)
        {
            foreach (Vector2 vert in Polygons[selected].verts)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(vert, 0.1f);
            }
        }
    }
}
