using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLiftTurnSectionDefinition : MonoBehaviour
{
    [Header("Transforms Linking")]
    public Transform RightCablePoint;
    public Transform LeftCablePoint;
    [Tooltip("What is passed to the wheel fetcher, so for example, auto generated wheels point correctly")]
    public Transform CableAimingPoint;
    public float MaxAngle;

    [Header("Custom Scripts")]
    public CustomScriptDefinition CablePathScript;

}
