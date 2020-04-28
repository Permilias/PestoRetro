using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PactoleScript : MonoBehaviour
{
    bool canPickUp = false;

    public void EnterInTrigger ()
    {
        canPickUp = true;
    }

    private void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("is pick up");
        }
    }

    public void ExitInTrigger()
    {
        Debug.Log("you can't pick up");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hello");
    }
}
