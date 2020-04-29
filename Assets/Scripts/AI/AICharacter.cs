using UnityEngine;

public class AICharacter
{
    // ############### VARIABLES ###############
    // Commun variable
    private int life;
    private int pastaToLoot;

    // Soldado variable
    private Vector2 target;
    private int cooldown;
    private int pastaToShoot;

    // Brute variable
    private float walkSpeed;
    private float chargeSpeed;
    private int cacPower;
    private int chargePower;
    private int hitForCharge;
    private int hitRemaningForCharge;


    // ############### GET-SET ###############
    public int Life { get => life; set => life = value; }
    public int PastaToLoot { get => pastaToLoot; set => pastaToLoot = value; }

    public Vector2 Target { get => target; set => target = value; }
    public int Cooldown { get => cooldown; set => cooldown = value; }
    public int PastaToShoot { get => pastaToShoot; set => pastaToShoot = value; }

    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
    public float ChargeSpeed { get => chargeSpeed; set => chargeSpeed = value; }
    public int CacPower { get => cacPower; set => cacPower = value; }
    public int ChargePower { get => chargePower; set => chargePower = value; }
    public int HitForCharge { get => hitForCharge; set => hitForCharge = value; }
    public int HitRemaningForCharge { get => hitRemaningForCharge; set => hitRemaningForCharge = value; }


    // ############### CONSTRUCTEURS ###############
    public AICharacter(int life, int pastaToLoot, int cooldown, int pastaToShoot)
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

    public AICharacter(int life, int pastaToLoot, int cacPower, int chargePower, int hitForCharge, int hitRemainingForCharge, float walkSpeed, float chargeSpeed)
    {
        Life = life;
        PastaToLoot = pastaToLoot;

        Target = Vector2.zero;
        Cooldown = -1;
        PastaToShoot = 0;

        WalkSpeed = walkSpeed;
        ChargeSpeed = chargeSpeed;
        CacPower = cacPower;
        ChargePower = chargePower;
        HitForCharge = hitForCharge;
        HitRemaningForCharge = hitRemainingForCharge;
    }
}