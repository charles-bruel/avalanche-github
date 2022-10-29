using UnityEngine;

public class ResearchAspectReplacement : MonoBehaviour
{
    [Header("ToReplace")]
    public string ID;

    [Header("Details")]
    [Tooltip("Can be left blank, in which case the default name will be used")]
    public string NewName;
    [Tooltip("Can be left blank, in which case the default sprite will be used")]
    public Sprite NewSprite;

}
