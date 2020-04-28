using UnityEngine;

public class AICharacter
{
    // ############### VARIABLES ###############
    // Commun variable
    private int life;
    private Pasta pastaToLoot;

    // Soldado variable
    private Vector2 target;
    private int cooldown;
    private Pasta pastaToShoot;

    // Brute variable
    private float walkSpeed;
    private float chargeSpeed;
    private int cacPower;
    private int chargePower;
    private int hitForCharge;
    private int hitRemaningForCharge;


    // ############### GET-SET ###############
    public int Life { get => life; set => life = value; }
    public Pasta PastaToLoot { get => pastaToLoot; set => pastaToLoot = value; }

    public Vector2 Target { get => target; set => target = value; }
    public int Cooldown { get => cooldown; set => cooldown = value; }
    public Pasta PastaToShoot { get => pastaToShoot; set => pastaToShoot = value; }

    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
    public float ChargeSpeed { get => chargeSpeed; set => chargeSpeed = value; }
    public int CacPower { get => cacPower; set => cacPower = value; }
    public int ChargePower { get => chargePower; set => chargePower = value; }
    public int HitForCharge { get => hitForCharge; set => hitForCharge = value; }
    public int HitRemaningForCharge { get => hitRemaningForCharge; set => hitRemaningForCharge = value; }


    // ############### CONSTRUCTEURS ###############
    public AICharacter(int life, Pasta pastaToLoot, int cooldown, Pasta pastaToShoot)
    {
        Life = life;
        PastaToLoot = pastaToLoot;

        Target = Vector2.zero;
        Cooldown = cooldown;
        PastaToShoot = pastaToShoot;

        CacPower = -1;
        ChargePower = -1;
        HitForCharge = -1;
        HitRemaningForCharge = -1;
    }

    public AICharacter(int life, Pasta pastaToLoot, int cacPower, int chargePower, int hitForCharge, int hitRemainingForCharge, float walkSpeed, float chargeSpeed)
    {
        Life = life;
        PastaToLoot = pastaToLoot;

        Target = Vector2.zero;
        Cooldown = -1;
        PastaToShoot = null;

        WalkSpeed = walkSpeed;
        ChargeSpeed = chargeSpeed;
        CacPower = cacPower;
        ChargePower = chargePower;
        HitForCharge = hitForCharge;
        HitRemaningForCharge = hitRemainingForCharge;
    }
}