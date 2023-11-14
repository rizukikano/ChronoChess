using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    private TMP_Text highscoreTxt;
    void Start()
    {
        highscoreTxt = gameObject.GetComponent<TMP_Text>();
        GetHighscore();
    }
    public void GetHighscore(){
        highscoreTxt.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
}
