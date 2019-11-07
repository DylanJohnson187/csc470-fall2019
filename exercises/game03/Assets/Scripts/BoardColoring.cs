using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardColoring : MonoBehaviour
{
    public static BoardColoring Instance { set; get; }

    public GameObject highlightPrefab;
    private List<GameObject> highlights;
    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }

    private GameObject GetColorObj()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);        //will find first object where active self is equal to false
        
        if(go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }
        return go;
    }

    public void HighlightAllowedMoves(bool[,] moves)
    {
        for(int i = 0; i<8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if (moves[i, j])
                {
                    GameObject go = GetColorObj();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i+0.5f, 0, j+0.5f);               //Not sure about the flip of i and j will check
                }
            }
        }
    }

    public void HideColor()
    {
        foreach (GameObject go in highlights)
            go.SetActive(false);
    }
}
