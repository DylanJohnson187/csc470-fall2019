using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayManager : MonoBehaviour
{
    //Allows for spawning of prefab tiles during runtime
    public GameObject[] hallwayPrefab;

    //object will be the doubledoors to enter dmti
    public GameObject endofHall;

    //Allows for spawning collectible objects & power ups
    public GameObject[] spawnables;

    //Keep track of spawnables
    private List<GameObject> activeSpawnables = new List<GameObject>();

    //Keep track of current Hallways that are instantiated
    private List<GameObject> activeHallways = new List<GameObject>();

    //keep track of endof halls
    private List<GameObject> activeEOH = new List<GameObject>();

    //Keep track of player so it knows when to spawn new tiles infront
    private Transform playerTransform;

    private float zSpawn = 0;           //equal to 1 negative halwaylength
    private float hallwayLength = 12f;
    private int hallwaysOnScreen = 5;
    private int safeZone = 12;

    private int lastHallwayPFidx = 0;

    class coordinates
    {
        public int x;

        public int z;
    };

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 

        for(int i = 0; i < hallwaysOnScreen; i++)
        {
            if (i < 3)
                spawnHallway(0);
            else
                spawnHallway();
        }
        //at z location hallway length * hallwaysonscreen spawn the prefab for end of hallway this will just be a plane but it will have the background texture of the double doors in donnys
        spawnEndofHall();

    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.z - safeZone > zSpawn - hallwaysOnScreen * hallwayLength)
        {
            spawnCollectible();
            spawnHallway();
            deleteHallway();
            spawnEndofHall();
            deleteEOH();
            //deleteCollectible();
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

    private void spawnEndofHall()
    {
        GameObject go;
        go = Instantiate(endofHall) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * zSpawn + Vector3.up *1;
        activeEOH.Add(go);
    }
    private void deleteEOH()
    {
        Destroy(activeEOH[0]);
        activeEOH.RemoveAt(0);
    }
    private void spawnCollectible()
    {
        //Here will be the code to generate collectibles that the player will get to add to score or give immunity
        int SpawnProb = Random.Range(0, 3);
        if (SpawnProb == 1)
        {
            //figure out spawn locations
            coordinates spawnloc = selectRandomSpawn();
            int x = spawnloc.x;
            int z = spawnloc.z;

            //select which collectible to spawn - randomly
            /* 
             blue book - 1/2 chance = 10 points
             Red Book - 1/4 chance = 25 points
             gold book - 1/8 chance = 50 points
             A+ - 1/8 chance = 5 seconds of immunity
            */

            int collectibleSpawnProb = Random.Range(0, 15);


            GameObject go;
            
            if (collectibleSpawnProb < 7)
            {
                //spawn blue book
                go = Instantiate(spawnables[0]) as GameObject;
                go.transform.SetParent(transform);
                if (x < 0) {
                    go.transform.position = Vector3.forward * (zSpawn + z) + Vector3.left * x;
                }
                else if(x > 0) {
                    go.transform.position = Vector3.forward * (zSpawn + z) + Vector3.right * x;
                }
                else
                {
                    go.transform.position = Vector3.forward * (zSpawn + z);
                }
                activeSpawnables.Add(go);

            }
            else if(collectibleSpawnProb < 11)
            {
                //spawn Red Book
                go = Instantiate(spawnables[1]) as GameObject;
                go.transform.SetParent(transform);
                if (x < 0)
                {
                    go.transform.position = Vector3.forward * (zSpawn + z) + Vector3.left * x;
                }
                else if (x > 0)
                {
                    go.transform.position = Vector3.forward * (zSpawn + z) + Vector3.right * x;
                }
                else
                {
                    go.transform.position = Vector3.forward * (zSpawn + z);
                }
                activeSpawnables.Add(go);
            }
            else if (collectibleSpawnProb < 13)
            {
                //spawn Gold Book
                go = Instantiate(spawnables[2]) as GameObject;
                go.transform.SetParent(transform);
                if (x < 0)
                {
                    go.transform.position = Vector3.forward * (zSpawn + z) + Vector3.left * x;
                }
                else if (x > 0)
                {
                    go.transform.position = Vector3.forward * (zSpawn + z) + Vector3.right * x;
                }
                else
                {
                    go.transform.position = Vector3.forward * (zSpawn + z);
                }
                activeSpawnables.Add(go);
            }
            else if (collectibleSpawnProb < 15)
            {
                //spawn A+
                go = Instantiate(spawnables[3]) as GameObject;
                go.transform.SetParent(transform);
                if (x < 0)
                {
                    go.transform.position = Vector3.forward * (zSpawn + z) + Vector3.left * x;
                }
                else if (x > 0)
                {
                    go.transform.position = Vector3.forward * (zSpawn + z) + Vector3.right * x;
                }
                else
                {
                    go.transform.position = Vector3.forward * (zSpawn + z);
                }
                activeSpawnables.Add(go);
            }
        }
    }


    private void deleteCollectible()
    {
        Destroy(activeSpawnables[0]);
        activeSpawnables.RemoveAt(0);
    }
    private coordinates selectRandomSpawn()
    {
        int x;
        int z;
        int prob = Random.Range(0, 5);
        if (prob ==0)
        {
            x = -1;
            z = 2;
        }
        else if (prob ==1)
        {
            x = -1;
            z = 7;
        }
        else if (prob ==2)
        {
            x = 1;
            z = 2;
        }
        else if (prob ==3)
        {
            x = 1;
            z = 7;
        }
        else if (prob ==4)
        {
            x = 0;
            z = 2;
        }
        else
        {
            x = 0;
            z = 9;
        }
        coordinates spawnlocation = new coordinates();
        spawnlocation.x = x;
        spawnlocation.z = z;
        return spawnlocation;
    }
}
