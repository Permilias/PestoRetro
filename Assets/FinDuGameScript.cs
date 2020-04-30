using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinDuGameScript : MonoBehaviour
{
    #region Singleton
    public static FinDuGameScript _instance;
    public void Awake()
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

    public Animator animFinDuGame;

    public void FinDuGame()
    {
        animFinDuGame.SetBool("gameFinished", true);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
