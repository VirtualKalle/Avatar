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
    private Collider m_collider;
    [SerializeField] GameObject plane;
    Plane cutPlane;
    [SerializeField] GameObject Centroid;
    [SerializeField] GameObject SlicePrefab;
    [SerializeField] Material cutMaterial;
    public bool isCut;
    Rigidbody rb;

    // Use this for initialization
    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        size = GetComponent<Renderer>().bounds.size;
        m_collider = GetComponent<Collider>();
        vertices = mesh.vertices;
        triangles = mesh.triangles;
        normals = mesh.normals;
        uv = mesh.uv;
        rb = GetComponent<Rigidbody>();
        //Debug.Log("vertices.Length " + vertices.Length);
        //Debug.Log("triangles.Length " + triangles.Length);

        //Cut(plane.transform.position, plane.transform.up);
    }


    List<Vector3> SplitFaceTriangles(List<Vector3> dominantVerts1, List<Vector3> subservientVerts, Plane cutPlane, Vector3 faceNormal)
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

    List<Vector3> SplitVertexTriangles(List<Vector3> verts1, List<Vector3> verts2, Plane cutPlane, Vector3 faceNormal)
    {
        // Right surface 0-2, Left surface 3-5, inside 6-7
        Vector3 excusiveVertex1;
        Vector3 excusiveVertex2;
        Vector3 sharedVertex;
        //RaycastHit hit;
        Ray ray;
        float distance;


        if (!verts2.Contains(verts1[0]))
        {
            excusiveVertex1 = verts1[0];
            sharedVertex = verts1[1];
        }
        else
        {
            excusiveVertex1 = verts1[1];
            sharedVertex = verts1[0];
        }

        if (verts2[0] == sharedVertex)
        {
            excusiveVertex2 = verts2[1];
        }
        else
        {
            excusiveVertex2 = verts2[0];
        }

        ray = new Ray(excusiveVertex2, excusiveVertex1 - excusiveVertex2);

        //New triangle on right side
        distance = 0;
        cutPlane.Raycast(ray, out distance);
        Vector3 intersection = ray.GetPoint(distance);

        verts1.Add(intersection);
        verts2.Add(intersection);

        verts1 = FaceDirection(verts1, faceNormal);
        verts2 = FaceDirection(verts2, faceNormal);

        List<Vector3> splitVerts = new List<Vector3>();
        splitVerts.AddRange(verts2);
        splitVerts.AddRange(verts1);
        splitVerts.Add(intersection);
        splitVerts.Add(sharedVertex);

        return splitVerts;
    }

    List<Vector3> FaceDirection(List<Vector3> face, Vector3 direction)
    {

        Vector3 faceDirection = Vector3.Cross(face[1] - face[0], face[2] - face[0]);
        //Debug.Log(Vector3.Angle(direction, faceDirection));
        if (Vector3.Angle(direction, faceDirection) > 90f)
        {
            face.Reverse();
        }
        return face;
    }

    #region Cut

    public void TryCut(Vector3 cutPos, List<Vector3> cutNormalList)
    {
        if (!isCut)
        {
            isCut = true;
            Cut(cutPos, cutNormalList);
        }
    }

    public void TryCut(Vector3 cutPos, Vector3 cutNormalList)
    {
        if (!isCut)
        {
            isCut = true;
            Cut(cutPos, new List<Vector3>(new Vector3[] { cutNormalList }));
        }
    }

    void Cut(Vector3 cutPos, List<Vector3> cutNormalList)
    {
        Vector3 cutNormal = cutNormalList[0];
        cutNormalList.RemoveAt(0);
        cutPlane = new Plane(cutNormal, cutPos);
        Vector3 cutMidPoint = new Vector3();
        List<Vector3> rightVerts = new List<Vector3>();
        List<int> rightTris = new List<int>();
        List<Vector3> leftVerts = new List<Vector3>();
        List<int> leftTris = new List<int>();

        List<Vector3> insideVerts = new List<Vector3>();
        List<int> insideTris = new List<int>();

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

                if (Mathf.Abs(cutPlane.GetDistanceToPoint(vertex)) < transform.lossyScale.magnitude * 0.001f)
                {
                    Debug.Log("Cut vertex! Distance " + cutPlane.GetDistanceToPoint(vertex));

                    tempRightVertsWorld1.Add(vertex);
                    tempLeftVertsWorld1.Add(vertex);
                }
                else
                if (cutPlane.GetSide(vertex))
                {
                    tempRightVertsWorld1.Add(vertex);
                }
                else
                {
                    tempLeftVertsWorld1.Add(vertex);
                }
            }

            if (tempRightVertsWorld1.Count == 1 && tempLeftVertsWorld1.Count == 2)
            {
                Debug.Log("Split through face (left)");
                List<Vector3> surfVerts = SplitFaceTriangles(tempLeftVertsWorld1, tempRightVertsWorld1, cutPlane, initialDirection);

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

                // Inside faces
                insideVerts.Add(transform.InverseTransformPoint(surfVerts[9]));
                insideVerts.Add(transform.InverseTransformPoint(surfVerts[10]));

            }

            if (tempLeftVertsWorld1.Count == 1 && tempRightVertsWorld1.Count == 2)
            {
                Debug.Log("Split through face (right)");
                List<Vector3> surfVerts = SplitFaceTriangles(tempRightVertsWorld1, tempLeftVertsWorld1, cutPlane, initialDirection);

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

                // Inside faces
                insideVerts.Add(transform.InverseTransformPoint(surfVerts[9]));
                insideVerts.Add(transform.InverseTransformPoint(surfVerts[10]));

            }

            // Split face through vertex
            if (tempRightVertsWorld1.Count == 2 && tempLeftVertsWorld1.Count == 2)
            {
                Debug.Log("Split through vertex");
                List<Vector3> surfVerts = SplitVertexTriangles(tempRightVertsWorld1, tempLeftVertsWorld1, cutPlane, initialDirection);

                for (int k = 0; k < 3; k++)
                {
                    leftTris.Add(leftVerts.Count);
                    leftVerts.Add(transform.InverseTransformPoint(surfVerts[k]));
                }

                for (int k = 3; k < 6; k++)
                {
                    rightTris.Add(rightVerts.Count);
                    rightVerts.Add(transform.InverseTransformPoint(surfVerts[k]));
                }

                // Inside faces
                insideVerts.Add(transform.InverseTransformPoint(surfVerts[6]));
                insideVerts.Add(transform.InverseTransformPoint(surfVerts[7]));

            }

            // Split at edge of face (left)
            if (tempRightVertsWorld1.Count == 2 && tempLeftVertsWorld1.Count == 3)
            {
                Debug.Log("Split at edge (left)");
                for (int j = 0; j < 3; j++)
                {
                    leftTris.Add(leftVerts.Count);
                    leftVerts.Add(transform.InverseTransformPoint(tempLeftVertsWorld1[j]));
                }

                // Inside faces
                insideVerts.Add(transform.InverseTransformPoint(tempRightVertsWorld1[0]));
                insideVerts.Add(transform.InverseTransformPoint(tempRightVertsWorld1[1]));
            }

            if (tempRightVertsWorld1.Count == 3 && tempLeftVertsWorld1.Count <= 2)
            {
                rightVerts.Add(vertices[triangles[i * 3 + 0]]);
                rightTris.Add(rightVerts.Count - 1);
                rightVerts.Add(vertices[triangles[i * 3 + 1]]);
                rightTris.Add(rightVerts.Count - 1);
                rightVerts.Add(vertices[triangles[i * 3 + 2]]);
                rightTris.Add(rightVerts.Count - 1);
            }

            if (tempLeftVertsWorld1.Count == 3 && tempRightVertsWorld1.Count < 2)
            {
                leftVerts.Add(vertices[triangles[i * 3 + 0]]);
                leftTris.Add(leftVerts.Count - 1);
                leftVerts.Add(vertices[triangles[i * 3 + 1]]);
                leftTris.Add(leftVerts.Count - 1);
                leftVerts.Add(vertices[triangles[i * 3 + 2]]);
                leftTris.Add(leftVerts.Count - 1);
            }

        }

        for (int k = 0; k < insideVerts.Count; k++)
        {
            cutMidPoint += insideVerts[k];
        }
        cutMidPoint /= insideVerts.Count;


        for (int k = 0; k < insideVerts.Count / 2; k++)
        {
            rightTris.Add(rightVerts.Count + 0);
            rightTris.Add(rightVerts.Count + 1);
            rightTris.Add(rightVerts.Count + 2);
            rightVerts.AddRange(FaceDirection(new List<Vector3>(new Vector3[] { insideVerts[k * 2], insideVerts[k * 2 + 1], cutMidPoint }), transform.InverseTransformDirection(-cutPlane.normal)));


            leftTris.Add(leftVerts.Count + 0);
            leftTris.Add(leftVerts.Count + 1);
            leftTris.Add(leftVerts.Count + 2);
            leftVerts.AddRange(FaceDirection(new List<Vector3>(new Vector3[] { insideVerts[k * 2], insideVerts[k * 2 + 1], cutMidPoint }), transform.InverseTransformDirection(cutPlane.normal)));
        }

        if (rightVerts.Count > 0)
        {

            m_collider.enabled = false;

            Mesh rightMesh = new Mesh();
            rightMesh.vertices = rightVerts.ToArray();
            rightMesh.triangles = rightTris.ToArray();
            rightMesh.RecalculateBounds();
            rightMesh.RecalculateNormals();
            rightMesh.RecalculateTangents();

            GameObject rightGO = InstansiateNewSlice(rightMesh, cutPos);

            Destroy(rightGO, 3);

            MeshSlicer rightSlicer = rightGO.GetComponent<MeshSlicer>();

            if (cutNormalList.Count > 0)
            {
                Debug.Log("Cutting right again");
                rightSlicer.Cut(cutPos, new List<Vector3>(cutNormalList));
            }

        }

        if (leftVerts.Count > 0)
        {

            Mesh leftMesh = new Mesh();
            leftMesh.vertices = leftVerts.ToArray();
            leftMesh.triangles = leftTris.ToArray();
            leftMesh.RecalculateBounds();
            leftMesh.RecalculateNormals();
            leftMesh.RecalculateTangents();

            GameObject leftGO = InstansiateNewSlice(leftMesh, cutPos);
            MeshSlicer leftSlicer = leftGO.GetComponent<MeshSlicer>();

            Destroy(leftGO, 3);

            if (cutNormalList.Count > 0)
            {
                Debug.Log("Cutting left again");
                leftSlicer.Cut(cutPos, new List<Vector3>(cutNormalList));
            }
        }

        Destroy(gameObject);

    }

    #endregion

    GameObject InstansiateNewSlice(Mesh mesh, Vector3 cutPos)
    {
        GameObject go = new GameObject("Slice");
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
        go.transform.localScale = transform.localScale;
        go.AddComponent<MeshFilter>().mesh = mesh;
        go.AddComponent<MeshRenderer>().material = cutMaterial;
        MeshCollider meshCollider = go.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
        Rigidbody slice_rb = go.AddComponent<Rigidbody>();
        slice_rb.mass = 0.3f;
        slice_rb.velocity = rb.velocity;
        slice_rb.angularVelocity = rb.angularVelocity;
        slice_rb.AddExplosionForce(100, cutPos, 1);

        MeshSlicer slicer = go.AddComponent<MeshSlicer>();
        slicer.cutMaterial = cutMaterial;
        slicer.isCut = true;
        return go;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            List<Vector3> cutPlanes = new List<Vector3>();
            for (int i = 0; i < 3; i++)
            {
                cutPlanes.Add(transform.TransformDirection(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f))));
            }
            Cut(transform.position, new List<Vector3>(new Vector3[] { transform.TransformDirection(1, 0, 0), transform.TransformDirection(0, 1, 0), transform.TransformDirection(0, 0, 1), transform.TransformDirection(0, 1, 1) }));
            Cut(transform.position, cutPlanes);

            //cutPlanes.Add(transform.TransformDirection(1, 0, 0));
            //Cut(transform.position, cutPlanes);
#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPaused = true;
#endif
        }
    }
}
