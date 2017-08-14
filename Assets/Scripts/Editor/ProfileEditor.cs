using System;
using UnityEditor;
using System.IO;

public class ProfileEditor : Editor
{
    private ProfileEditor profileEditor;

    [MenuItem("MyTools/Profile Editor/Delete Current %#d")]
    static void DeleteCurrent()
    {
        string tmp = StandardPaths.saveDataDirectory + "/save.xml";

        if (File.Exists(tmp))
        {
            using (StreamReader sr = new StreamReader(tmp))
            {
                tmp = sr.ReadToEnd();
            }
        }

        File.Delete(StandardPaths.saveDataDirectory + "/save.xml");
    }

    [MenuItem("MyTools/Profile Editor/Create New %#c")]
    static void CreateNew()
    {
        Profile profile = new Profile
        {
            currentAdventure = "test.json",
            currentNodeId = String.Empty,
            values = new System.Collections.Generic.Dictionary<string, string>()
        };

        if (!Directory.Exists(StandardPaths.saveDataDirectory))
        {
            Directory.CreateDirectory(StandardPaths.saveDataDirectory);
        }

        profile.SaveXml();
    }

    [MenuItem("MyTools/Profile Editor/RESET %#r")]
    static void Reset()
    {
        DeleteCurrent();
        CreateNew();
    }

    [MenuItem("MyTools/Profile Editor/Open saves folder")]
    static void OpenFolder()
    {
        System.Diagnostics.Process.Start("explorer.exe", "/select," + StandardPaths.saveDataDirectory);
    }
}
