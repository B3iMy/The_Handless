using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using System.IO;

public class CloudSaveLoadFile : MonoBehaviour
{
    //public Text Text;
    //public InputField Input;
    //public Button save;
    //public Button cancel;
    //public Button load;

    //private bool isInitialized = false;

    //public async void Start()
    //{
    //    try
    //    {
    //        await UnityServices.InitializeAsync();
    //        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    //        isInitialized = true;
    //        // Add listeners for the buttons
    //        save.onClick.AddListener(SavePlayerFile);
            
    //        load.onClick.AddListener(ListPlayerFiles);
    //        cancel.onClick.AddListener(DeletePlayerFile);
    //        Text.text = "Initialized and signed in anonymously";
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Text.text = $"Initialization failed: {ex.Message}";
    //    }
    //}
    //// Save file to cloud
    //public async void SavePlayerFile()
    //{
    //    byte[] file = System.IO.File.ReadAllBytes("fileName.txt");
    //    await CloudSaveService.Instance.Files.Player.SaveAsync("fileName", file);
    //}
    //// list file from cloud
    //public async void ListPlayerFiles()
    //{
    //    // List<string> files = await CloudSaveService.Instance.Files.Player.ListAllAsync();

    //    for (int i = 0; i < files.Count; i++)
    //    {
    //        Debug.Log(files[i]);
    //    }
    //}
    //// Delete file from cloud
    //public async void DeletePlayerFile()
    //{
    //    await CloudSaveService.Instance.Files.Player.DeleteAsync("fileName");
    //}
    //public async void GetPlayerFileAsByteArray()
    //{
    //    byte[] file = await CloudSaveService.Instance.Files.Player.LoadBytesAsync("fileName");
    //}
    //public async void GetPlayerFileAsStream()
    //{
    //    Stream file = await CloudSaveService.Instance.Files.Player.LoadStreamAsync("fileName");
    //}
    //public async void GetPlayerFileMetadata()
    //{
    //    var metadata = await CloudSaveService.Instance.Files.Player.GetMetadataAsync("fileName");
    //    Debug.Log(metadata.Key);
    //    Debug.Log(metadata.Size);
    //    Debug.Log(metadata.ContentType);
    //    Debug.Log(metadata.Created);
    //    Debug.Log(metadata.WriteLock);
    //}
}
