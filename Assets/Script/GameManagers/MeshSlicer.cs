using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSlicer : MonoBehaviour
{

    public bool isCut;

    private int[] triangles;
    
    private Vector3[] vertices;

    Plane cutPlane;
    Mesh mesh;
    [SerializeField] Material cutMaterial;

    [SerializeField] GameObject plane;
    [SerializeField] GameObject Centroid;
    [SerializeField] GameObject SlicePrefab;
    Collider m_collider;
    Rigidbody rb;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        m_collider = GetComponent<Collider>();
        vertices = mesh.vertices;
        triangles = mesh.triangles;
        rb = GetComponent<Rigidbody>();
    }

    // Dominant side is the one with 2 verts of the cut plane
    List<Vector3> SplitFaceTriangles(List<Vector3> dominantVerts1, List<Vector3> subservientVerts, Plane cutPlane, Vector3 faceNormal)
    {
        Ray ray;
        float distance;

        // Find first cut point of triangle edge
        distance = 0;
        ray = new Ray(subservientVerts[0], dominantVerts1[0] - subservientVerts[0]);
        cutPlane.Raycast(ray, out distance);
        Vector3 intersection1 = ray.GetPoint(distance);

        // First new triangle on dominant side
        dominantVerts1.Add(intersection1);
        dominantVerts1 = FaceDirection(dominantVerts1, faceNormal); // Correct face direction

        // Find second cut point of triangle edge
        distance = 0;
        ray = new Ray(subservientVerts[0], dominantVerts1[1] - subservientVerts[0]);
        cutPlane.Raycast(ray, out distance);
        Vector3 intersection2 = ray.GetPoint(distance);

        // Subservient new triangle
        subservientVerts.Add(intersection1);
        subservientVerts.Add(intersection2);
        subservientVerts = FaceDirection(subservientVerts, faceNormal); // Correct face direction

        // Second new triangle on dominant side
        List<Vector3> dominantVerts2 = new List<Vector3>();
        dominantVerts2.Add(intersection2);
        dominantVerts2.Add(intersection1);
        dominantVerts2.Add(dominantVerts1[1]);
        dominantVerts2 = FaceDirection(dominantVerts2, faceNormal);

        // Collect new triangles. Subservient triangle 0-2, dominant triangle 3-8, intersection vertices 9-10
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
        Vector3 exclusiveVertex1;
        Vector3 exclusiveVertex2;
        Vector3 sharedVertex;
        Ray ray;
        float distance;

        // Find shared vertex
        if (!verts2.Contains(verts1[0]))
        {
            exclusiveVertex1 = verts1[0];
            sharedVertex = verts1[1];
        }
        else
        {
            exclusiveVertex1 = verts1[1];
            sharedVertex = verts1[0];
        }

        if (verts2[0] == sharedVertex)
        {
            exclusiveVertex2 = verts2[1];
        }
        else
        {
            exclusiveVertex2 = verts2[0];
        }

        // Find plane edge intersection
        ray = new Ray(exclusiveVertex2, exclusiveVertex1 - exclusiveVertex2);

        distance = 0;
        cutPlane.Raycast(ray, out distance);
        Vector3 intersection = ray.GetPoint(distance);

        // New triangle 1
        verts1.Add(intersection);
        verts1 = FaceDirection(verts1, faceNormal);

        // New triangle 2
        verts2.Add(intersection);
        verts2 = FaceDirection(verts2, faceNormal);

        // Triangle 1 index: 0-2, Triangle 2 index: 3-5, Intersection index: 6, Shared vertex index: 7
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

        Vector3 cutMidPointLocal = new Vector3();

        List<Vector3> rightVerts = new List<Vector3>();
        List<int> rightTris = new List<int>();

        List<Vector3> leftVerts = new List<Vector3>();
        List<int> leftTris = new List<int>();

        List<Vector3> insideVerts = new List<Vector3>();

        // Set new cutplane
        Vector3 cutNormal = cutNormalList[0];
        cutNormalList.RemoveAt(0);
        cutPlane = new Plane(cutNormal, cutPos);

        // 
        for (int i = 0; i < triangles.Length / 3; i++)
        {
            // Get triangle face normal
            Vector3 initialDirection = Vector3.Cross(vertices[triangles[i * 3 + 1]] - vertices[triangles[i * 3 + 0]],
                                            vertices[triangles[i * 3 + 2]] - vertices[triangles[i * 3 + 0]]);

            initialDirection = transform.TransformDirection(initialDirection);

            List<Vector3> tempLeftVertsWorld1 = new List<Vector3>();
            List<Vector3> tempRightVertsWorld1 = new List<Vector3>();
            Vector3 vertex;

            // Vertex position relative to cut plane
            for (int j = 0; j < 3; j++)
            {
                vertex = transform.TransformPoint(vertices[triangles[i * 3 + j]]);

                if (Mathf.Abs(cutPlane.GetDistanceToPoint(vertex)) < transform.lossyScale.magnitude * 0.001f)
                {
                    tempRightVertsWorld1.Add(vertex);
                    tempLeftVertsWorld1.Add(vertex);
                }
                else if (cutPlane.GetSide(vertex))
                {
                    tempRightVertsWorld1.Add(vertex);
                }
                else
                {
                    tempLeftVertsWorld1.Add(vertex);
                }
            }

            // Split through face. Left side dominant
            if (tempRightVertsWorld1.Count == 1 && tempLeftVertsWorld1.Count == 2)
            {
                //Debug.Log("Split through face (left)");
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

            // Split through face. Right side dominant
            if (tempLeftVertsWorld1.Count == 1 && tempRightVertsWorld1.Count == 2)
            {
                //Debug.Log("Split through face (right)");
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
                //Debug.Log("Split through vertex");
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

            // Split at edge of face. Left side dominant.
            if (tempRightVertsWorld1.Count == 2 && tempLeftVertsWorld1.Count == 3)
            {
                //Debug.Log("Split at edge (left)");
                for (int j = 0; j < 3; j++)
                {
                    leftTris.Add(leftVerts.Count);
                    leftVerts.Add(transform.InverseTransformPoint(tempLeftVertsWorld1[j]));
                }

                // Inside faces (only needed for one side when cut along edge)
                insideVerts.Add(transform.InverseTransformPoint(tempRightVertsWorld1[0]));
                insideVerts.Add(transform.InverseTransformPoint(tempRightVertsWorld1[1]));
            }

            // Cut plane at edge or on vertex of face or no intersection. Right side dominant
            if (tempRightVertsWorld1.Count == 3 && tempLeftVertsWorld1.Count <= 2)
            {
                rightVerts.Add(vertices[triangles[i * 3 + 0]]);
                rightTris.Add(rightVerts.Count - 1);
                rightVerts.Add(vertices[triangles[i * 3 + 1]]);
                rightTris.Add(rightVerts.Count - 1);
                rightVerts.Add(vertices[triangles[i * 3 + 2]]);
                rightTris.Add(rightVerts.Count - 1);
            }

            // Cut plane on vertex of face or no intersection. Left side dominant
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

        // Create inside cut triangles
        if (insideVerts.Count > 0)
        {
            // Create mid point for inside triangles
            for (int k = 0; k < insideVerts.Count; k++)
            {
                cutMidPointLocal += insideVerts[k];
            }
            cutMidPointLocal /= insideVerts.Count;

            // Create triangles
            for (int k = 0; k < insideVerts.Count / 2; k++)
            {
                rightTris.Add(rightVerts.Count + 0);
                rightTris.Add(rightVerts.Count + 1);
                rightTris.Add(rightVerts.Count + 2);
                rightVerts.AddRange(FaceDirection(new List<Vector3>(new Vector3[] { insideVerts[k * 2], insideVerts[k * 2 + 1], cutMidPointLocal }), transform.InverseTransformDirection(-cutPlane.normal)));


                leftTris.Add(leftVerts.Count + 0);
                leftTris.Add(leftVerts.Count + 1);
                leftTris.Add(leftVerts.Count + 2);
                leftVerts.AddRange(FaceDirection(new List<Vector3>(new Vector3[] { insideVerts[k * 2], insideVerts[k * 2 + 1], cutMidPointLocal }), transform.InverseTransformDirection(cutPlane.normal)));
            }

            // Create new mesh right side of cutplane
            if (rightVerts.Count > 0)
            {
                m_collider.enabled = false;
                Mesh rightMesh = new Mesh();

                rightMesh.vertices = rightVerts.ToArray();
                rightMesh.triangles = rightTris.ToArray();
                rightMesh.RecalculateBounds();
                rightMesh.RecalculateNormals();
                rightMesh.RecalculateTangents();

                GameObject rightGO = InstansiateNewSlice(rightMesh, cutPlane.normal);

                Destroy(rightGO, 3);

                MeshSlicer rightSlicer = rightGO.GetComponent<MeshSlicer>();

                // Cut again if multiple cut planes
                if (cutNormalList.Count > 0)
                {
                    rightSlicer.Cut(cutPos, new List<Vector3>(cutNormalList));
                }

            }

            // Create new mesh left side of cutplane
            if (leftVerts.Count > 0)
            {

                Mesh leftMesh = new Mesh();
                leftMesh.vertices = leftVerts.ToArray();
                leftMesh.triangles = leftTris.ToArray();
                leftMesh.RecalculateBounds();
                leftMesh.RecalculateNormals();
                leftMesh.RecalculateTangents();
                GameObject leftGO = InstansiateNewSlice(leftMesh, -cutPlane.normal);
                MeshSlicer leftSlicer = leftGO.GetComponent<MeshSlicer>();

                Destroy(leftGO, 3);

                // Cut again if multiple cut planes
                if (cutNormalList.Count > 0)
                {
                    leftSlicer.Cut(cutPos, new List<Vector3>(cutNormalList));
                }
            }

            Destroy(gameObject);
        }
        else if (cutNormalList.Count > 0) // Cut again if multiple cut planes
        {
            Cut(cutPos, new List<Vector3>(cutNormalList));
        }
    }

    #endregion

    // Assign components and properties to new slice
    GameObject InstansiateNewSlice(Mesh mesh, Vector3 splitForce)
    {
        GameObject go = new GameObject("Slice");
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
        go.transform.localScale = transform.localScale;
        go.AddComponent<MeshFilter>().mesh = mesh;
        go.AddComponent<MeshRenderer>().material = cutMaterial;
        MeshCollider meshCollider = go.AddComponent<MeshCollider>();
        meshCollider.cookingOptions = MeshColliderCookingOptions.InflateConvexMesh;
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;

        Rigidbody slice_rb = go.AddComponent<Rigidbody>();
        slice_rb.mass = 0.3f;
        slice_rb.centerOfMass = go.transform.InverseTransformPoint(meshCollider.bounds.center);
        slice_rb.velocity = rb.velocity;
        slice_rb.angularVelocity = rb.angularVelocity;


        slice_rb.AddForce(splitForce, ForceMode.Impulse);

        MeshSlicer slicer = go.AddComponent<MeshSlicer>();
        slicer.cutMaterial = cutMaterial;
        slicer.isCut = true;
        return go;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Cut(transform.position, new List<Vector3>(new Vector3[] { transform.TransformDirection(1, 0, 0), transform.TransformDirection(0, 1, 0), transform.TransformDirection(0, 0, 1), transform.TransformDirection(0, 1, 1) }));

#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPaused = true;
#endif

        }
    }
}
