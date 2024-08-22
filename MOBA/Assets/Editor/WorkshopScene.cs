#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class CustomBuild
{
    public static void PerformBuild()
    {
        // Specify the scenes you want to include in the build
        string[] scenes = {
            "Assets/Scenes/SampleScene.unity",  // Adjust the path as per your project structure
        };

        // Configure the build options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "Builds/StandaloneWindows64/MyGame.exe",  // Adjust the path as needed
            target = BuildTarget.StandaloneWindows64,  // Specify the build target
            options = BuildOptions.None
        };

        // Perform the build
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
#endif
