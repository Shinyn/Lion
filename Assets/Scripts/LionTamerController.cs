using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionTamerController : MonoBehaviour
{
    public List<Transform> positions = new List<Transform>();
    int currentPositionLeftSide = 1;
    int currentPositionRightSide = 0;

    void Start()
    {
        UpdateLeftPosition();
        UpdateRightPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        ButtonInput.OnLeftUpPressed += OnLeftUpPressed;
    }

    private void OnDisable()
    {
        ButtonInput.OnLeftUpPressed -= OnLeftUpPressed;
    }
    private void UpdateLeftPosition()
    {
        transform.position = positions[currentPositionLeftSide].position;
        Debug.Log("update pos left");
    }

    private void UpdateRightPosition()
    {
        transform.position = positions[currentPositionRightSide].position;
        Debug.Log("update pos right");
    }

    private void OnLeftUpPressed()
    {
        if (currentPositionLeftSide > 0)
        {
            currentPositionLeftSide--;
            UpdateLeftPosition();
        }
    }

    private void OnLeftDownPressed()
    {
        if (currentPositionLeftSide < positions.Count - 1)
        {
            currentPositionLeftSide++;
            UpdateLeftPosition();
        }
    }

    private void OnRightUpPressed()
    {
        if (currentPositionRightSide > 0)
        {
            currentPositionRightSide--;
            UpdateRightPosition();
        }
    }

    private void OnRightDownPressed()
    {
        if (currentPositionRightSide < positions.Count - 1)
        {
            currentPositionRightSide++;
            UpdateRightPosition();
            Debug.Log("rightDown updated");
        }
    }
}
