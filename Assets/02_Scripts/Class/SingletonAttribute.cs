using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Field)]
public class SingletonAttribute : Attribute
{
    public Type type;
    public SingletonAttribute(Type type)
    {
        this.type = type;
    }
}
public static class SingletonInjection
{
    static Dictionary<string, object> singletonDic = new();
    static object singleton;
    public static void Injection(object o)
    {
        Type type = o.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Public |
                                            BindingFlags.NonPublic |
                                            BindingFlags.Instance);

        foreach (var one in fields)
        {
            SingletonAttribute attribute = (SingletonAttribute)one.GetCustomAttribute(typeof(SingletonAttribute));

            if (attribute == null) continue;

            Type singletonType = attribute.type;

            if (!singletonDic.TryGetValue(singletonType.Name, out singleton))
                singleton = GameObject.FindFirstObjectByType(singletonType);

            if (singleton == null)
            {
                var property = singletonType.GetProperty("Instance",
                    BindingFlags.Static |
                    BindingFlags.FlattenHierarchy |
                    BindingFlags.Public |
                    BindingFlags.GetProperty);

                singleton = (object)property.GetValue(null, null);
            }

            one.SetValue(o, singleton);

            if (!singletonDic.ContainsKey(singletonType.Name)) singletonDic.Add(singletonType.Name, singleton);
        }
    }
}
