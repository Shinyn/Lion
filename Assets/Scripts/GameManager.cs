using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int escapedLions = 0;
    int points = 0;
    string sceneName = "Main";
    public List<GameObject> misses = new List<GameObject>();
    public GameObject gameOverSign;
    int activeMiss = 0;
    public TextMeshPro score;

    public LionTamerController lionTamerController1;
    public LionTamerController lionTamerController2;

    public MinotaurController minotaurController1;
    public MinotaurController minotaurController2;


    private void Start()
    {
        gameOverSign.SetActive(false);
    }
    public void EscapedLions()
    {
        if (escapedLions < 3)
        {
            Debug.Log(escapedLions);
            escapedLions++;
            misses[activeMiss].SetActive(true);
            activeMiss++;
            // tänd missarna
        }

        else
            GameOver();
    }

    private void GameOver()
    {
        // stäng av kontroller och minotaurer och visa game over skylt
        gameOverSign.SetActive(true);

        minotaurController1.gameObject.SetActive(false);
        minotaurController2.gameObject.SetActive(false);

        lionTamerController1.gameObject.SetActive(false);
        lionTamerController2.gameObject.SetActive(false);

        // delay på minotaurerna så dom syns i sista position innan gameover
        // lägga till sprites
        // fixa så dom inte hoppar tillbaka på en annan minotaur vid save eller fail
        // fixa så dom hoppar tillbaka 1 steg i taget vid save eller fail
        // byt till touch och testa på mobil - Optional (mousedown funkar på telefon också)
        // lägg till 2 "escaped" punkter när minotaurerna tagit sig ut
        // lägg till 2 "scared" punkter för tamers när minotaurerna tagit sig ut
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void AddPoint()
    {
        points++;
        score.text = points.ToString();
    }
}
