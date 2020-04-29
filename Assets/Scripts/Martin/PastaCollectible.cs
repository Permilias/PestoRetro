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


    public void Initialize()
    {
        Pasta pasta = PastaManager.Instance.pastas[pastaIndex];


        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;

        CircleCollider2D col = gameObject.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = pasta.config.collectibleColliderRadius;

        sr = new GameObject("CollectibleSprite").AddComponent<SpriteRenderer>();
        sr.sprite = pasta.config.collectibleSprite;
        sr.transform.parent = transform;
        sr.transform.localPosition = Vector3.zero;
        sr.transform.localScale = pasta.config.collectibleGraphicsScale;

        sr.enabled = true;


        basePos = transform.position;
        movingUp = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered !");
        if(!collected)
        {
            Collect();
        }

    }

    public void Collect()
    {
        collected = true;
        sr.enabled = false;
    }
}
