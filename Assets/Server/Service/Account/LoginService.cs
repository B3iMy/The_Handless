using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using Unity.Services.Core;

public class LoginService : MonoBehaviour
{
    public InputField InputName;
    public InputField InputPassword;
    public Button login;
    public Text Result;
    private bool isInitialized = false;
    public HttpClient httpClient;

    public async void Start()
    {
        try
        {

            // Add listener for the login button
            login.onClick.AddListener(OnLoginButtonClick);

            // Instantiate HttpClient (basic instantiation)
            httpClient = new HttpClient();
        }
        catch (Exception ex)
        {
            Result.text = $"Initialization failed: {ex.Message}";
        }
    }

    private async void OnLoginButtonClick()
    {
        string username_ = InputName.text;
        string password_ = InputPassword.text;
        Debug.Log($"Logging in with username: {username_} and password: {password_}");
        // Assuming LoadUserByIDAsync is an async method on httpClient
        string user = await httpClient.OnLoginAsync(InputName.text, InputPassword.text);
        Result.text = "Login successful";
        // Check if HttpClient is initialized
        if (httpClient == null)
        {
            return;
        }

        

    }
}
