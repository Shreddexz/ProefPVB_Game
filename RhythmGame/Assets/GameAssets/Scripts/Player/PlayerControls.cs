using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerControls", menuName = "ScriptableObjects/Create PlayerControls Asset")]
public class PlayerControls : ScriptableObject
{
    public KeyCode button1, button2, button3, button4, button5, button6;
    public KeyCode joyL, joyR, joyU, joyD;
}