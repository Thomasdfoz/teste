using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GitHubUploader
{
    private readonly string hubToken;
    private readonly string repositoryOwner;
    private readonly string repositoryName;
    private readonly string moduleFolderPath;

    public GitHubUploader(string hubToken, string userName, string moduleName, string moduleFolder) 
    {
        this.hubToken = hubToken;
        repositoryOwner = userName;
        repositoryName = moduleName;
        moduleFolderPath = moduleFolder;
    }

    public async Task UploadModuleToGitHubAsync()
    {
        await CreateGitHubRepositoryAsync();
        await UploadModuleContentsAsync(moduleFolderPath);
    }

    private async Task CreateGitHubRepositoryAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", "token " + hubToken);

            var createRepoRequest = new
            {
                name = repositoryName,
                auto_init = true
            };
            var createRepoJson = Newtonsoft.Json.JsonConvert.SerializeObject(createRepoRequest);
            var content = new StringContent(createRepoJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.github.com/user/repos", content);

            if (response.IsSuccessStatusCode)
            {
                Debug.Log("Repositório criado com sucesso!");
            }
            else
            {
               Debug.Log("Erro ao criar o repositório: " + response);
            }
        }
    }

    private async Task UploadModuleContentsAsync(string folderPath)
    {
        string[] files = Directory.GetFiles(folderPath);

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", "token " + hubToken);

            foreach (var filePath in files)
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                string fileBase64 = Convert.ToBase64String(fileBytes);

                var uploadRequest = new
                {
                    message = "Adicionar arquivo " + Path.GetFileName(filePath),
                    content = fileBase64
                };

                var uploadJson = JsonConvert.SerializeObject(uploadRequest);
                var content = new StringContent(uploadJson, Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"https://api.github.com/repos/{repositoryOwner}/{repositoryName}/contents/{Path.GetFileName(filePath)}", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Arquivo {Path.GetFileName(filePath)} enviado com sucesso!");
                }
                else
                {
                    Console.WriteLine($"Erro ao enviar o arquivo {Path.GetFileName(filePath)}: {response.ReasonPhrase}");
                }
            }
        }
    }
}
