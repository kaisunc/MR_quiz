using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;
using System.Collections;

public class Manager : MonoBehaviour
{
    public static Manager Instanse { get; private set; }
    public static GameObject SelectedObject;
    private List<Box> boxes = new List<Box>();
    private List<GameObject> uniGO;
    private List<GameObject> subGO;
    private GameObject composite;

    void Awake()
    {
        if (Instanse == null)
        {
            Instanse = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            Show();
        }
    }

    void Start()
    {
        DoOp();
    }

    public void DoOp()
    {
        Model result;
        Model uniComp = CSG.Union(uniGO[0], uniGO[1]);
        Model subComp = CSG.Union(subGO[0], subGO[1]);
        result = CSG.Subtract(uniComp, subComp); // modified subtract method to use models instead of gameobject
        composite.GetComponent<MeshFilter>().sharedMesh = result.mesh;
        composite.transform.position = Vector3.zero;
        GenerateBarycentric(composite);
    }

    public void Select(Box box)
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
            boxes[i]._selected = false;
        }
        box._selected = true;
    }

    public void UnSelect(Box box)
    {
        box.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        box._selected = false;
    }

    public void Show()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            StartCoroutine(boxes[i].Fade());
        }
    }

    private void GenerateBarycentric(GameObject go)
    {
        Mesh m = go.GetComponent<MeshFilter>().sharedMesh;

        if (m == null) return;

        int[] tris = m.triangles;
        int triangleCount = tris.Length;

        Vector3[] mesh_vertices = m.vertices;
        Vector3[] mesh_normals = m.normals;
        Vector2[] mesh_uv = m.uv;

        Vector3[] vertices = new Vector3[triangleCount];
        Vector3[] normals = new Vector3[triangleCount];
        Vector2[] uv = new Vector2[triangleCount];
        Color[] colors = new Color[triangleCount];

        for (int i = 0; i < triangleCount; i++)
        {
            vertices[i] = mesh_vertices[tris[i]];
            normals[i] = mesh_normals[tris[i]];
            uv[i] = mesh_uv[tris[i]];

            colors[i] = i % 3 == 0 ? new Color(1, 0, 0, 0) : (i % 3) == 1 ? new Color(0, 1, 0, 0) : new Color(0, 0, 1, 0);

            tris[i] = i;
        }

        Mesh wireframeMesh = new Mesh();

        wireframeMesh.Clear();
        wireframeMesh.vertices = vertices;
        wireframeMesh.triangles = tris;
        wireframeMesh.normals = normals;
        wireframeMesh.colors = colors;
        wireframeMesh.uv = uv;

        go.GetComponent<MeshFilter>().sharedMesh = wireframeMesh;
    }
}
