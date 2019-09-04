using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> positions = new List<Transform>();
    int leftMinotaurPosition = 8;
    int rightMinotaurPosition = 9;
    public bool left;
    public bool right;
    float lastMoveTime = 1.0f;
    float moveDelay = 1.0f;
    

    void Start()
    {
        if (left)
        {
            UpdateLeftMinotaurPosition();
        }
        else
        {
            UpdateRightMinotaurPosition();
        }
        lastMoveTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > lastMoveTime + moveDelay)
        {
            MovePosition();
            lastMoveTime = Time.time;
        }
    }

    private void UpdateLeftMinotaurPosition()
    {
        if(left && leftMinotaurPosition < positions.Count && leftMinotaurPosition >= 0)
        transform.position = positions[leftMinotaurPosition].position;
    }

    private void UpdateRightMinotaurPosition()
    {
        if(right && rightMinotaurPosition < positions.Count && rightMinotaurPosition >= 0)
        transform.position = positions[rightMinotaurPosition].position;
    }

    private void MovePosition()
    {
        leftMinotaurPosition++;
        UpdateLeftMinotaurPosition();
        rightMinotaurPosition--;
        UpdateRightMinotaurPosition();
        // ska kunna gå åt olika håll
        // ska ha en attack fas
        // ska inte kunna stå på samma plats som en annan minotaur
    }

}
