using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.UIElements;

public static class CharacterClassLoader
{ 
    public static Dictionary<string, CharacterClassData> GetClasses()
    {
        TextAsset classJson = Resources.Load<TextAsset>("classes");
        //Testing if the classJson is empty
        if (classJson == null)
        {
            Debug.LogError("Could not find classes.json in Resources folder.");
            return new Dictionary<string, CharacterClassData>();
        }

        return JsonConvert.DeserializeObject<Dictionary<string, CharacterClassData>>(classJson.text);
    }
}
