using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    #region Singleton
    public static ChunkManager _instance;

    [Range(1, 7)] public int nbChunkToDisplay;
    public List<GameObject> chuncks;

    private int actualchunk;
    private int nbChunkToDisplayOnEachPart;
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
        if (nbChunkToDisplay % 2 == 0) nbChunkToDisplay++;
        nbChunkToDisplayOnEachPart = (nbChunkToDisplay - 1) >> 1;

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
        DisplayChunk();
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

    private void DisplayChunk()
    {
        chuncksObject[actualchunk].SetActive(true);

        DisplayChunkPartLeft();
        DisplayChunkPartRight();
    }

    private void DisplayChunkPartLeft()
    {
        if (actualchunk - (nbChunkToDisplayOnEachPart + 1) >= 0)
        {
            chuncksObject[actualchunk - (nbChunkToDisplayOnEachPart + 1)].SetActive(false);
        }
        for (int i = nbChunkToDisplayOnEachPart; i > 0; i--)
        {
            if (actualchunk - i >= 0)
            {
                for (int j = 1; j <= i; j++)
                {
                    chuncksObject[actualchunk - j].SetActive(true);
                }

                break;
            }
        }
    }

    private void DisplayChunkPartRight()
    {
        if (actualchunk + (nbChunkToDisplayOnEachPart + 1) < chuncksObject.Length)
        {
            chuncksObject[actualchunk + (nbChunkToDisplayOnEachPart + 1)].SetActive(false);
        }
        for (int i = nbChunkToDisplayOnEachPart; i > 0; i--)
        {
            if (actualchunk + i < chuncksObject.Length)
            {
                for (int j = 1; j <= i; j++)
                {
                    chuncksObject[actualchunk + j].SetActive(true);
                }

                break;
            }
        }
    }
    
}
