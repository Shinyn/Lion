using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameManager gameManager;
    private void OnMouseDown()
    {
        gameManager.RestartGame();
    }
}
