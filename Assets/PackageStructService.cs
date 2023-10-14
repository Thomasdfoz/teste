using PackageManager.Data;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using PackageInfo = PackageManager.Data.PackageInfo;
public class PackageStructService : MonoBehaviour
{
    [MenuItem("Custom/Create Package")]
    public static async void CreateStructToPackage()
    {
        string namePackage = "Teste";
        string moduleFolder = Path.Combine(Application.dataPath, namePackage);

        if (!Directory.Exists(moduleFolder))
        {
            Directory.CreateDirectory(moduleFolder);

            TextAsset gitIgnoreTextAsset = Resources.Load<TextAsset>("gitIgnore");
            if (gitIgnoreTextAsset != null)
            {
                string gitIgnoreContent = gitIgnoreTextAsset.text;
                File.WriteAllText(Path.Combine(moduleFolder, ".gitignore"), gitIgnoreContent);
            }

            string packageFolder = Path.Combine(moduleFolder, "Package");
            Directory.CreateDirectory(packageFolder);

            PackageInfo packageInfo = new PackageInfo
            {
                name = $"com.vrglass.{namePackage.Replace(" ", "").ToLower()}",
                version = "0.0.1",
                displayName = $"{namePackage}",
                description = "...",
                unity = "2021.3",
                unityRelease = "8f1",
                type = "tools",
                dependencies = new Dictionary<string, string>
                {
                },
                author = new AuthorInfo
                {
                    name = "Thomas",
                    email = "Thomas@Example.com",
                    url = "https://www.Thomas.com"
                }
            };

            
            string json = JsonUtility.ToJson(packageInfo);

            File.WriteAllText(Path.Combine(packageFolder, "Package.json"), json);

            string scriptsFolder = Path.Combine(moduleFolder, "Scripts");
            Directory.CreateDirectory(scriptsFolder);

            GitHubUploader gitHub = new GitHubUploader("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", "Thomasdfoz", namePackage, moduleFolder);

            await gitHub.UploadModuleToGitHubAsync();

            Debug.Log("Pacote criado Com Sucesso!!");
        }
        else
        {
            Debug.Log("A pasta já existe: " + moduleFolder);
        }
    }
}
