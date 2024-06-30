using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using Unity.Services.Core;

public class AcceptCodeSevice : MonoBehaviour
{
    public InputField InputCode;
    public Button ExceptCode;
    public Text Result;
    private bool isInitialized = false;
    public HttpClient httpClient;

    public async void Start()
    {
        try
        {

            // Add listener for the login button
            ExceptCode.onClick.AddListener(OnForgotPasswordButtonClick);

            // Instantiate HttpClient (basic instantiation)
            httpClient = new HttpClient();
        }
        catch (Exception ex)
        {
            Result.text = $"Initialization failed: {ex.Message}";
        }
    }

    private async void OnForgotPasswordButtonClick()
    {
        
        /*string usercode = InputCode.text;
        // Assuming LoadUserByIDAsync is an async method on httpClient
        string user = await httpClient.ForgotPasswordAsync(usercode);
        Result.text = "Forgot password successful";
        // Check if HttpClient is initialized
        if (httpClient == null)
        {
            return;
        }*/


    }
}
