using UnityEngine;

public class CustomLiftExitStationDefinition : MonoBehaviour
{
    [Header("Transforms Linking")]
    public Transform RightCablePoint;
    public Transform LeftCablePoint;
    public Transform SFXEmitter;
    [Tooltip("What is passed to the wheel fetcher, so for example, auto generated wheels point correctly")]
    public Transform CableAimingPoint;

    [Header("Collision Parameters")]
    public AreaDefinition[] collisions;

    [Header("Nav Area Parameters")]
    [Tooltip("I *think* this is the point that people \"enter\" the building at.")]
    public Transform NavNodeTransform;
    public AreaDefinition navArea;

    [Header("Custom Scripts")]
    public CustomScriptDefinition CablePathScript;
}
