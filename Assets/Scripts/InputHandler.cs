using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    [Header("Key Bindings")]
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode jump = KeyCode.UpArrow;
    public KeyCode actionKey = KeyCode.LeftShift;

    public PlayerController controller;

    void Update()
    {
        controller.leftKey = Input.GetKey(left);
        controller.leftKeyDown = Input.GetKeyDown(left);
        controller.leftKeyUp = Input.GetKeyUp(left);

        controller.rightKey = Input.GetKey(right);
        controller.rightKeyDown = Input.GetKeyDown(right);
        controller.rightKeyUp = Input.GetKeyUp(right);

        controller.jumpKey = Input.GetKey(jump);
        controller.jumpKeyDown = Input.GetKeyDown(jump);
        controller.jumpKeyUp = Input.GetKeyUp(jump);

        controller.actionsKey = Input.GetKey(actionKey);
        controller.actionsKeyDown = Input.GetKeyDown(actionKey);
        controller.actionsKeyUp = Input.GetKeyUp(actionKey);
    }
}
