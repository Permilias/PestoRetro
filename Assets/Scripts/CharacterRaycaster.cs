using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CollisionFlags
{
    public bool left, right, up, down;

    public void Reset()
    {
        left = right = up = down = false;
    }
}

public class CharacterRaycaster : MonoBehaviour
{
    public Transform self;
    public float skinWidthMultiplier = 0.95f;
    public BoxCollider2D box;
    public CollisionEmitter collisionEmitter;
    public LayerMask layerMask;

    public CollisionFlags collisionFlags;

    void Start()
    {
        collisionFlags.Reset();
    }

    public float CastBoxHorizontal(float distance)
    {
        RaycastHit2D result = Physics2D.BoxCast(self.position, new Vector2(box.size.x * self.lossyScale.x * skinWidthMultiplier, box.size.y * self.lossyScale.y * skinWidthMultiplier),
            0,
            Vector2.right * Mathf.Sign(distance),
            Mathf.Abs(distance),
            layerMask);

        if(result.collider != null)
        {
            float startPoint = self.position.x + (box.size.x * self.lossyScale.x * 0.5f * Mathf.Sign(distance));
            float newDistance = Mathf.Sign(distance) * Mathf.Abs(result.point.x - startPoint);

            if (distance < 0) collisionFlags.left = true;
            if (distance > 0) collisionFlags.right = true;

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

        if (distance < 0) collisionFlags.left = false;
        if (distance > 0) collisionFlags.right = false;

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

            if (distance < 0) collisionFlags.down = true;
            if (distance > 0) collisionFlags.up = true;

            if (collisionEmitter)
            {
                if (distance < 0)
                    collisionEmitter.OnCollidedDown.Invoke();

                if (distance > 0)
                    collisionEmitter.OnCollidedUp.Invoke();

            }

            CollisionReceiver cr = result.collider.GetComponent<CollisionReceiver>();

            if (cr != null)
            {
                if (distance < 0)
                    cr.OnCollidedFromUp?.Invoke();


                if (distance > 0)
                    cr.OnCollidedFromDown?.Invoke();
            }

            return newDistance;
        }

        if (distance < 0) collisionFlags.down = false;
        if (distance > 0) collisionFlags.up = false;

        return distance;
    }
}
