using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastaGun : MonoBehaviour
{
    public static PastaGun Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Vector2 shotLocalPosition;
    public int currentSelectedPasta;
    public bool cooking;
    public bool shootingLeft;
    public float groundLevel;

    public void RollPastaSelection(bool next)
    {


        cookingCount = 0f;
        cookedReady = false;
        cooking = false;


        if (next)
        {
            currentSelectedPasta++;
            if (currentSelectedPasta >= PastaManager.Instance.pastas.Length)
            {
                currentSelectedPasta = 0;
            }
        }
        else
        {
            currentSelectedPasta--;
            if (currentSelectedPasta < 0)
            {
                currentSelectedPasta = PastaManager.Instance.pastas.Length - 1;
            }
        }
    }

    public float reloadSpeed = 1f;
    public float reloadCount;
    bool reloading;

    public float cookingCount;
    public bool cookedReady;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RollPastaSelection(true);
            return;
        }

        if (reloadCount > 0f)
        {
            reloadCount -= Time.deltaTime;
            if(reloadCount <= 0f)
            {
                reloadCount = 0f;
            }
        }

        if(PastaManager.Instance.pastaAmounts[currentSelectedPasta] <= 0)
        {
            return;
        }

        if (cooking)
        {
            cookingCount += Time.deltaTime;
            if(cookingCount >= PastaManager.Instance.pastas[currentSelectedPasta].config.cookingSpeed)
            {
                cookedReady = true;
            }

            if(reloadCount <= 0f)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Shoot();
                }
            }

        }
        else
        {
            cookedReady = false;
            cookingCount = 0f;
        }

        if (Input.GetMouseButton(0))
        {
            cooking = true;
            return;
        }
        else
        {
            cookedReady = false;
            cooking = false;
        }


    }

    public void Shoot()
    {
        PastaManager.Instance.pastaAmounts[currentSelectedPasta] -= 1;
        Pasta shotPasta = PastaManager.Instance.pastas[currentSelectedPasta];
        PastaShotConfig shotConfig = cookedReady ? shotPasta.config.cookedShot : shotPasta.config.crudeShot;
        reloadSpeed = shotConfig.reloadSpeed;
        reloadCount = reloadSpeed;

        if (cookedReady) cookedReady = false;
        cooking = false;
        cookingCount = 0f;

        for (int i = 0; i < shotConfig.missileAmount; i++)
        {
            PastaProjectile projectile = PastaManager.Instance.CreateProjectileAtPosition(shotConfig, (Vector2)transform.position +
                (shootingLeft ? new Vector2(-shotLocalPosition.x, shotLocalPosition.y) : shotLocalPosition));

            if(shotConfig.missileAmount > 1)
            {
                int addedAngle = shootingLeft ? 0 : 0;
                float angleIncrement = shotConfig.missileSpreadAngle / (shotConfig.missileAmount - 1);
                float maxPositiveAngle = addedAngle + (shotConfig.missileSpreadAngle / 2f);

                projectile.transform.eulerAngles = new Vector3(0, 0, maxPositiveAngle - (angleIncrement * i));
            }
            else
            {
                projectile.transform.eulerAngles = Vector3.zero;
            }

            projectile.SetDirection(shootingLeft);
            projectile.Shoot();
        }       
    }
}
