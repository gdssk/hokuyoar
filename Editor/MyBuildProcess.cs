using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class MyBuildProcess : MonoBehaviour
{
    public static object PlistReader { get; private set; }

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            var plistPath = pathToBuiltProject + "/Info.plist";
            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            var rootDict = plist.root;
            var dict = rootDict.CreateDict("NSAppTransportSecurity");
            dict.SetBoolean("NSAllowsArbitraryLoads", true);
            Debug.Log("[NSAppTransportSecurity] added");

            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
