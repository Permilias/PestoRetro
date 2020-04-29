using UnityEngine;

public class GravityPactol : MonoBehaviour
{
    CharacterRaycaster raycaster;

    void Start()
    {
        raycaster = GetComponent<CharacterRaycaster>();
    }
    
    void Update()
    {
        Vector3 movement = new Vector3(0, -9.81f);
        movement *= Time.deltaTime;
        raycaster.Move(movement);
    }
}
