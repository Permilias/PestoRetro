using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    #region Singleton
    public static OpenDoor _instance;

    void Awake()
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

    public List<string> tags;
    public List<GameObject> doors;
}
