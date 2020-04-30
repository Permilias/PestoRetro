using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, PlayerController._instance.transform.position) < 1f)
        {
            GameManager._instance.healthSystem.Heal(1);
            Destroy(gameObject);
        }
    }
}
