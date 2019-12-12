using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayManager : MonoBehaviour
{
    //Allows for spawning of prefab tiles during runtime
    public GameObject[] hallwayPrefab;

    //Keep track of current Hallways that are instantiated
    private List<GameObject> activeHallways = new List<GameObject>();

    //Keep track of player so it knows when to spawn new tiles infront
    private Transform playerTransform;

    private float zSpawn = 0;           //equal to 1 negative halwaylength
    private float hallwayLength = 12f;
    private int hallwaysOnScreen = 5;
    private int safeZone = 12;

    private int lastHallwayPFidx = 0;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 

        for(int i = 0; i < hallwaysOnScreen; i++)
        {
            if (i == 0)
                spawnHallway(0);
            else if (i == 1)
            {
                spawnHallway(1);            //standardize the start so nothing unexpected happens immediately
            }
            else
                spawnHallway();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.z - safeZone > zSpawn - hallwaysOnScreen * hallwayLength)
        {
            spawnHallway();
            deleteHallway();
        }
        
    }

    private void spawnHallway(int prefabidx = -1)
    {
        GameObject go;
        if(prefabidx ==-1)
            go = Instantiate(hallwayPrefab[getRandomindex()]) as GameObject;
        else
            go = Instantiate(hallwayPrefab[prefabidx]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * zSpawn;
        zSpawn += hallwayLength;
        activeHallways.Add(go);
    }

    private void deleteHallway()
    {
        Destroy(activeHallways[0]);
        activeHallways.RemoveAt(0);
    }

    private int getRandomindex()
    {
        int randomIdx = lastHallwayPFidx;
        while(randomIdx == lastHallwayPFidx)
        {
            randomIdx = Random.Range(0, hallwayPrefab.Length);

        }
        lastHallwayPFidx = randomIdx;
        return randomIdx;
    }
}
