using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PactoleScript : MonoBehaviour
{
    public void PickUp ()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Debug.Log("Pactole is picked up!");
    }
}
