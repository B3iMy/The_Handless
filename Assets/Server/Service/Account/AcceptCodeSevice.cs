using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using Unity.Services.Core;

public class AcceptCodeSevice : MonoBehaviour
{
    public InputField InputCode;
    public InputField InputPassword;
    public InputField InputComfirmPassword;
    public Button ExceptCode;
    public Text Result;
    private bool isInitialized = false;
    private HttpClient httpClient;

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

        string usercode = InputCode.text;
        string password = InputPassword.text;
        string confirmPassword = InputComfirmPassword.text;
        if (password == confirmPassword)
        {
            string user = await httpClient.ResetPasswordAsync(usercode, password);
            Result.text = "Forgot password successful";
        }
        else
        {
            Result.text = "Please confirm password not alike ";
        }

        // Assuming LoadUserByIDAsync is an async method on httpClient
        
        
        // Check if HttpClient is initialized
        if (httpClient == null)
        {
            return;
        }


    }
}
