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

        sr.gameObject.SetActive(true);


        basePos = transform.position;
        movingUp = true;
    }


    public void Collect()
    {
        //SFX

        PastaManager.Instance.pastaAmounts[pastaIndex] += givenAmount;
        collected = true;
        sr.gameObject.SetActive(false);
    }
}
