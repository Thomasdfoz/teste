using UnityEditor;
using UnityEngine;

public class PackageCreator : EditorWindow
{
    private string inputText = "";

    [MenuItem("Package Manager/New Package")]
    public static void ShowWindow()
    {
        GetWindow<PackageCreator>("Package Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Package Name");

        inputText = EditorGUILayout.TextField(inputText);

        if (GUILayout.Button("Create Package"))
        {
            PackageStructService.CreateStructToPackage(inputText);
        }
    }
}
