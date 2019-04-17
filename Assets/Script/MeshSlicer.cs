using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSlicer : MonoBehaviour
{
    Mesh mesh;
    private Vector3 size;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector3[] normals;
    private Vector2[] uv;
    [SerializeField] GameObject plane;
    Plane cutPlane;
    [SerializeField] GameObject Centroid;
    [SerializeField] GameObject SlicePrefab;
    [SerializeField] Material cutMaterial;


    // Use this for initialization
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        size = GetComponent<Renderer>().bounds.size;
        vertices = mesh.vertices;
        triangles = mesh.triangles;
        normals = mesh.normals;
        uv = mesh.uv;

        //Debug.Log("vertices.Length " + vertices.Length);
        //Debug.Log("triangles.Length " + triangles.Length);

        //Cut(plane.transform.position, plane.transform.up);
    }


    List<Vector3> DrawSurfaceTriangles(List<Vector3> dominantVerts1, List<Vector3> subservientVerts, Plane cutPlane, Vector3 faceNormal)
    {

        //RaycastHit hit;
        Ray ray;
        float distance;

        //New triangle on right side
        distance = 0;
        ray = new Ray(subservientVerts[0], dominantVerts1[0] - subservientVerts[0]);

        cutPlane.Raycast(ray, out distance);
        Vector3 intersection1 = ray.GetPoint(distance);
        //Debug.Log("intersection1 " + intersection1);

        subservientVerts.Add(intersection1);
        dominantVerts1.Add(intersection1);
        dominantVerts1 = FaceDirection(dominantVerts1, faceNormal);

        distance = 0;
        ray = new Ray(subservientVerts[0], dominantVerts1[1] - subservientVerts[0]);
        cutPlane.Raycast(ray, out distance);
        Vector3 intersection2 = ray.GetPoint(distance);

        subservientVerts.Add(intersection2);

        subservientVerts = FaceDirection(subservientVerts, faceNormal);

        //Second new on left side
        List<Vector3> dominantVerts2 = new List<Vector3>();
        dominantVerts2.Add(intersection2);
        dominantVerts2.Add(intersection1);
        dominantVerts2.Add(dominantVerts1[1]);

        dominantVerts2 = FaceDirection(dominantVerts2, faceNormal);

        List<Vector3> splitVerts = new List<Vector3>();
        splitVerts.AddRange(subservientVerts);
        splitVerts.AddRange(dominantVerts1);
        splitVerts.AddRange(dominantVerts2);
        splitVerts.Add(intersection1);
        splitVerts.Add(intersection2);

        return splitVerts;
    }

    List<Vector3> FaceDirection(List<Vector3> face, Vector3 direction)
    {

        Vector3 faceDirection = Vector3.Cross(face[1] - face[0], face[2] - face[0]);
        Debug.Log(Vector3.Angle(direction, faceDirection));
        if (Vector3.Angle(direction, faceDirection) > 90f)
        {
            face.Reverse();
        }
        return face;
    }

    Vector3 FindCutMidPoint()
    {
        Vector3 midPoint = new Vector3();

        return midPoint;
    }


    public void Cut(Vector3 cutPos, Vector3 cutNormal)
    {
        cutPlane = new Plane(cutNormal, cutPos);
        Vector3 cutMidPoint = cutPlane.ClosestPointOnPlane(transform.position);

        List<Vector3> rightVerts = new List<Vector3>();
        List<int> rightTris = new List<int>();
        List<Vector3> leftVerts = new List<Vector3>();
        List<int> leftTris = new List<int>();


        for (int i = 0; i < triangles.Length / 3; i++)
        {

            Vector3 centroidPos = (vertices[triangles[i * 3 + 0]] + vertices[triangles[i * 3 + 1]] + vertices[triangles[i * 3 + 2]]) / 3;

            Vector3 initialDirection = Vector3.Cross(vertices[triangles[i * 3 + 1]] - vertices[triangles[i * 3 + 0]],
                                            vertices[triangles[i * 3 + 2]] - vertices[triangles[i * 3 + 0]]);

            initialDirection = transform.TransformDirection(initialDirection);

            List<Vector3> tempLeftVertsWorld1 = new List<Vector3>();
            List<Vector3> tempRightVertsWorld1 = new List<Vector3>();
            Vector3 vertex;

            for (int j = 0; j < 3; j++)
            {
                vertex = transform.TransformPoint(vertices[triangles[i * 3 + j]]);
                if (cutPlane.GetSide(vertex))
                {
                    tempRightVertsWorld1.Add(vertex);
                }
                else
                {
                    tempLeftVertsWorld1.Add(vertex);
                }
            }

            if (tempRightVertsWorld1.Count == 1)
            {
                List<Vector3> surfVerts = DrawSurfaceTriangles(tempLeftVertsWorld1, tempRightVertsWorld1, cutPlane, initialDirection);

                for (int k = 0; k < 3; k++)
                {
                    rightTris.Add(rightVerts.Count);
                    rightVerts.Add(transform.InverseTransformPoint(surfVerts[k]));

                }

                for (int k = 3; k < 9; k++)
                {
                    leftTris.Add(leftVerts.Count);
                    leftVerts.Add(transform.InverseTransformPoint(surfVerts[k]));
                }
                
                List<Vector3> rightInside = FaceDirection(new List<Vector3>(new Vector3[] { cutMidPoint, surfVerts[9], surfVerts[10] }), -cutNormal);
                List<Vector3> leftInside = FaceDirection(new List<Vector3>(new Vector3[] { cutMidPoint, surfVerts[9], surfVerts[10] }), cutNormal);

                for (int k = 0; k < 3; k++)
                {
                    rightTris.Add(rightVerts.Count);
                    rightVerts.Add(transform.InverseTransformPoint(rightInside[k]));
                    leftTris.Add(leftVerts.Count);
                    leftVerts.Add(transform.InverseTransformPoint(leftInside[k]));
                }

            }

            else if (tempLeftVertsWorld1.Count == 1)
            {
                List<Vector3> surfVerts = DrawSurfaceTriangles(tempRightVertsWorld1, tempLeftVertsWorld1, cutPlane, initialDirection);

                for (int k = 0; k < 3; k++)
                {
                    leftTris.Add(leftVerts.Count);
                    leftVerts.Add(transform.InverseTransformPoint(surfVerts[k]));
                }

                for (int k = 3; k < 9; k++)
                {
                    rightTris.Add(rightVerts.Count);
                    rightVerts.Add(transform.InverseTransformPoint(surfVerts[k]));
                }

                List<Vector3> rightInside = FaceDirection(new List<Vector3>(new Vector3[] { cutMidPoint, surfVerts[9], surfVerts[10] }), -cutNormal);
                List<Vector3> leftInside = FaceDirection(new List<Vector3>(new Vector3[] { cutMidPoint, surfVerts[9], surfVerts[10] }), cutNormal);

                for (int k = 0; k < 3; k++)
                {
                    rightTris.Add(rightVerts.Count);
                    rightVerts.Add(transform.InverseTransformPoint(rightInside[k]));
                    leftTris.Add(leftVerts.Count);
                    leftVerts.Add(transform.InverseTransformPoint(leftInside[k]));
                }

            }
            else if (tempRightVertsWorld1.Count == 3)
            {
                rightVerts.Add(vertices[triangles[i * 3 + 0]]);
                rightTris.Add(rightVerts.Count - 1);
                rightVerts.Add(vertices[triangles[i * 3 + 1]]);
                rightTris.Add(rightVerts.Count - 1);
                rightVerts.Add(vertices[triangles[i * 3 + 2]]);
                rightTris.Add(rightVerts.Count - 1);
            }
            else if (tempLeftVertsWorld1.Count == 3)

            {
                leftVerts.Add(vertices[triangles[i * 3 + 0]]);
                leftTris.Add(leftVerts.Count - 1);
                leftVerts.Add(vertices[triangles[i * 3 + 1]]);
                leftTris.Add(leftVerts.Count - 1);
                leftVerts.Add(vertices[triangles[i * 3 + 2]]);
                leftTris.Add(leftVerts.Count - 1);
            }






        }

        Mesh rightMesh = new Mesh();
        rightMesh.vertices = rightVerts.ToArray();
        rightMesh.triangles = rightTris.ToArray();
        rightMesh.RecalculateBounds();
        rightMesh.RecalculateNormals();
        rightMesh.RecalculateTangents();

        GameObject rightGO = Instantiate(SlicePrefab, transform.TransformPoint(0, 0, 0), transform.rotation);
        rightGO.transform.localScale = transform.localScale;

        rightGO.GetComponent<MeshFilter>().mesh = rightMesh;
        rightGO.GetComponent<MeshRenderer>().material = cutMaterial;
        rightGO.GetComponent<MeshCollider>().sharedMesh = rightMesh;

        Mesh leftMesh = new Mesh();
        leftMesh.vertices = leftVerts.ToArray();
        leftMesh.triangles = leftTris.ToArray();
        leftMesh.RecalculateBounds();
        leftMesh.RecalculateNormals();
        leftMesh.RecalculateTangents();

        GameObject leftGO = Instantiate(SlicePrefab, transform.TransformPoint(0, 0, 0), transform.rotation);
        leftGO.transform.localScale = transform.localScale;
        leftGO.GetComponent<MeshFilter>().mesh = leftMesh;
        leftGO.GetComponent<MeshRenderer>().material = cutMaterial;
        leftGO.GetComponent<MeshCollider>().sharedMesh = leftMesh;

        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Cut(plane.transform.position, plane.transform.up);
        }
    }
}
