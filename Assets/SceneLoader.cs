using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("SceneManagement")]
    [SerializeField, Tooltip("Enter the scene name")] private string sceneName;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void DebugButton ()
    {
        Debug.Log("salope");
    }
}
