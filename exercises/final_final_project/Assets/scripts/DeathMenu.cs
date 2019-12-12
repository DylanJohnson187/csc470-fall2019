using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public Text endText;
    public Image background;

    public bool fadeOut = false;

    private float transition = 0f;

    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!fadeOut)
            return;
        transition += Time.deltaTime;
        background.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transition);
    }
    public void endMenu(float score)
    {

        gameObject.SetActive(true);
        endText.text = ((int)score).ToString();
        fadeOut = true;
    }
    public void replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void returnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
