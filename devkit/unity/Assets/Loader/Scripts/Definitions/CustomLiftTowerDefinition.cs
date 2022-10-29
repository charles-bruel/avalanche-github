using UnityEngine;

public class CustomLiftTowerDefinition : MonoBehaviour
{
    [Header("Transforms Linking")]
    public Transform RightCablePoint;
    public Transform LeftCablePoint;
    [Tooltip("What is passed to the wheel fetcher, so for example, auto generated wheels point correctly")]
    public Transform CableAimingPoint;

    [Header("Custom Scripts")]
    public CustomScriptDefinition CablePathScript;
    public CustomScriptDefinition TowerPlacementScript;

    [Header("Pretties")]
    public GameObject Padding;

}
