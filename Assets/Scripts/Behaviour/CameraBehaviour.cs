using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraBehaviour : MonoBehaviour
{

    public static Transform PlayerTransform   { get {   return PlayerController._instance.self; }   }
    public float maxX = 1;
    Vector3 targetPosition;
    float x;
    public float tweenSpeed;
    public Vector3 offset;

    void Start()
    {

        GetPosition();
        transform.position = targetPosition;
        x = 0;
    }

    void Update()
    {
        if (PlayerController._instance.movementVector.x > 0)
        {
            x = maxX;
        }

        else if (PlayerController._instance.movementVector.x < 0)
        {
            x = -maxX;
        }

        GetPosition();
        transform.DOMove(targetPosition, tweenSpeed);
    }

    void GetPosition()
    {
        targetPosition = PlayerTransform.position + new Vector3(x, 0, 0);
        targetPosition.z = -10;
        targetPosition += offset;
    }
}
