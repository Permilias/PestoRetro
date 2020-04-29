using UnityEngine;
using TMPro;

public class PickUpAndDropPactol : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pickUpText;
    [SerializeField] private Transform targetPoint;

    public GameObject pactol;

    private bool pickUpAllowed;
    private bool passPasta;
    private string currentPassPasta;
    private string savePassPasta;

    private Transform pactolTransform;
    private SpriteRenderer pactolSprite;
    private GravityPactol pactolGravity;

    private void Start()
    {
        pickUpText.gameObject.SetActive(false);
        pickUpAllowed = false;

        passPasta = false;
        currentPassPasta = null;
        savePassPasta = null;

        pactolTransform = pactol.GetComponent<Transform>();
        pactolSprite = pactol.GetComponent<SpriteRenderer>();
        pactolGravity = pactol.GetComponent<GravityPactol>();
    }

    private void Update()
    {
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }

        else if (PlayerController._instance.hasBag == true && Input.GetKeyDown(KeyCode.E))
        {
            Drop();     
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Pactol"))
        {
            pickUpText.gameObject.SetActive(true);
            pickUpAllowed = true;
        }

        else if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("PassPasta")))
        {
            passPasta = true;
            currentPassPasta = collision.gameObject.tag;
            
            if (savePassPasta != null && savePassPasta.Equals(currentPassPasta))
            {
                pickUpAllowed = true;
                pickUpText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.name.Equals("Pactol"))
        {
            pickUpText.gameObject.SetActive(false);
            pickUpAllowed = false;
        }

        else if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("PassPasta")))
        {
            pickUpAllowed = false;
            passPasta = false;
            currentPassPasta = null;
            pickUpText.gameObject.SetActive(false);
        }
    }

    private void PickUp ()
    {
        if (savePassPasta != null && savePassPasta.Equals(currentPassPasta))
        {
            savePassPasta = null;
            pactolSprite.enabled = true;
        }

        pactolTransform.position = targetPoint.transform.position;
        pactolTransform.parent = targetPoint.transform;

        pactolGravity.enabled = false;
        PlayerController._instance.hasBag = true;
    }

    private void Drop ()
    {
        if (passPasta)
        {
            savePassPasta = currentPassPasta;
            pactolSprite.enabled = false;
        }

        pactolTransform.parent = null;

        pactolGravity.enabled = true;
        PlayerController._instance.hasBag = false;
    }
}
