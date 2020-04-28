using UnityEngine;

public class SoldadoBrain : MonoBehaviour
{
    // ############### VARIABLES ###############
    public int life;
    public Vector2 coord;
    public Pasta pastaToLoot;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    public int cooldown;
    public Pasta pastaToShoot;


    private float timer;
    private AICharacter soldado;


    // ############### FUNCTIONS ###############
    private void Start()
    {
        soldado = new AICharacter(life, coord, pastaToLoot, spriteLeft, cooldown, pastaToShoot);
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
        if (PlayerController._instance.coord.x < soldado.Coord.x)
        {
            soldado.Sprite = spriteLeft;
        }
        else
        {
            soldado.Sprite = spriteRight;
        }
    }

    private void Shoot()
    {
        //PlayerController.Shoot(PlayerController._instance.coord, soldado.PastaToShoot);
    }

    private void Dead()
    {
        Destroy(this);
    }

    public void TakeDamage(Pasta pasta)
    {
        soldado.Life -= pasta.degats;
    }
}
