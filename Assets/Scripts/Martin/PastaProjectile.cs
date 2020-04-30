using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastaProjectile : MonoBehaviour
{
    public Pasta pasta;
    public SpriteRenderer sr;
    public BoxCollider2D col;
    public PastaShotConfig shotConfig;

    public bool shotByPlayer;
    public string shooter;


    public void Initialize(PastaShotConfig _shotConfig)
    {
        gameObject.layer = LayerMask.NameToLayer("Projectiles");

        shotConfig = _shotConfig;

        sr.sprite = shotConfig.projectileSprite;
        sr.transform.localScale = shotConfig.spriteObjectScale;
        sr.enabled = false;

        col.size = shotConfig.colliderSize;
        col.isTrigger = true;
        
       
    }

    public void SetDirection(bool left)
    {
        goesLeft = left;
        float x = left ? -shotConfig.spriteObjectOffset.x : shotConfig.spriteObjectOffset.x;
        Vector2 pos = shotConfig.spriteObjectOffset;
        pos.x = x;
        sr.transform.localPosition = pos;

    }

    public Vector2 target;
    public Vector2 relative;
    public Vector2 travelDistance;
    public Vector2 posIncrement;
    public Vector2 pos;

    bool travelling;

    float travelDuration;
    float travelCount;
    float speed;

    public bool goesLeft;
    public void Shoot()
    {
        AcquireTarget();
        travelling = true;
    }

    void AcquireTarget()
    {
        transform.parent = null;

        pos = transform.position;

        relative = (Vector2)(goesLeft ? -transform.right : transform.right);
        target = pos + (relative * shotConfig.range);

        if(shotConfig.goesToGround)
        {
            target.y = PastaGun.Instance.groundLevel;
        }

        travelDuration = shotConfig.travelDuration;
        travelCount = 0f;   

        travelDistance = target - pos;
        posIncrement = travelDistance / travelDuration;
        if (shotConfig.straight) posIncrement.y = 0f;



    }

    private void Update()
    {
        if(travelling)
        {
            Travel();
        }
    }

    public void Travel()
    {
        pos += posIncrement * Time.deltaTime;
        
        Vector3 curveBonus = (transform.up * shotConfig.yTrajectory.Evaluate(travelCount / travelDuration)) - transform.up;
        transform.position = (Vector3)pos + curveBonus;

        if (!sr.enabled) sr.enabled = true;

        travelCount += Time.deltaTime;
        if(travelCount >= travelDuration)
        {
            if(shotConfig.goesToGround)
            {
                if(shotConfig.groundTouchingSound != null)
                {
                    SoundManager.Instance.PlaySound(shotConfig.groundTouchingSound);
                }
            }
            Repool();
            travelling = false;
        }
    }

    void Repool()
    {
        PastaManager.Instance.Repool(this);
    }
}
