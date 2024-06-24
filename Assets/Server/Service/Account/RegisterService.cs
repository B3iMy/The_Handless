using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.UI;




public class RegisterService : MonoBehaviour
{

    public InputField InputName;
    public InputField InputPassword;
    public InputField InputConfirmPassword;
    public InputField InputEmail;
    public Button register;
    public Button login;
    public Text Result;
    private bool isInitialized = false;
    public HttpClient httpClient;
    // Start is called before the first frame update
    public async void Start()
    {
        try
        {


            // Add listeners for the buttons
            register.onClick.AddListener(RegisterUserData);
            //login.onClick.AddListener(DeleteUserData);




            // Connect to the server
            httpClient.StartServer();
        }
        catch (System.Exception ex)
        {
            // Display error message if initialization fails
        }
    }
    // Register user 
    public void RegisterUserData()
    {
        httpClient.RegisterUserDataAsync(InputName.text, InputPassword.text, InputConfirmPassword.text, InputEmail.text);
    }


}
