using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : MonoBehaviour
{
    public CollisionInfo collisions;

    public void Move(Vector3 velocity, PolygonCollider2D colliderA, PolygonCollider2D colliderB)
    {
        collisions.Reset();

        if (Collisions.IntersectPolygons(colliderA, colliderB, out Vector3 normal, out float depth))
        {
            float directionY = Mathf.Sign(velocity.y);
            if (normal.y < 0) { collisions.below = directionY == -1; }
            velocity += normal * depth;

        }
        transform.Translate(velocity);
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}


