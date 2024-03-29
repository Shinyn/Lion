﻿using System.Collections;
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
    [HideInInspector]
    public LionTamerController lionTamer;

    private void OnMouseDown()
    {
        if (button == Button.topLeft)
        {
            lionTamer.OnLeftUpPressed();
        }
        else if (button == Button.bottomLeft)
        {
            lionTamer.OnLeftDownPressed();
        }
        else if (button == Button.topRight)
        {
            lionTamer.OnRightUpPressed();
        }
        else if (button == Button.bottomRight)
        {
            lionTamer.OnRightDownPressed();
        }
    }
}
