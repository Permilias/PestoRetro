using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    #region Singleton
    public static ChunkManager _instance;

    public float distanceApparition;
    public List<GameObject> chuncks;

    private GameObject[] chuncksObject;

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
        chuncksObject = new GameObject[chuncks.Count];
        for (int i = 0; i < chuncks.Count; i++)
        {
            chuncksObject[i] = Instantiate(chuncks[i], this.transform) as GameObject;
            chuncksObject[i].SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < chuncks.Count; i++)
        {
            if (Mathf.Abs(PlayerUtils.PlayerTransform.position.x - chuncks[i].transform.position.x) < distanceApparition)
            {
                if (!chuncksObject[i].activeSelf)
                {
                    chuncksObject[i].SetActive(true);
                }
            }
            else
            {
                if (chuncksObject[i].activeSelf)
                {
                    chuncksObject[i].SetActive(false);
                }
            }
        }
    }
}
