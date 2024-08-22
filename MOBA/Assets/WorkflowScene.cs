using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class CustomBuild
{
    public static void PerformBuild()
    {
        string[] scenes = {
            "Assets/Scenes/SampleScene.unity"
        };

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "Builds/StandaloneWindows64/MyGame.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
