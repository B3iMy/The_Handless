using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.CloudSave.Models.Data.Player;
using SaveOptions = Unity.Services.CloudSave.Models.Data.Player.SaveOptions;

public class CloudSaveScript : MonoBehaviour
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
            cancel.onClick.AddListener(Cancel);
            load.onClick.AddListener(Load);
            Text.text = "Initialized and signed in anonymously";
        }
        catch (System.Exception ex)
        {
            Text.text = $"Initialization failed: {ex.Message}";
        }
    }

    public async void Save()
    {
        if (!isInitialized)
        {
            Text.text = "Not initialized";
            return;
        }

        if (string.IsNullOrEmpty(Input.text))
        {
            Text.text = "Input is empty";
            return;
        }

        save.interactable = false;

        try
        {
            for (int i = 0; i < 10; i++)
            {
                var data = new Dictionary<string, object> { { i.ToString(), Input.text } };
                await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            }
            Text.text = "Saved";
            Load();
        }
        catch (System.Exception ex)
        {
            Text.text = $"Save failed: {ex.Message}";
        }
        finally
        {
            save.interactable = true;
        }
    }

    public async void Load()
    {
        if (!isInitialized)
        {
            Text.text = "Not initialized";
            return;
        }

        try
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            for (int i = 0; i < 10; i++)
            {
                var result = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { i.ToString() });
                foreach (var kvp in result)
                {
                    if (!data.ContainsKey(kvp.Key))
                    {
                        data[kvp.Key] = kvp.Value;
                    }
                }
            }

            if (data.Count > 0)
            {
                Text.text = string.Join("\n", data.Values);
            }
            else
            {
                Text.text = "No data found";
            }
        }
        catch (System.Exception ex)
        {
            Text.text = $"Load failed: {ex.Message}";
            Debug.LogException(ex);
            print(ex.Message);
        }
    }

    public async void Cancel()
    {
        Input.text = "";
        Text.text = "Cancelled";

        try
        {
            for (int i = 0; i < 10; i++)
            {
                
                await CloudSaveService.Instance.Data.ForceDeleteAsync(i.ToString());
            }
            Text.text = "Cancelled and data cleared";
        }
        catch (System.Exception ex)
        {
            Text.text = $"Cancellation and clearing data failed: {ex.Message}";
        }
    }

    public async void SaveData()
    {
        var data = new Dictionary<string, object> { { "keyName", "value" } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }
    

    public async void SavePublicData()
    {
        var data = new Dictionary<string, object> { { "keyName", "value" } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data, new SaveOptions(new PublicWriteAccessClassOptions()));
    }
    public async void LoadData()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "keyName" });
        if (playerData.TryGetValue("keyName", out var keyName))
        {
            Debug.Log($"keyName: {keyName.Value.GetAs<string>()}");
        }
    }
    public async void LoadPublicData()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "keyName" }, new LoadOptions(new PublicReadAccessClassOptions()));
        if (playerData.TryGetValue("keyName", out var keyName))
        {
            Debug.Log($"keyName: {keyName.Value.GetAs<string>()}");
        }
    }
    public async void LoadProtectedData()
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "keyName" }, new LoadOptions(new ProtectedReadAccessClassOptions()));
        if (playerData.TryGetValue("keyName", out var keyName))
        {
            Debug.Log($"keyName: {keyName.Value.GetAs<string>()}");
        }
    }
    public async void LoadPublicDataByPlayerId()
    {
        var playerId = "JE1unrWOOzzbIwy3Nl60fRefuiVE";
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "keyName" }, new LoadOptions(new PublicReadAccessClassOptions(playerId)));
        if (playerData.TryGetValue("keyName", out var keyName))
        {
            Debug.Log($"keyName: {keyName.Value.GetAs<string>()}");
        }
    }
    public async void DeleteData()
    {
        await CloudSaveService.Instance.Data.Player.DeleteAsync("key");
    }
    public async void ListKeys()
    {
        var keys = await CloudSaveService.Instance.Data.Player.ListAllKeysAsync();
        for (int i = 0; i < keys.Count; i++)
        {
            Debug.Log(keys[i].Key);
        }
    }

}
