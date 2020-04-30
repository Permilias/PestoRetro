using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastaManager : MonoBehaviour
{
    public static PastaManager Instance;
    private void Awake()
    {
        Instance = this;

        projectilePool = new Queue<PastaProjectile>();
        FillProjectilePool();

        pastas = new Pasta[configs.Length];
        pastaAmounts = new int[configs.Length];

        for (int i = 0; i < configs.Length; i++)
        {
            pastas[i] = new Pasta(configs[i]);
            pastaAmounts[i] = configs[i].startingAmount;
        }
    }



    public PastaConfig[] configs;
    public int[] pastaAmounts;

    [HideInInspector]
    public Pasta[] pastas;

    public int poolingAmount;
    Queue<PastaProjectile> projectilePool;
    int projectileAmount;

    public Vector3 collectibleTopPos;
    public float collectibleMoveSpeed;

    void FillProjectilePool()
    {
        for(int i = 0; i < poolingAmount; i++)
        {
            PastaProjectile newProjectile = new GameObject("Projectile_" + projectileAmount.ToString()).AddComponent<PastaProjectile>();
            newProjectile.transform.parent = transform;
            newProjectile.col = newProjectile.gameObject.AddComponent<BoxCollider2D>();
            newProjectile.sr = new GameObject("Projectile_" + projectileAmount.ToString() + "_Sprite").AddComponent<SpriteRenderer>();
            newProjectile.sr.transform.parent = newProjectile.transform;
            projectilePool.Enqueue(newProjectile);
            newProjectile.gameObject.SetActive(false);
            projectileAmount++;
        }
    }

    public PastaProjectile CreateProjectileAtPosition(PastaShotConfig shotConfig, Vector2 _position, string shooter, Pasta pasta)
    {

        if (projectilePool.Count < 1)
        {

            FillProjectilePool();
        }
        PastaProjectile createdProjectile = projectilePool.Dequeue();
        createdProjectile.shooter = shooter;
        createdProjectile.gameObject.SetActive(true);
        createdProjectile.transform.position = new Vector3(_position.x, _position.y, 0f);
        createdProjectile.Initialize(shotConfig);
        createdProjectile.pasta = pasta;
        return createdProjectile;

    }

    public void Repool(PastaProjectile projectile)
    {
        projectile.transform.parent = transform;
        projectilePool.Enqueue(projectile);
        projectile.gameObject.SetActive(false);
    }
}
