using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameoverscipt : MonoBehaviour
{
    public TextMeshProUGUI scoretext;
    public TextMeshProUGUI falsetext;
    public Button restartButton;
    public Button homeButton;
    public int score;
    public int falsescr;
    // Start is called before the first frame update
    void Start()
    {
        score = PlayerPrefs.GetInt("True Score");
        falsescr = PlayerPrefs.GetInt("False Score");
        scoretext.text = score.ToString();
        falsetext.text = falsescr.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void restartGame()
    {
        SceneManager.LoadScene(2);
    }
    public void homeGame()
    {
        SceneManager.LoadScene(0);
    }
}
