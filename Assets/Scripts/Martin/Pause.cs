using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    bool paused;
    public GameObject body;

    private void Start()
    {
        body.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            paused = !paused;
            body.SetActive(paused);
            Time.timeScale = paused ? 0f : 1f;
        }
    }
}
