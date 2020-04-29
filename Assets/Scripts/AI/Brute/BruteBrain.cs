﻿using UnityEngine;

enum ChargeTo { None, Left, Right }

public class BruteBrain : MonoBehaviour
{
    // ############### VARIABLES ###############
    public int life;

    public float walkSpeed;
    public float chargeSpeed;

    public int cacPower;
    public float timeBetweenTwoCac;

    public int chargePower;
    public float timeToStopAfterCharge;
    public int hitForCharge;

    public int timeSpaghettiCookedStopIA;


    private float timerCharge;
    private float timerImmobile;
    private float timerAttack;
    private float gravity = -9.81f;
    private ChargeTo chargeTo;

    private AICharacter brute;
    private CharacterRaycaster raycaster;
    private SpriteRenderer spriteRenderer;
    private PastaCollectible[] pastaLoot;


    // ############### FUNCTIONS ###############
    private void Reset()
    {
        life = 21;

        walkSpeed = 2;
        chargeSpeed = 4;

        cacPower = 1;
        timeBetweenTwoCac = 1;

        chargePower = 2;
        timeToStopAfterCharge = 1;
        hitForCharge = 4;
        
        timeSpaghettiCookedStopIA = 4;
    }

    private void Start()
    {
        chargeTo = ChargeTo.None;
        timerCharge = -1;
        timerImmobile = -1;
        timerAttack = -1;

        brute = new AICharacter(life, cacPower, chargePower, hitForCharge, hitForCharge, walkSpeed, chargeSpeed);
        raycaster = GetComponent<CharacterRaycaster>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        pastaLoot = GetComponentsInChildren<PastaCollectible>(true);
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
                        timerImmobile = timeSpaghettiCookedStopIA;
                    }

                    brute.Life -= pastaProjectile.shotConfig.damage;
                    brute.HitRemaningForCharge -= pastaProjectile.shotConfig.damage;

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
        Debug.Log("Loot de Pasta Brute");

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                pastaLoot[i + (j * 3)].gameObject.SetActive(true);
                pastaLoot[i + (j * 3)].gameObject.transform.parent = null;
                pastaLoot[i + (j * 3)].gameObject.transform.position = new Vector3(this.transform.position.x + (i - 1), this.transform.position.y + (j<<1));
                pastaLoot[i + (j * 3)].Initialize();
            }
        }

        Destroy(gameObject);
    }
}
