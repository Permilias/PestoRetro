using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PastaCollectible : MonoBehaviour
{
    public bool collected;
    public int pastaIndex;
    int givenAmount;
    SpriteRenderer sr;

    bool operating;
    bool movingUp;
    Vector3 basePos;

    private void Update()
    {
        if (collected) return;

        if(Vector2.Distance(transform.position, PlayerController._instance.transform.position) < radius)
        {
            if(!collected)
            {
                Collect();
            }
        }

        if(movingUp)
        {
            if(!operating)
            {
                operating = true;
                transform.DOMove(basePos + PastaManager.Instance.collectibleTopPos, PastaManager.Instance.collectibleMoveSpeed).SetEase(Ease.Linear).OnComplete(() =>
                {
                    operating = false;
                    movingUp = false;
                });

            }

        }
        else
        {
            if(!operating)
            {
                operating = true;
                transform.DOMove(basePos, PastaManager.Instance.collectibleMoveSpeed).SetEase(Ease.Linear).OnComplete(() =>
                {
                    movingUp = true;
                    operating = false;
                });
            }

        }

    }

    float radius;
    public void Initialize()
    {
        Pasta pasta = PastaManager.Instance.pastas[pastaIndex];

        radius = pasta.config.collectibleColliderRadius;

        sr = new GameObject("CollectibleSprite").AddComponent<SpriteRenderer>();
        sr.sprite = pasta.config.collectibleSprite;
        sr.transform.parent = transform;
        sr.transform.localPosition = Vector3.zero;
        sr.transform.localScale = pasta.config.collectibleGraphicsScale;

        sr.enabled = true;


        basePos = transform.position;
        movingUp = true;
    }


    public void Collect()
    {
        //SFX

        PastaManager.Instance.pastaAmounts[pastaIndex] += givenAmount;
        collected = true;
        sr.enabled = false;
    }
}
