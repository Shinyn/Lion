using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInput : MonoBehaviour
{
    public enum Button
    {
        topLeft,
        topRight,
        bottomLeft,
        bottomRight
    };

    public Button button;

    public delegate void ButtonPressed();
    public static event ButtonPressed OnLeftUpPressed;
    

    private void OnMouseDown()
    {
        if (OnLeftUpPressed != null && button == Button.topLeft)
        {
            OnLeftUpPressed();
            //Debug.Log("top left");
        }
        else if (button == Button.bottomLeft)
        {
            //Debug.Log("bottom left");
        }
        else if (button == Button.topRight)
        {
            //Debug.Log("top right");
        }
        else if (button == Button.bottomRight)
        {
            //Debug.Log("bottom right");
        }
        
    }
}
