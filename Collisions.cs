using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collisions
{
    public static bool IntersectPolygons(PolygonCollider2D polygonA, PolygonCollider2D polygonB, out Vector3 normal, out float depth)
    {
        normal = Vector3.zero;
        depth = float.MaxValue;

        Vector2[] verticesA = polygonA.points;
        Vector2[] verticesB = polygonB.points;

        LocalToWorld(polygonA.transform, verticesA);
        LocalToWorld(polygonB.transform, verticesB);

        for (int i = 0; i < verticesA.Length; i++)
        {
            Vector2 va = verticesA[i];
            Vector2 vb = verticesA[(i + 1) % verticesA.Length];

            Vector2 edge = vb - va;
            Vector2 axis = new Vector2(edge.y, -edge.x);
            axis = Vector3.Normalize(axis);

            Collisions.ProjectVertices(verticesA, axis, out float minA, out float maxA);
            Collisions.ProjectVertices(verticesB, axis, out float minB, out float maxB);
            
            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            float axisDepth = Mathf.Min(maxB - minA, maxA - minB);
            if (axisDepth == (maxB - minA)) { axis = -axis; }

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        for (int i = 0; i < verticesB.Length; i++)
        {
            Vector2 va = verticesB[i];
            Vector2 vb = verticesB[(i + 1) % verticesB.Length];

            Vector2 edge = vb - va;
            Vector2 axis = new Vector2(edge.y, -edge.x);
            axis = Vector3.Normalize(axis);

            Collisions.ProjectVertices(verticesA, axis, out float minA, out float maxA);
            Collisions.ProjectVertices(verticesB, axis, out float minB, out float maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            float axisDepth = Mathf.Min(maxB - minA, maxA - minB);
            if (axisDepth == (maxB - minA)) { axis = -axis; }

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        depth /= Vector3.Magnitude(normal);

        return true;
    }

    private static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
    {
        min = float.MaxValue;
        max = float.MinValue;
        
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 v = vertices[i];
            float proj = Vector2.Dot(v, axis);

            if (proj < min) { min = proj; }
            if (proj > max) { max = proj; }
        }
    }

    private static void LocalToWorld(Transform transform, Vector2[] polygonVector)
    {
        for (int i = 0; i < polygonVector.Length; i++)
        {
            polygonVector[i] = transform.TransformPoint(polygonVector[i]);
        }
    }
}
