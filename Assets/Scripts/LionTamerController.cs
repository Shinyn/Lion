using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionTamerController : MonoBehaviour
{
    public List<Transform> positions = new List<Transform>();
    int currentPositionLeftSide = 1;
    int currentPositionRightSide = 4;
    public bool left;
    private ButtonInput buttonInput;

    void Start()
    {
        if (left)
        {
            UpdateLeftPosition();
        }
        else
        {
            UpdateRightPosition();
        }
    }

    public void OnLeftUpPressed()
    {
        if (currentPositionLeftSide > 0)
        {
            currentPositionLeftSide--;
            UpdateLeftPosition();
        }
    }

    public void OnLeftDownPressed()
    {
        //Debug.Log("Left down");
        if (currentPositionLeftSide < 2)
        {
            currentPositionLeftSide++;
            UpdateLeftPosition();
        }
    }

    public void OnRightUpPressed()
    {
        if (currentPositionRightSide > 3)
        {
            currentPositionRightSide--;
            UpdateRightPosition();
        }
    }

    public void OnRightDownPressed()
    {
        if (currentPositionRightSide < positions.Count -1)
        {
            currentPositionRightSide++;
            UpdateRightPosition();
        }
    }

    private void UpdateLeftPosition()
    {
        transform.position = positions[currentPositionLeftSide].position;
    }

    private void UpdateRightPosition()
    {
        transform.position = positions[currentPositionRightSide].position;
    }
}
