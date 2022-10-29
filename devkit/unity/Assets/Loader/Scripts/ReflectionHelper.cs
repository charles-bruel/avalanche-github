using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ReflectionHelper
{
    public static Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();

    public static T GetInstance<T>(CustomScriptDefinition customScriptDefinition) where T : class
    {
        try
        {
            if (customScriptDefinition == null || customScriptDefinition.BundleName == "" || customScriptDefinition.AssemblyName == "" || customScriptDefinition.Namespace == "" || customScriptDefinition.TypeName == "")
            {
                Type typeParameter = typeof(T);
                return typeParameter.GetConstructor(new Type[0]).Invoke(null, null) as T;
            }
            else
            {
                Assembly assembly = LoadIfNotLoaded(customScriptDefinition.BundleName, customScriptDefinition.AssemblyName);
                Type typeParameter = assembly.GetType(customScriptDefinition.Namespace + "." + customScriptDefinition.TypeName);
                return typeParameter.GetConstructor(new Type[0]).Invoke(null, null) as T;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
        }
        return null;
    }

    public static Assembly LoadIfNotLoaded(string bundleName, string name)
    {
        if (assemblies.ContainsKey(name))
        {
            return assemblies[name];
        }
        else
        {
            AssetBundle bundle = BundleHelper.bundles[bundleName];
            TextAsset assemblyAsset = bundle.LoadAsset(name) as TextAsset;
            Assembly assembly = Assembly.Load(assemblyAsset.bytes);
            return assembly;
        }
    }
}
