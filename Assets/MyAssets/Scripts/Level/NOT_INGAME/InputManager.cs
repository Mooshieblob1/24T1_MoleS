using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class InputManager : MonoBehaviour
{
    [TitleGroup("PLAYER CONTROLLER")]

    [FoldoutGroup("PLAYER CONTROLLER/PLAYER1", expanded: false)]
    [TitleGroup("PLAYER CONTROLLER/PLAYER1/P1 CONTROLLER")]
    public KeyCode 
    p1_Left,
    p1_Right,
    p1_Up,
    p1_Down,
    p1_Accept,
    p1_Back;

    [FoldoutGroup("PLAYER CONTROLLER/PLAYER2", expanded: false)]
    [TitleGroup("PLAYER CONTROLLER/PLAYER2/P2 CONTROLLER")]
    public KeyCode
    p2_Left,
    p2_Right,
    p2_Up,
    p2_Down,
    p2_Accept,
    p2_Back;

    [FoldoutGroup("PLAYER CONTROLLER/PLAYER3", expanded: false)]
    [TitleGroup("PLAYER CONTROLLER/PLAYER3/P3 CONTROLLER")]
    public KeyCode
    p3_Left,
    p3_Right,
    p3_Up,
    p3_Down,
    p3_Accept,
    p3_Back;

    [FoldoutGroup("PLAYER CONTROLLER/PLAYER4", expanded: false)]
    [TitleGroup("PLAYER CONTROLLER/PLAYER4/P4 CONTROLLER")]
    public KeyCode
    p4_Left,
    p4_Right,
    p4_Up,
    p4_Down,
    p4_Accept,
    p4_Back;
}
