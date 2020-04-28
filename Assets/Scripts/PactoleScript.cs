using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PactoleScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pickUpText;
    [SerializeField] private Transform targetPoint;

    private bool pickUpAllowed;
    private bool objectPickedUp;

    private CharacterRaycaster raycast;

    private void Start()
    {
        pickUpText.gameObject.SetActive(false);
        objectPickedUp = false;

        raycast = GetComponent<CharacterRaycaster>();
    }

    private void Update()
    {
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }

        else if (objectPickedUp == true && Input.GetKeyDown(KeyCode.E))
        {
            Drop();     
        }

        if (!objectPickedUp)
        {
            Vector3 movement = new Vector3(0, -9.81f);
            movement *= Time.deltaTime;
            raycast.Move(movement);
        }

        Debug.Log(objectPickedUp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            pickUpText.gameObject.SetActive(true);
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.name.Equals("Player"))
        {
            pickUpText.gameObject.SetActive(false);
            pickUpAllowed = false;
        }
    }

    private void PickUp ()
    {
        Debug.Log("Pactole is picked up!");
        objectPickedUp = true;

        this.transform.position = targetPoint.transform.position;
        this.transform.parent = targetPoint.transform;

        PlayerController._instance.hasBag = true;
    }

    private void Drop ()
    {
        this.transform.parent = null;

        objectPickedUp = false;

        PlayerController._instance.hasBag = false;
    }
}
