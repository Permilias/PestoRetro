using UnityEngine;

public class SoldadoBrain : MonoBehaviour
{
    // ############### VARIABLES ###############
    public int life;
    public int cooldown;
    public Vector2 shotOffset;

    public int shotPasta;
    public int lootedPasta;


    private bool facingLeft;

    private float timerReload;
    private float timerImmobile;
    private float gravity = -9.81f;
    
    private AICharacter soldado;
    private CharacterRaycaster raycaster;

    
    // ############### FUNCTIONS ###############
    private void Reset()
    {
        life = 20;
        cooldown = 5;

        shotPasta = 1;
        lootedPasta = 1;
    }

    private void Start()
    {
        timerReload = cooldown;
        timerImmobile = -1;

        soldado = new AICharacter(life, lootedPasta, cooldown, shotPasta);
        raycaster = GetComponent<CharacterRaycaster>();
    }

    private void Update()
    {
        if (timerImmobile >= 0)
        {
            timerImmobile -= Time.deltaTime;
        }
        else
        {
            Look();

            if (timerReload >= 0)
            {
                timerReload -= Time.deltaTime;
            }
            else
            {
                Shoot();
                timerReload = soldado.Cooldown;
            }
        }
    }

    private void Look()
    {
        if (PlayerUtils.PlayerTransform.position.x < this.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingLeft = true;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingLeft = false;
        }
    }


    private void Shoot()
    {
        Vector2 offset = facingLeft ? new Vector2(-shotOffset.x, shotOffset.y) : shotOffset;

        PastaProjectile projectile = PastaManager.Instance.CreateProjectileAtPosition(PastaManager.Instance.pastas[shotPasta].config.crudeShot,
                                                                                    (Vector2)transform.position + offset, "IA");
        projectile.SetDirection(facingLeft);
        projectile.Shoot();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Projectiles")))
        {
            PastaProjectile pastaProjectile = collision.gameObject.GetComponent<PastaProjectile>();
            if (pastaProjectile.shooter.Equals("Player"))
            {
                if (pastaProjectile.shotConfig.cooked && pastaProjectile.pasta.config.pastaName.Equals("Spaghetti")) //spag cuite
                {
                    timerImmobile = 2;
                }

                soldado.Life -= pastaProjectile.pasta.degats;

                PastaManager.Instance.Repool(pastaProjectile);

                if (soldado.Life <= 0)
                {
                    Dead();
                }
            }

        }
    }

    private void Dead()
    {
        Debug.Log("Loot de Pasta Soldado");

        PastaCollectible pastaCollectible = new PastaCollectible();
        pastaCollectible.pastaIndex = lootedPasta;
        pastaCollectible.Initialize();

        Instantiate (pastaCollectible, this.transform);
        Destroy(this);
    }
}
