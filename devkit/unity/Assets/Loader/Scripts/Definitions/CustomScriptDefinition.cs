using UnityEngine;

[System.Serializable]
public class CustomScriptDefinition
{
    public string BundleName;
    public string AssemblyName;
    public string Namespace;
    public string TypeName;

    [Tooltip("This data is copied to the script. Use it for tranform refrences, prefabs, whatever. It pretty much just works.")]
    public GameObject[] ProvidedData;
    [Tooltip("This data is copied to the script.")]
    public float[] FloatParameters;
    [Tooltip("This data is copied to the script.")]
    public int[] IntParameters;
}
