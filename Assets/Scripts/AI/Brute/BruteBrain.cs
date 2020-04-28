using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // ############### VARIABLES ###############
    public int life;
    public Vector2 coord;
    public Pasta pastaToLoot;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    public int cacPower;
    public int chargePower;
    public int hitForCharge;

    
    private AICharacter brute;


    // ############### FUNCTIONS ###############
    private void Start()
    {
        brute = new AICharacter(life, coord, pastaToLoot, spriteLeft, cacPower, chargePower, hitForCharge, hitForCharge);
    }

    private void Update()
    {
        Look();

        if (brute.HitRemaningForCharge <= 0)
        {
            Charge();
        }
        else
        {
            Reach();
        }
        
    }

    private void Look()
    {
        if (PlayerController._instance.coord.x < brute.Coord.x)
        {
            brute.Sprite = spriteLeft;
        }
        else
        {
            brute.Sprite = spriteRight;
        }
    }

    private void Reach()
    {
        // Mvt

        if (true)
        {
            Attack();
        }
    }

    private void Attack()
    {
        PlayerController._instance.life -= brute.CacPower;
    }

    private void Charge()
    {
        // Mvt (speed up) --> stop if this.x >/< player.x

        // collision --> degat + projeter ?

        // if mvt stop ou degat
        /*if ()
        {
            brute.HitRemaningForCharge = brute.HitForCharge;
        }*/
    }

    private void Dead()
    {
        Destroy(this);
    }

    public void TakeDamage(Pasta pasta)
    {
        brute.Life -= pasta.degats;
        brute.HitRemaningForCharge--;
    }
}
