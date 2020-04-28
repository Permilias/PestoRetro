using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform self;
    public static Transform PlayerTransform   { get {   return PlayerController._instance.self; }   }
    public float cameraSpeed = 0.3f;
    public float cameraBackSpeed = 0.3f;
    public float maxDelta = 1;
    float targetPosition;
    float leftDelta;


    // Start is called before the first frame update
    void Start()
    {
        leftDelta = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController._instance.movementVector.x > 0)
            leftDelta += Mathf.Abs(PlayerController._instance.movementVector.x) * cameraSpeed;
        else if (PlayerController._instance.movementVector.x < 0) leftDelta -= Time.deltaTime * cameraBackSpeed;

        leftDelta = Mathf.Clamp(leftDelta, 0, maxDelta);

        targetPosition = PlayerTransform.position.x + leftDelta;

        self.position = new Vector3(targetPosition, self.position.y, self.position.z);
    }
}
