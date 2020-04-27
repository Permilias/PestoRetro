using UnityEngine;

public class AICharacter
{
    // ############### VARIABLES ###############
    // Commun variable
    private int life;
    private Vector2 coord;
    private Pasta pastaToLoot;
    private Sprite sprite;

    // Soldado variable
    private Vector2 target;
    private int cooldown;
    private Pasta pastaToShoot;

    // Brute variable
    private int cacPower;
    private int chargePower;
    private int hitForCharge;
    private int hitRemaningForCharge;


    // ############### GET-SET ###############
    public int Life { get => life; set => life = value; }
    public Vector2 Coord { get => coord; set => coord = value; }
    public Pasta PastaToLoot { get => pastaToLoot; set => pastaToLoot = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }

    public Vector2 Target { get => target; set => target = value; }
    public int Cooldown { get => cooldown; set => cooldown = value; }
    public Pasta PastaToShoot { get => pastaToShoot; set => pastaToShoot = value; }

    public int CacPower { get => cacPower; set => cacPower = value; }
    public int ChargePower { get => chargePower; set => chargePower = value; }
    public int HitForCharge { get => hitForCharge; set => hitForCharge = value; }
    public int HitRemaningForCharge { get => hitRemaningForCharge; set => hitRemaningForCharge = value; }
    

    // ############### CONSTRUCTEURS ###############
    public AICharacter(int life, Vector2 coord, Pasta pastaToLoot, Sprite sprite, int cooldown, Pasta pastaToShoot)
    {
        Life = life;
        Coord = coord;
        PastaToLoot = pastaToLoot;
        Sprite = sprite;

        Target = Vector2.zero;
        Cooldown = cooldown;
        PastaToShoot = pastaToShoot;

        CacPower = -1;
        ChargePower = -1;
        HitForCharge = -1;
        HitRemaningForCharge = -1;
    }

    public AICharacter(int life, Vector2 coord, Pasta pastaToLoot, Sprite sprite, int cacPower, int chargePower, int hitForCharge, int hitRemainingForCharge)
    {
        Life = life;
        Coord = coord;
        PastaToLoot = pastaToLoot;
        Sprite = sprite;

        Target = Vector2.zero;
        Cooldown = -1;
        PastaToShoot = null;

        CacPower = cacPower;
        ChargePower = chargePower;
        HitForCharge = hitForCharge;
        HitRemaningForCharge = hitRemainingForCharge;
    }
}