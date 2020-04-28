using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CharacterRaycaster : MonoBehaviour
{
    public LayerMask collisionMask;
    public LayerMask triggerMask;

    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;


    public RaycastOrigins raycastOrigins;

    public CollisionInfo collisions;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth * 2;
        

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            RaycastHit2D hitTrigger = Physics2D.Raycast(rayOrigin, Vector2.up * directionX, rayLength, triggerMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }

            if (hitTrigger)
            {
                CollisionReceiver cr = hitTrigger.collider.GetComponent<CollisionReceiver>();

                if(cr != null)
                {
                    cr.OnTriggerEnter?.Invoke();
                }
                
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth * 2;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            RaycastHit2D hitTrigger = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, triggerMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;

               
            }

            if (hitTrigger)
            {
                CollisionReceiver cr = hitTrigger.collider.GetComponent<CollisionReceiver>();

                if (cr != null)
                {
                    cr.OnTriggerEnter?.Invoke();
                }
            }


        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
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

        public bool HaveCollision()
        {
            return above || below || right || left;
        }
    }
}


/*
public struct CharacterCollisionFlags
{
    public bool left, right, above, below;

    public void Reset()
    {
        left = right = above = below = false;
    }
}

public class CharacterRaycaster : MonoBehaviour
{
    public float skinWidthMultiplier = 0.95f;
    public Transform self;
    public BoxCollider2D box;
    public CollisionEmitter collisionEmitter;
    public LayerMask layerMask;

    public CharacterCollisionFlags flags;

    void Start()
    {
        flags.Reset();
    }

    public float CastBoxHorizontal(float distance)
    {
        RaycastHit2D result = Physics2D.BoxCast(
            self.position,
            new Vector2(box.size.x * self.lossyScale.x * skinWidthMultiplier, box.size.y * self.lossyScale.y * skinWidthMultiplier),
            0,
            Vector2.right * Mathf.Sign(distance),
            Mathf.Abs(distance),
            layerMask);

        if (result.collider != null)
        {
            float startPoint = self.position.x + (box.size.x * self.lossyScale.x * 0.5f * Mathf.Sign(distance));
            float newDistance = Mathf.Sign(distance) * Mathf.Abs(result.point.x - startPoint);

            if (distance < 0) flags.left = true;
            if (distance > 0) flags.right = true;

            if (collisionEmitter)
            {
                if (distance < 0)
                    collisionEmitter.OnCollidedLeft.Invoke();

                if (distance > 0)
                    collisionEmitter.OnCollidedRight.Invoke();
            }

            CollisionReceiver cr = result.collider.GetComponent<CollisionReceiver>();
            if (cr != null)
            {
                if (distance < 0)
                    cr.OnCollidedFromRight?.Invoke();

                if (distance > 0)
                    cr.OnCollidedFromLeft?.Invoke();
            }

            return newDistance;
        }

        if (distance < 0) flags.left = false;
        if (distance > 0) flags.right = false;

        return distance;
    }

    public float CastBoxVertical(float distance)
    {
        RaycastHit2D result = Physics2D.BoxCast(
            self.position,
            new Vector2(box.size.x * self.lossyScale.x * skinWidthMultiplier,
                box.size.y * self.lossyScale.y * skinWidthMultiplier),
            0,
            Vector2.up * Mathf.Sign(distance),
            Mathf.Abs(distance),
            layerMask);

        if (result.collider != null)
        {
            float startPoint = self.position.y + (box.size.y * self.lossyScale.y * 0.5f * Mathf.Sign(distance));
            float newDistance = Mathf.Sign(distance) * Mathf.Abs(result.point.y - startPoint);

            if (distance < 0) flags.below = true;
            if (distance > 0) flags.above = true;

            if (collisionEmitter)
            {
                if (distance < 0)
                    collisionEmitter.OnCollidedBelow.Invoke();

                if (distance > 0)
                    collisionEmitter.OnCollidedAbove.Invoke();
            }

            CollisionReceiver cr = result.collider.GetComponent<CollisionReceiver>();
            if (cr != null)
            {
                if (distance < 0)
                    //if (cr.OnCollidedFromRight != null)
                    cr.OnCollidedFromAbove?.Invoke();

                if (distance > 0)
                    //if (cr.OnCollidedFromRight != null)
                    cr.OnCollidedFromBelow?.Invoke();
            }

            return newDistance;
        }

        if (distance < 0) flags.below = false;
        if (distance > 0) flags.above = false;

        return distance;
    }
}
*/
