using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionTamerController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> positions = new List<Transform>();
    public int currentPosition = 1;
    private ButtonInput buttonInput;
    public bool leftTamer;

    void Start()
    {
        UpdateCurrentPosition();
    }

    public void OnLeftUpPressed()
    {
        if (currentPosition > 0)
        {
            currentPosition--;
            UpdateCurrentPosition();
        }
    }

    public void OnLeftDownPressed()
    {
        if (currentPosition < 2)
        {
            currentPosition++;
            UpdateCurrentPosition();
        }
    }

    public void OnRightUpPressed()
    {
        if (currentPosition > 4)
        {
            currentPosition--;
            UpdateCurrentPosition();
        }
    }

    public void OnRightDownPressed()
    {
        if (currentPosition < positions.Count -2)
        {
            currentPosition++;
            UpdateCurrentPosition();
        }
    }

    public void UpdateCurrentPosition()
    {
        transform.position = positions[currentPosition].position;
    }
}
