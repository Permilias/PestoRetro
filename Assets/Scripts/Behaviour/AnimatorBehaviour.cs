﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorBehaviour
{ 
    static Animator currentAnimator;
    static bool shooting;

    public static Animator GetAnimator(Animator animator)
    {
        currentAnimator = animator;
        return animator;
    }

   public static void MovementAnimations(Vector3 movement)
    {
        if (!PlayerController._instance.jumpKey && shooting == false)
        {
            if (PlayerController._instance.rightKey || PlayerController._instance.leftKey || movement.x != 0f)
            {
                currentAnimator.SetBool("IsMoving", true);
                if (PlayerController._instance.hasBag)
                {
                    currentAnimator.speed = 0.8f;
                }
                
            }
            else
            {
                currentAnimator.SetBool("IsMoving", false);
                currentAnimator.speed = 1f;
            }
        }
        else
        {
            if (PlayerController._instance.hasBag)
            {
                currentAnimator.SetBool("IsMoving", true);
                currentAnimator.speed = 0.8f;
            }
            else
            {
                currentAnimator.SetBool("IsMoving", false);
            }

            
        }
    }

    public static void ShootingAnimations(Vector3 movement)
    {
        shooting = true;
        currentAnimator.SetBool("IsShooting", true);
    }

    public static void MovingAndShootingAnimations(Vector3 movement)
    {
        shooting = true;
        currentAnimator.SetBool("IsShootingAndMoving", true);
    }

    public static void StopShootingAnimations(Vector3 movement)
    {
        shooting = false;
        currentAnimator.SetBool("IsShootingAndMoving", false);
        currentAnimator.SetBool("IsShooting", false);
    }

    public static void StopMovingAndShootingAnimations(Vector3 movement)
    {
        currentAnimator.SetBool("IsShootingAndMoving", false);
    }

    public static void JumpAnimations(Vector3 movement)
    {
        currentAnimator.SetBool("isJumping", true);
    }

    public static void CancelJumpAnimations()
    {
        currentAnimator.SetBool("isJumping", false);
    }

    public static void DeadAnimations()
    {
       // currentAnimator.SetBool("IsDead", true);
    }
}
