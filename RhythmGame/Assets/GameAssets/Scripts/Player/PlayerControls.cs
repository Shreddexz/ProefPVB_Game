using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class creates an asset with playercontrols.
/// This in turn makes it easy to reference inputs without hardcoded input values, and also makes it easy to change the controls from inside the editor.
/// </summary>
[CreateAssetMenu(fileName = "PlayerControls", menuName = "ScriptableObjects/Create PlayerControls Asset")]
public class PlayerControls : ScriptableObject
{
    public KeyCode button1, button2, button3, button4, button5, button6;
    public KeyCode joyL, joyR, joyU, joyD;
}