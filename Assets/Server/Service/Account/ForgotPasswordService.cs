using System;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;

public class ForgotPasswordService : MonoBehaviour
{
    public InputField InputName;
    public InputField InputEmail;
    public Button ForgotPassword;
    public Text Result;
    public HttpClient httpClient;

    public async void Start()
    {
        try
        {
            // Add listener for the login button
            if (ForgotPassword != null)
            {
                ForgotPassword.onClick.AddListener(OnForgotPasswordButtonClick);
            }
            else
            {
                Debug.LogError("ForgotPassword button is not assigned.");
                Result.text = "ForgotPassword button is not assigned.";
                return;
            }

            // Instantiate HttpClient
            httpClient = new HttpClient();
        }
        catch (Exception ex)
        {
            if (Result != null)
            {
                Result.text = $"Initialization failed: {ex.Message}";
            }
            else
            {
                Debug.LogError($"Initialization failed: {ex.Message}");
            }
        }
    }

    private async void OnForgotPasswordButtonClick()
    {

        string fieldName = InputName.text;
        string fieldEmail = InputEmail.text;

        try
        {
            // Assuming ForgotPasswordAsync is an async method on httpClient
            string user = await httpClient.ForgotPasswordAsync(fieldName, fieldEmail);
            Result.text = "Forgot password successful";
        }
        catch (Exception ex)
        {
            Result.text = $"Forgot password failed: {ex.Message}";
        }
    }
}
