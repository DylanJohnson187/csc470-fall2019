using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public int spX;
    public int spY;
    public GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        int sX = Random.Range(0, GM.gridWidth);
        int sY = Random.Range(0, GM.gridHeight);
        spX = sX;
        spY = sY;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
