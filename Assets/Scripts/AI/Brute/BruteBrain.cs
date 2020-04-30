using System.Collections;
using UnityEngine;

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
    public Animator bruteAnimator;


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
        timeToStopAfterCharge = 3;
        hitForCharge = 4;
        
        timeSpaghettiCookedStopIA = 4;
        bruteAnimator = GetComponentInChildren<Animator>();
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
        if (OpenDoor._instance.start)
        {
            if (timerCharge >= 0)
            {
                timerCharge -= Time.deltaTime;
            }
            else if (timerImmobile >= 0)
            {
                timerImmobile -= Time.deltaTime;
                bruteAnimator.SetBool("IsCharging", false);
            }
            else
            {
                Look();

                if (raycaster.collisions.HaveHorizontalCollision() && raycaster.objectCollisionHorizontal.layer.Equals(LayerMask.NameToLayer("Player")))
                {
                    bruteAnimator.SetBool("HasImpact", true);
                    bruteAnimator.SetBool("IsCharging", false);

                    StartCoroutine("ImpactCharge");

                    raycaster.collisions.Reset();
                    raycaster.objectCollisionHorizontal = null;

                    timerCharge = timeToStopAfterCharge;
                }
                else
                {
                    bruteAnimator.SetBool("IsCharging", true);

                    Charge();
                }
            }
        }
    }

    /*
    private void OldUpdate()
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
                    bruteAnimator.SetBool("HasImpact", true);
                    StartCoroutine("ImpactCharge");
                }
                else
                {
                    bruteAnimator.SetBool("IsCharging", true);
                    Charge();
                }
            }
            else
            {
                Reach();
            }
        }
    }
    */

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
            StartCoroutine("Attack");

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

    private IEnumerator Attack()
    {
        if (timerAttack < 0)
        {
            bruteAnimator.SetBool("IsPunching", true);

            yield return new WaitForSeconds(1);

            GameManager._instance.healthSystem.Damage(cacPower);
            timerAttack = timeBetweenTwoCac;

            bruteAnimator.SetBool("IsPunching", false);
        }
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
            bruteAnimator.SetBool("IsCharging", false);
        }
        else
        {
            movement *= Time.deltaTime;
            raycaster.Move(movement);
        }
    }

    private IEnumerator ImpactCharge()
    {
        yield return new WaitForSeconds(1);

        GameManager._instance.healthSystem.Damage(brute.ChargePower);
        brute.HitRemaningForCharge = brute.HitForCharge;
        timerAttack = timeBetweenTwoCac;
        chargeTo = ChargeTo.None;

        bruteAnimator.SetBool("HasImpact", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Projectiles")))
        {
            //if (brute.HitRemaningForCharge > 0)
            //{
                PastaProjectile pastaProjectile = collision.gameObject.GetComponent<PastaProjectile>();
                if (pastaProjectile.shotByPlayer)
                {
                    if (pastaProjectile.shotConfig.cooked && pastaProjectile.pasta.config.pastaName.Equals("Spaghetti"))
                    {
                        timerImmobile = timeSpaghettiCookedStopIA;
                    }

                    SoundManager.Instance.PlaySound(SoundManager.Instance.enemyBruteHit);
                    brute.Life -= pastaProjectile.shotConfig.damage;
                    brute.HitRemaningForCharge -= pastaProjectile.shotConfig.damage;

                    PastaManager.Instance.Repool(pastaProjectile);

                    if (brute.Life <= 0 && !bruteAnimator.GetBool("IsDead"))
                    {
                        bruteAnimator.SetBool("IsDead", true);
                    
                        StartCoroutine("Dead");
                    }
                }
            //}
        }
    }

    private IEnumerator Dead()
    {
        timerImmobile = 3;
        yield return new WaitForSeconds(2);

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

        FinDuGameScript._instance.FinDuGame();

        Destroy(gameObject);

    }
}
