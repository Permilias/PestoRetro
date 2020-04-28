using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    #region Singleton
    public static ChunkManager _instance;

    public List<GameObject> chuncks;

    private int actualchunk;
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
        actualchunk = -1;

        chuncksObject = new GameObject[chuncks.Count];
        for (int i = 0; i < chuncks.Count; i++)
        {
            chuncksObject[i] = Instantiate(chuncks[i], this.transform) as GameObject;
            chuncksObject[i].SetActive(false);
        }
    }

    private void Update()
    {
        OnWhichChunk();

        chuncksObject[actualchunk].SetActive(true);

        if (actualchunk - 2 >= 0)
        {
            chuncksObject[actualchunk - 2].SetActive(false);
            chuncksObject[actualchunk - 1].SetActive(true);
        }
        else if (actualchunk - 1 >= 0)
        {
            chuncksObject[actualchunk - 1].SetActive(true);
        }

        if (actualchunk + 2 < chuncks.Count)
        {
            chuncksObject[actualchunk + 2].SetActive(false);
            chuncksObject[actualchunk + 1].SetActive(true);
        }
        else if (actualchunk + 1 < chuncks.Count)
        {
            chuncksObject[actualchunk + 1].SetActive(true);
        }
    }

    private void OnWhichChunk()
    {
        float minDist = Mathf.Abs(PlayerUtils.PlayerTransform.position.x - chuncks[0].transform.position.x), dist;
        actualchunk = 0;

        for (int i = 1; i < chuncks.Count; i++)
        {
            dist = Mathf.Abs(PlayerUtils.PlayerTransform.position.x - chuncks[i].transform.position.x);

            if (dist < minDist)
            {
                minDist = dist;
                actualchunk = i;
            }
        }
    }
}
