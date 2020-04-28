using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    #region Singleton
    public static ChunkManager _instance;

    public float distanceApparition;
    public List<GameObject> chuncks;

    private GameObject[] chuncksObject;
    private List<bool> chuncksIsInstantiate;

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

    private void Start()
    {
        chuncksIsInstantiate = new List<bool>();
        chuncksObject = new GameObject[chuncks.Count];
        for (int i = 0; i < chuncks.Count; i++)
        {
            chuncksIsInstantiate.Add(false);
            chuncksObject[i] = null;
        }
    }

    private void Update()
    {
        for (int i = 0; i < chuncks.Count; i++)
        {
            Debug.Log(Mathf.Abs(PlayerUtils.PlayerTransform.position.x - chuncks[i].transform.position.x) < distanceApparition);

            if (Mathf.Abs(PlayerUtils.PlayerTransform.position.x - chuncks[i].transform.position.x) < distanceApparition)
            {
                if (!chuncksIsInstantiate[i])
                {
                    chuncksIsInstantiate[i] = true;

                    chuncksObject[i] = Instantiate(chuncks[i], this.transform) as GameObject;
                }
            }
            else
            {
                if (chuncksIsInstantiate[i])
                {
                    chuncksObject[i].SetActive(false);
                }
            }
        }
    }
}
