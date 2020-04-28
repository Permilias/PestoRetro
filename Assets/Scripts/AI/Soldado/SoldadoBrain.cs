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


    private float timer;
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
        timer = cooldown;

        soldado = new AICharacter(life, pastaToLoot, cooldown, pastaToShoot);
        raycaster = GetComponent<CharacterRaycaster>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(0, gravity);
        movement *= Time.deltaTime;
        raycaster.Move(movement);

        Look();

        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Shoot();
            timer = soldado.Cooldown;
        }

        if (raycaster.collisions.HaveCollision() && raycaster.collisionMask.value.Equals(LayerMask.NameToLayer("Projectiles")))
        {
            Debug.Log("Soldado - Take Damage");
            //TakeDamage();
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
        Destroy(this);
    }
}
