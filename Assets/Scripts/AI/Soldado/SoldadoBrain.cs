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
    private AICharacter soldado;
    private Transform transform;
    private SpriteRenderer spriteRenderer;


    // ############### FUNCTIONS ###############
    private void Reset()
    {
        life = 20;
        cooldown = 5;
    }

    private void Start()
    {
        soldado = new AICharacter(life, pastaToLoot, cooldown, pastaToShoot);
        transform = GetComponent<Transform>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        timer = cooldown;
    }

    private void Update()
    {
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

    private void Shoot()
    {
        Debug.LogWarning("Soldado - Shoot");
        //PlayerController.Shoot(PlayerController._instance.coord, soldado.PastaToShoot);
    }
    
    

    public void TakeDamage(Pasta pasta)
    {
        soldado.Life -= pasta.degats;
    }

    private void Dead()
    {
        Destroy(this);
    }
}
