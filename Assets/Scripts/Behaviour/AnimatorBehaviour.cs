using System.Collections;
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
        if (PlayerController._instance.rightKey || PlayerController._instance.leftKey)
        {
            currentAnimator.SetBool("IsMoving", true);
        }
        else
        {
            currentAnimator.SetBool("IsMoving", false);
        }
    }
}
