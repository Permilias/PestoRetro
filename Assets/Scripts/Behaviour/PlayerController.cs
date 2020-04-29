using System.Collections;
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
    public Transform graphicsTransform;
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

   public bool isGrounded { get { return raycaster.collisions.below; } }

    [HideInInspector] public bool isJumping;

    int jumpsAllowedLeft;

    float timeSinceJumped, timeSinceAccelerated;

    //TEMPORARY
    public bool hasBag;

    CharacterRaycaster character;

    void Start()
    {
        character = GetComponent<CharacterRaycaster>();
        timeSinceJumped = 10f;
        jumpsAllowedLeft = maxJumpsAllowed;
        AnimatorBehaviour.GetAnimator(animator);
    }

    void Update()
    {
        if (raycaster.collisions.above || raycaster.collisions.below)
        {
            movementVector.y = 0;
        }

        InputUpdate();
        JumpUpdate();
        MovementUpdate();
        PostMovementJumpUpdate();
        DebugUpdate();
        AnimationUpdate();

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager._instance.healthSystem.Damage(1);
            Debug.Log(GameManager._instance.healthSystem.GetHealth());
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            GameManager._instance.healthSystem.Heal(1);
            Debug.Log(GameManager._instance.healthSystem.GetHealth());
        }
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
        {
            isJumping = false;
            AnimatorBehaviour.CancelJumpAnimations();
        }
            


        
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

        float accelerationMultiplier = 1;

        switch (hasBag)
        {
            case true:
                if (accelerationTime > 0) accelerationMultiplier = accelerationCurveBag.Evaluate(timeSinceAccelerated / accelerationTime);
                float usedSpeed = speedBag * accelerationMultiplier;
                movementVector.x *= usedSpeed;
                break;
            case false:
                if (accelerationTime > 0) accelerationMultiplier = accelerationCurve.Evaluate(timeSinceAccelerated / accelerationTimeBag);
                float usedSpeedBag = speed * accelerationMultiplier;
                movementVector.x *= usedSpeedBag;
                break;
            default:
                break;
        }

        Vector3 finalVector = Time.deltaTime * movementVector;

        Move(finalVector);
    }

    void DebugUpdate()
    {
        /*
        if (!debugMode) return;

        if (self.position.y <= fakeGroundLevel.position.y)
        {
            jumpsAllowedLeft = maxJumpsAllowed;
            self.position = new Vector3(self.position.x, fakeGroundLevel.position.y, self.position.z);
        }
        */
    }

    void TryJump()
    {
        if (jumpsAllowedLeft == 0) return;
        if (hasBag) return;
        jumpsAllowedLeft--;
        StartJump();
        AnimatorBehaviour.JumpAnimations(movementVector);
    }

    void StartJump()
    {
        isJumping = true;
        timeSinceJumped = 0f;

        
    }

    void Move(Vector3 movement)
    {

        /*      if (movement.x > 0) raycaster.flags.left = false;
              if (movement.x < 0) raycaster.flags.right = false;
              if (movement.y > 0) raycaster.flags.below = false;
              if (movement.y < 0) raycaster.flags.above = false;

          */
        if (Mathf.Abs(movement.y) < movementThreshold)
            movement.y = 0;


        character.Move(movement);

        AnimatorBehaviour.MovementAnimations(movement);
        
    }

    void AnimationUpdate()
    {
        if (movementVector.x != 0)
            graphicsTransform.localScale = new Vector3(
                Mathf.Abs(graphicsTransform.transform.localScale.x) * Mathf.Sign(movementVector.x),
                graphicsTransform.transform.localScale.y,
                graphicsTransform.transform.localScale.z
            );
    }
}
