using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundIntegrations : MonoBehaviour
{
    public void PlayJump ()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.grazJump);
    }

    public void PlayFootsteps ()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.grazFootSteps);
    }

    public void PlayBruteCharge()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.enemyBruteCharge);
    }

    public void PlayBrutePunch()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.enemyBrutePunch);
    }

    public void PlayBruteDie()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.enemyBruteDies);
    }

    public void PlayBruteFootsteps()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.enemyBruteFootSteps);
    }

    public void PlaySoldadoDie()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.enemySimpleDies);
    }

    public void PlaySoldadoShoot()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.enemySimpleShoot);
    }

    public void PlaySoldadoFootsteps()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.enemySimpleFootsteps);
    }
}
