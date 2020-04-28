using UnityEngine;

enum ChargeTo { None, Left, Right }

public class BruteBrain : MonoBehaviour
{
    // ############### VARIABLES ###############
    public int life;
    public Pasta pastaToLoot;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    public float walkSpeed;
    public float chargeSpeed;
    public int cacPower;
    public int chargePower;
    public int hitForCharge;


    private float movementThreshold = 0.0015f;
    private float gravity = -9.81f;
    private ChargeTo chargeTo;

    private AICharacter brute;
    private Transform transform;
    private CharacterRaycaster raycaster;
    private SpriteRenderer spriteRenderer;


    // ############### FUNCTIONS ###############
    private void Reset()
    {
        life = 50;
        walkSpeed = 2;
        chargeSpeed = 4;
        cacPower = 2;
        chargePower = 4;
        hitForCharge = 5;
    }

    private void Start()
    {
        chargeTo = ChargeTo.None;

        brute = new AICharacter(life, pastaToLoot, cacPower, chargePower, hitForCharge, 0, walkSpeed, chargeSpeed);
        transform = GetComponent<Transform>();
        raycaster = GetComponent<CharacterRaycaster>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Look();

        if (brute.HitRemaningForCharge <= 0)
        {
            Charge();
        }
        else
        {
            Reach();
        }

    }

    private void Look()
    {
        if (PlayerUtils.PlayerTransform.position.x < transform.position.x)
        {
            spriteRenderer.sprite = spriteLeft;
        }
        else
        {
            spriteRenderer.sprite = spriteRight;
        }
    }

    private void Reach()
    {
        if (raycaster.collisions.HaveCollision() && raycaster.collisionMask.value.Equals(LayerMask.NameToLayer("Player")))
        {
            Attack();
        }
        else
        {
            Vector3 movement;
            if (PlayerUtils.PlayerTransform.position.x < transform.position.x)
            {
                movement = new Vector3(-walkSpeed, gravity);
            }
            else
            {
                movement = new Vector3(walkSpeed, gravity);
            }
            movement *= Time.deltaTime;
            raycaster.Move(movement);
        }
    }

    private void Attack()
    {
        // lose life
        Debug.Log("Attack Cac");
    }

    private void Charge()
    {
        Vector3 movement = Vector3.zero;

        // Init charge
        if (chargeTo.Equals(ChargeTo.None))
        {
            if (PlayerUtils.PlayerTransform.position.x < transform.position.x)
            {
                chargeTo = ChargeTo.Left;
            }
            else
            {
                chargeTo = ChargeTo.Right;
            }
        }

        // Update info charge
        else if (chargeTo.Equals(ChargeTo.Left) && PlayerUtils.PlayerTransform.position.x < transform.position.x)
        {
            movement = new Vector3(-chargeSpeed, gravity);
        }
        else if (chargeTo.Equals(ChargeTo.Right) && PlayerUtils.PlayerTransform.position.x > transform.position.x)
        {
            movement = new Vector3(chargeSpeed, gravity);
        }

        // Play or stop charge
        if (movement.Equals(Vector3.zero))
        {
            brute.HitRemaningForCharge = brute.HitForCharge;
            chargeTo = ChargeTo.None;
        }
        else
        {
            movement *= Time.deltaTime;
            raycaster.Move(movement);
        }
    }


    public void TakeDamage(Pasta pasta)
    {
        brute.Life -= pasta.degats;
        brute.HitRemaningForCharge--;
    }

    private void Dead()
    {
        Destroy(this);
    }
}
