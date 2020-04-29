using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [System.NonSerialized] public HealthSystem healthSystem;


    private void Start()
    {
        healthSystem = new HealthSystem(3);

        for (int i = 0; i < healthSystem.GetHealth(); i++)
        {
            spriteLifes[i].SetActive(true);
        }

        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        spriteLifes[spriteLifes.Length - 1].SetActive(false);
        
        for (int i = 0; i < healthSystem.GetHealth(); i++)
        {
            spriteLifes[i].SetActive(true);
        }
    }
}
