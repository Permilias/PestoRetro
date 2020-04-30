using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Singleton
    public static GameManager _instance;

    void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    public GameObject[] spriteLifes;
    public Animator animatorGameOver;

    [System.NonSerialized] public HealthSystem healthSystem;
    bool endGame;


    private void Start()
    {
        healthSystem = new HealthSystem(5);

        for (int i = 0; i < healthSystem.GetHealth(); i++)
        {
            spriteLifes[i].SetActive(true);
        }

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        spriteLifes[spriteLifes.Length - 1].SetActive(false);
        
        for (int i = 0; i < healthSystem.healthMax; i++)
        {
            if (i >= healthSystem.GetHealth())
            {
                spriteLifes[i].SetActive(false);
            }
            else
            {
                spriteLifes[i].SetActive(true);
            }
        }

        if (healthSystem.GetHealth() == 0) {
           // AnimatorBehaviour.DeadAnimations();
            StartCoroutine("RespawnPlayer");
            animatorGameOver.SetBool("gameOver", true);
            
        }
    }


    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(0.5f);

        endGame = true;
    }
}
