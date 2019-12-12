using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scorescript : MonoBehaviour
{
    // Start is called before the first frame update
    public float score = 0f;
    public Text scoreText;

    private int scoreModifier = 1;
    private int finalScoreModifier = 16;
    private int scoreToNextModifier = 2;           // i want this to increase exponentially so 10 may not be a good starting point

    private bool IamDead = false;

    public DeathMenu dm;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IamDead)
            return;                     //score stops on death
        if(score>= scoreToNextModifier)
        {
            increaseScoreMod();
        }
        score += (Time.deltaTime * scoreModifier);
        scoreText.text = ((int)score).ToString();
    }

    private void increaseScoreMod()
    {
        
        scoreToNextModifier = scoreToNextModifier * scoreToNextModifier;
        if (scoreModifier < finalScoreModifier)
        {
            scoreModifier++;
        }
        GetComponent<PlayerScript>().setSpeed(scoreModifier);
    }

    public void amDead()
    {
        IamDead = true;
        dm.endMenu(score);
    }

    public int addToScore(int bonus = 0)
    {
        Debug.Log(bonus);
        return bonus;
    }

}
