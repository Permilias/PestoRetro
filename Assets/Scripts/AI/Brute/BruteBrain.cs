using System;
using UnityEngine;

enum ChargeTo { None, Left, Right }

public class BruteBrain : MonoBehaviour
{
    // ############### VARIABLES ###############
    public int life;
    public int lootedPasta;

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
        life = 15;
        lootedPasta = 0;

        walkSpeed = 2;
        chargeSpeed = 4;

        cacPower = 1;
        timeBetweenTwoCac = 1;

        chargePower = 2;
        timeToStopAfterCharge = 1;
        hitForCharge = 3;
    }

    private void Start()
    {
        chargeTo = ChargeTo.None;
        timerCharge = -1;
        timerImmobile = -1;
        timerAttack = -1;

        brute = new AICharacter(life, lootedPasta, cacPower, chargePower, hitForCharge, hitForCharge, walkSpeed, chargeSpeed);
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
                    ImpactCharge();
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
        }
    }

    private void Look()
    {
        if (PlayerUtils.PlayerTransform.position.x < this.transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void Reach()
    {
        if (raycaster.collisions.HaveHorizontalCollision() && raycaster.objectCollisionHorizontal.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            Attack();
            raycaster.collisions.Reset();
            raycaster.objectCollisionHorizontal = null;
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

    private void ImpactCharge()
    {
        GameManager._instance.healthSystem.Damage(brute.ChargePower);
        brute.HitRemaningForCharge = brute.HitForCharge;
        timerAttack = timeBetweenTwoCac;
        Debug.Log("Impact Charge");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Projectiles")))
        {
            if (brute.HitRemaningForCharge > 0)
            {
                PastaProjectile pastaProjectile = collision.gameObject.GetComponent<PastaProjectile>();
                if (pastaProjectile.shooter.Equals("Player"))
                {
                    if (pastaProjectile.shotConfig.cooked && pastaProjectile.pasta.config.pastaName.Equals("Spaghetti"))
                    {
                        timerImmobile = 2;
                    }

                    brute.Life -= pastaProjectile.pasta.degats;
                    brute.HitRemaningForCharge -= pastaProjectile.pasta.degats;

                    PastaManager.Instance.Repool(pastaProjectile);

                    if (brute.Life <= 0)
                    {
                        Dead();
                    }
                }
            }
        }
    }

    private void Dead()
    {
        Debug.Log("Loot de Pasta Soldado");

        for (int i = -2; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                PastaCollectible pastaCollectible = new PastaCollectible();
                pastaCollectible.pastaIndex = lootedPasta;
                pastaCollectible.Initialize();

                Vector3 position = new Vector3(this.transform.position.x + i, this.transform.position.y + j);

                Instantiate(pastaCollectible, position, Quaternion.identity);
            }
        }
        
        Destroy(this);
    }
}
