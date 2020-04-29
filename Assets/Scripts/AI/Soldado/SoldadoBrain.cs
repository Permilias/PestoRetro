using UnityEngine;

public class SoldadoBrain : MonoBehaviour
{
    // ############### VARIABLES ###############
    public int life;
    public Pasta pastaToLoot;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    public int cooldown;
    public Pasta pastaToShoot;


    private float timerCharge;
    private float timerImmobile;
    private float gravity = -9.81f;

    private AICharacter soldado;
    private CharacterRaycaster raycaster;
    private SpriteRenderer spriteRenderer;


    // ############### FUNCTIONS ###############
    private void Reset()
    {
        life = 20;
        cooldown = 5;
    }

    private void Start()
    {
        timerCharge = cooldown;
        timerImmobile = -1;

        soldado = new AICharacter(life, pastaToLoot, cooldown, pastaToShoot);
        raycaster = GetComponent<CharacterRaycaster>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(0, gravity);
        movement *= Time.deltaTime;
        //raycaster.Move(movement);

        if (timerImmobile >= 0)
        {
            timerImmobile -= Time.deltaTime;
        }
        else
        {
            Look();

            if (timerCharge >= 0)
            {
                timerCharge -= Time.deltaTime;
            }
            else
            {
                Shoot();
                timerCharge = soldado.Cooldown;
            }

            if (raycaster.collisions.HaveHorizontalCollision() && raycaster.objectCollisionHorizontal.layer.Equals(LayerMask.NameToLayer("Projectiles")))
            {
                Debug.Log("Soldado - Take Damage");
                
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
                Debug.Log("Soldado - Take Damage");

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

    private void Shoot()
    {
        Debug.LogWarning("Soldado - Shoot");
        //PlayerController.Shoot(PlayerController._instance.coord, soldado.PastaToShoot);
    }

    private void TakeDamage(Pasta pasta)
    {
        soldado.Life -= pasta.degats;

        if (soldado.Life <= 0)
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
