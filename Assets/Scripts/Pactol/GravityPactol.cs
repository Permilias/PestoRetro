using UnityEngine;

public class GravityPactol : MonoBehaviour
{
    public static GravityPactol Instance;

    

    CharacterRaycaster raycaster;


    private void Awake()
    {
        Instance = this;
    }
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
