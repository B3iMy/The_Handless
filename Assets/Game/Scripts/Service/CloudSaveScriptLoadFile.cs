using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class CloudSaveLoadFile : MonoBehaviour
{
    public Text Text;
    public InputField Input;
    public Button save;
    public Button cancel;
    public Button load;

    private bool isInitialized = false;

    public async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            isInitialized = true;
            // Add listeners for the buttons
            save.onClick.AddListener(Save);
            
            load.onClick.AddListener(Load);
            cancel.onClick.AddListener(Delete);
            Text.text = "Initialized and signed in anonymously";
        }
        catch (System.Exception ex)
        {
            Text.text = $"Initialization failed: {ex.Message}";
        }
    }
    // Save file to cloud
    public async void Save()
    {
       
    }
    // Load file from cloud
    public async void Load()
    {
        
    }
    // Delete file from cloud
    public async void Delete()
    {

    }


}
