using PackageManager.Data;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using PackageInfo = PackageManager.Data.PackageInfo;
public class PackageStructService : MonoBehaviour
{
    public static async void CreateStructToPackage(string packageName)
    {
        string moduleFolder = Path.Combine(Application.dataPath, packageName);

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
                name = $"com.vrglass.{packageName.Replace(" ", "").ToLower()}",
                version = "0.0.1",
                displayName = $"{packageName}",
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

            GitHubUploader gitHub = new GitHubUploader("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", "Thomasdfoz", packageName, moduleFolder);

            await gitHub.UploadModuleToGitHubAsync();

            Debug.Log("Pacote criado Com Sucesso!!");
        }
        else
        {
            Debug.Log("A pasta já existe: " + moduleFolder);
        }
    }
}
