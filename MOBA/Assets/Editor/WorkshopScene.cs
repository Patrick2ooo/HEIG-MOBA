using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PreBuildSetup : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        // Ensure scenes are added to the build settings with MainMenu as the first scene
        EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[]
        {
            new EditorBuildSettingsScene("Assets/Scenes/UIs/MainMenu.unity", true),  // MainMenu is the first scene
            new EditorBuildSettingsScene("Assets/Scenes/SampleScene.unity", true)    // SampleScene is the second scene
        };

        EditorBuildSettings.scenes = scenes;
    }

}
