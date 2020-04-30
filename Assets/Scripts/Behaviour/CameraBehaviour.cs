using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraBehaviour : MonoBehaviour
{

    public static Transform PlayerTransform   { get {   return PlayerController._instance.self; }   }
    public float cameraSpeed = 0.3f;
    public float cameraBackSpeed = 0.3f;
    public float maxDelta = 1;
    Vector3 targetPosition;
    float leftDelta;
    public float tweenSpeed;

    void Start()
    {
        leftDelta = 0;
    }

    void Update()
    {
        if (PlayerController._instance.movementVector.x > 0)
        {
            leftDelta = -maxDelta;
        }

        else if (PlayerController._instance.movementVector.x < 0)
        {
            leftDelta = maxDelta;
        }


        targetPosition = PlayerTransform.position + new Vector3(leftDelta, 0, 0);
        targetPosition.y = transform.position.y;
        targetPosition.z = 0;

        transform.DOMove(targetPosition, tweenSpeed);
    }
}
