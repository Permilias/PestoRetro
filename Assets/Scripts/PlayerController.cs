﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Singleton
    public static PlayerController _instance;

    void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    [Header("Movement Without Bag")]
    public float speed = 5f;
    public float accelerationTime = .5f;
    public AnimationCurve accelerationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Movement With Bag")]
    public float speedBag = 2f;
    public float accelerationTimeBag = .5f;
    public AnimationCurve accelerationCurveBag = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Vertical Movement")]
    public float gravity = 9.81f;
    public AnimationCurve jumpCurve = AnimationCurve.Constant(0, 1, 1);
    public float jumpReleaseMultiplier = 3f;
    public int maxJumpsAllowed = 1;

    [Header("Physics")]
    public float movementThreshold = 0.0015f;

    [Header("References")]
    public Transform self;
    public CharacterRaycaster raycaster;
    public Transform graphicTransform;
    public Animator animator;

    [System.NonSerialized]
    public bool leftKeyDown, leftKeyUp, leftKey,
        rightKeyDown, rightKeyUp, rightKey,
        jumpKeyDown, jumpKeyUp, jumpKey,
        actionsKeyDown, actionsKeyUp, actionsKey;

    [Header("Debug")]
    public bool debugMode;
    public Transform fakeGroundLevel;

    [System.NonSerialized] public Vector2 movementVector;

    public bool isGrounded { get { return raycaster.collisionFlags.down; } }

    bool isJumping;

    int jumpsAllowedLeft;

    float timeSinceJumped, timeSinceAccelerated;

    //TEMPORARY
    public bool hasBag;

    void Start()
    {
        timeSinceJumped = 10f;
        jumpsAllowedLeft = maxJumpsAllowed;
    }

    
    void Update()
    {
        InputUpdate();
        JumpUpdate();
        MovementUpdate();
        PostMovementJumpUpdate();
        DebugUpdate();

        Debug.Log(PlayerUtils.PlayerTransform.position);
    }

    void InputUpdate()
    {
        movementVector = Vector2.zero;
        if (leftKey) movementVector.x--;
        if (rightKey) movementVector.x++;
        if (jumpKeyDown) TryJump();
    }

    void JumpUpdate()
    {
        movementVector.y = gravity * -1f;

        if (!isJumping) return;

        float releaseMultiplier = jumpKey ? 1 : jumpReleaseMultiplier;

        timeSinceJumped += Time.deltaTime * releaseMultiplier;

        float gravityMultiplier = jumpCurve.Evaluate(timeSinceJumped);
        movementVector.y *= -1 * gravityMultiplier;

        if (timeSinceJumped > jumpCurve.keys[jumpCurve.keys.Length - 1].time)
            isJumping = false;

    }

    void PostMovementJumpUpdate()
    {
        if (isGrounded)
        {
            isJumping = false;
            jumpsAllowedLeft = maxJumpsAllowed;
        }
    }

    void MovementUpdate()
    {
        if (movementVector.x == 0) timeSinceAccelerated = 0;
        else timeSinceAccelerated += Time.deltaTime;

        float accelerationMultiplier = 1f;
        
        float usedSpeed;
        switch (hasBag)
        {
            case true:
                if (accelerationTime > 0) accelerationMultiplier = accelerationCurveBag.Evaluate(timeSinceAccelerated / accelerationTimeBag);
                usedSpeed = speedBag * accelerationMultiplier;
                break;
            case false:
                if (accelerationTime > 0) accelerationMultiplier = accelerationCurve.Evaluate(timeSinceAccelerated / accelerationTime);
                usedSpeed = speed * accelerationMultiplier;
                break;
            default:
                if (accelerationTime > 0) accelerationMultiplier = accelerationCurveBag.Evaluate(timeSinceAccelerated / accelerationTimeBag);
                usedSpeed = speedBag * accelerationMultiplier;
                break;
        }

        
        movementVector.x *= usedSpeed;

        Vector3 finalVector = Time.deltaTime * movementVector;

        Move(finalVector);
    }

    void DebugUpdate()
    {
        if (!debugMode) return;

        if (self.position.y <= fakeGroundLevel.position.y)
        {
            jumpsAllowedLeft = maxJumpsAllowed;
            self.position = new Vector3(self.position.x, fakeGroundLevel.position.y, self.position.z);
        }
    }

    void TryJump()
    {
        if (!isGrounded) return;
        if (jumpsAllowedLeft == 0) return;
        jumpsAllowedLeft--;
        StartJump();
    }

    void StartJump()
    {
        isJumping = true;
        timeSinceJumped = 0f;
    }

    void Move(Vector3 movement)
    {
        if (movement.x != 0)
            movement.x = raycaster.CastBoxHorizontal(movement.x);
        if (Mathf.Abs(movement.x) < movementThreshold)
            movement.x = 0;

        if (movement.y != 0)
            movement.y = raycaster.CastBoxVertical(movement.y);
        if (Mathf.Abs(movement.y) < movementThreshold)
            movement.y = 0;

        if (movement.x > 0) raycaster.collisionFlags.left = false;
        if (movement.x < 0) raycaster.collisionFlags.right = false;
        if (movement.y > 0) raycaster.collisionFlags.down = false;
        if (movement.y < 0) raycaster.collisionFlags.up = false;

        self.Translate(movement);
    }
}
