using System.Collections;
using UnityEngine;

public class SoldadoBrain : MonoBehaviour
{
    // ############### VARIABLES ###############
    public int life;
    public int cooldown;
    public Vector2 shotOffset;

    public int timeSpaghettiCookedStopIA;
    public int shotPasta;
    public Animator soldadoAnimator;


    private bool facingLeft;

    private float timerReload;
    private float timerImmobile;
    private float gravity = -9.81f;
    
    private AICharacter soldado;
    private CharacterRaycaster raycaster;
    private PastaCollectible pastaLoot;

    
    // ############### FUNCTIONS ###############
    private void Reset()
    {
        life = 5;
        cooldown = 5;

        timeSpaghettiCookedStopIA = 4;
        shotPasta = 1;

        soldadoAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        timerReload = cooldown;
        timerImmobile = Random.Range(0, 5);

        soldado = new AICharacter(life, cooldown, shotPasta);
        raycaster = GetComponent<CharacterRaycaster>();
        pastaLoot = GetComponentInChildren<PastaCollectible>(true);

        //AnimatorBehaviour.GetAnimator(soldadoGraphics.GetComponent<Animator>());
    }

    private void Update()
    {
        if (timerImmobile >= 0)
        {
            timerImmobile -= Time.deltaTime;
        }
        else
        {
            //Anim Stop
            Look();

            if (timerReload >= 0)
            {
                timerReload -= Time.deltaTime;
            }
            else
            {
                if (Mathf.Abs(PlayerUtils.PlayerTransform.position.x - this.transform.position.x) <= (PastaManager.Instance.pastas[shotPasta].config.crudeShot.range))
                {
                    soldadoAnimator.SetBool("IsShooting", true);
                    
                    StartCoroutine("Shoot");
                    timerReload = soldado.Cooldown;
                }
            }
        }
    }

    private void Look()
    {
        if (PlayerUtils.PlayerTransform.position.x < this.transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingLeft = true;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingLeft = false;
        }
    }


    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.3f);

        Vector2 offset = facingLeft ? new Vector2(-shotOffset.x, shotOffset.y) : shotOffset;

        PastaProjectile projectile = PastaManager.Instance.CreateProjectileAtPosition(PastaManager.Instance.pastas[shotPasta].config.crudeShot,
                                                                                    (Vector2)transform.position + offset, PastaManager.Instance.pastas[shotPasta]);
        projectile.SetDirection(facingLeft);
        projectile.transform.eulerAngles = Vector3.zero;
        projectile.shotByPlayer = false;
        projectile.Shoot();

        soldadoAnimator.SetBool("IsShooting", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Projectiles")))
        {
            PastaProjectile pastaProjectile = collision.gameObject.GetComponent<PastaProjectile>();
            if (pastaProjectile.shotByPlayer)
            {
                if (pastaProjectile.shotConfig.cooked && pastaProjectile.pasta.config.pastaName.Equals("Spaghetti")) //spag cuite
                {
                    timerImmobile = timeSpaghettiCookedStopIA;

                    //Anim Start
                }

                SoundManager.Instance.PlaySound(SoundManager.Instance.enemySimpleHit);
                FXPlayer.Instance.PlayFX("Blood", this.transform.position);
                soldado.Life -= pastaProjectile.shotConfig.damage;

                PastaManager.Instance.Repool(pastaProjectile);

                if (soldado.Life <= 0 && !soldadoAnimator.GetBool("IsDead"))
                {
                    soldadoAnimator.SetBool("IsDead", true);
                    
                    StartCoroutine("Dead");
                }
            }
        }
    }

    private IEnumerator Dead()
    {
        timerImmobile = 3;
        yield return new WaitForSeconds(2);

        pastaLoot.gameObject.SetActive(true);
        pastaLoot.gameObject.transform.parent = null;
        pastaLoot.gameObject.transform.position = this.transform.position;
        pastaLoot.Initialize();
        
        Destroy(gameObject);
    }
}
