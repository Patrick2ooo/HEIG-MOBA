using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;


namespace WorkshopScene{
    public static class PreBuildSetup
    {
        public int callbackOrder => 0;

        public static void OnPreprocessBuild(BuildReport report)
        {
            // Ensure scenes are added to the build settings
            // Log build report and scene setup
            System.Console.WriteLine("Running PreBuildSetup. Modifying scenes for build.");

            // Ensure scenes are added to the build settings with MainMenu as the first scene
            EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene("Assets/Scenes/UIs/MainMenu.unity", true),  // MainMenu is the first scene
                new EditorBuildSettingsScene("Assets/Scenes/SampleScene.unity", true)    // SampleScene is the second scene
            };

            EditorBuildSettings.scenes = scenes;

            // Log each scene added to the build settings
            foreach (var scene in scenes)
            {
                System.Console.WriteLine("Scene added to build: " + scene.path);
            }
        }

    }
}
