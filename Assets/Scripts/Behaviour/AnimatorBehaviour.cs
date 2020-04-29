﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorBehaviour
{ 
    static Animator currentAnimator;

    public static Animator GetAnimator(Animator animator)
    {
        currentAnimator = animator;
        return animator;
    }

   public static void MovementAnimations(Vector3 movement)
    {
        if (!PlayerController._instance.jumpKey)
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
        if (PlayerController._instance.rightKey || PlayerController._instance.leftKey)
        {
        }
    }

    public static void JumpAnimations(Vector3 movement)
    {
        currentAnimator.SetBool("isJumping", true);
    }

    public static void CancelJumpAnimations()
    {
        currentAnimator.SetBool("isJumping", false);
    }
}