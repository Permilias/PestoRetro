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
    public float timeBetweenTwoCac;

    public int chargePower;
    public float timeToStopAfterCharge;
    public int hitForCharge;


    private float timerCharge;
    private float timerImmobile;
    private float timerAttack;
    private float gravity = -9.81f;
    private ChargeTo chargeTo;

    private AICharacter brute;
    private CharacterRaycaster raycaster;
    private SpriteRenderer spriteRenderer;


    // ############### FUNCTIONS ###############
    private void Reset()
    {
        life = 50;

        walkSpeed = 2;
        chargeSpeed = 4;

        cacPower = 2;
        timeBetweenTwoCac = 1;

        chargePower = 4;
        timeToStopAfterCharge = 1;
        hitForCharge = 5;
    }

    private void Start()
    {
        chargeTo = ChargeTo.None;
        timerCharge = -1;
        timerImmobile = -1;
        timerAttack = -1;

        brute = new AICharacter(life, pastaToLoot, cacPower, chargePower, hitForCharge, 0, walkSpeed, chargeSpeed);
        raycaster = GetComponent<CharacterRaycaster>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (timerAttack >= 0)
        {
            timerAttack -= Time.deltaTime;
        }

        if (timerCharge >= 0)
        {
            timerCharge -= Time.deltaTime;
        }
        else if (timerImmobile >= 0)
        {
            timerImmobile -= Time.deltaTime;
        }
        else
        {
            Look();
            
            if (brute.HitRemaningForCharge <= 0)
            {
                if (raycaster.collisions.HaveHorizontalCollision() && raycaster.objectCollisionHorizontal.layer.Equals(LayerMask.NameToLayer("Player")))
                {
                    GameManager._instance.healthSystem.Damage(chargePower);
                    brute.HitRemaningForCharge = brute.HitForCharge;
                    Debug.Log("Impact Charge");
                }
                else
                {
                    Charge();
                }
            }
            else
            {
                Reach();
            }

            if (raycaster.collisions.HaveHorizontalCollision() && raycaster.objectCollisionHorizontal.layer.Equals(LayerMask.NameToLayer("Projectiles")))
            {
                Debug.Log("Brute - Take Damage");

                /*
                if (raycaster.objectCollisionHorizontal) //spag cuite
                {
                    timerImmobile = // tmpImobile --> pasta
                }

                TakeDamage(raycaster.objectCollisionHorizontal) //dega de la pasta
                */
            }

            if (raycaster.collisions.HaveVerticalCollision() && raycaster.objectCollisionVertical.layer.Equals(LayerMask.NameToLayer("Projectiles")))
            {
                Debug.Log("Brute - Take Damage");

                /*
                if (raycaster.objectCollisionVertical) //spag cuite
                {
                    timerImmobile = // tmpImobile --> pasta
                }

                TakeDamage(raycaster.objectCollisionVertical) //dega de la pasta
                */
            }
        }
    }

    private void Look()
    {
        if (PlayerUtils.PlayerTransform.position.x < this.transform.position.x)
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
        if (raycaster.collisions.HaveHorizontalCollision() && raycaster.objectCollisionHorizontal.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            Attack();
        }
        else
        {
            Vector3 movement;
            if (PlayerUtils.PlayerTransform.position.x < this.transform.position.x)
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
        if (timerAttack >= 0) return;

        GameManager._instance.healthSystem.Damage(cacPower);
        timerAttack = timeBetweenTwoCac;
        Debug.Log("Attack Cac");
    }

    private void Charge()
    {
        Vector3 movement = Vector3.zero;

        // Init charge
        if (chargeTo.Equals(ChargeTo.None))
        {
            if (PlayerUtils.PlayerTransform.position.x < this.transform.position.x)
            {
                chargeTo = ChargeTo.Left;
            }
            else
            {
                chargeTo = ChargeTo.Right;
            }
        }


        // Update info charge
        if (chargeTo.Equals(ChargeTo.Left) && PlayerUtils.PlayerTransform.position.x < this.transform.position.x)
        {
            movement = new Vector3(-chargeSpeed, gravity);
        }
        else if (chargeTo.Equals(ChargeTo.Right) && PlayerUtils.PlayerTransform.position.x > this.transform.position.x)
        {
            movement = new Vector3(chargeSpeed, gravity);
        }


        // Play or stop charge
        if (movement.Equals(Vector3.zero))
        {
            brute.HitRemaningForCharge = brute.HitForCharge;
            chargeTo = ChargeTo.None;
            timerCharge = timeToStopAfterCharge;
        }
        else
        {
            movement *= Time.deltaTime;
            raycaster.Move(movement);
        }
    }

    private void TakeDamage(Pasta pasta)
    {
        brute.Life -= pasta.degats;
        brute.HitRemaningForCharge--;

        if (brute.Life <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        Debug.Log("Loot de Pasta Soldado");
        // Instantiate (pasta, this.transform);
        Destroy(this);
    }
}
