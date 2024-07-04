using System;
using UnityEngine;

[Serializable]
public class DataUser
{
    // Public fields for serialization
    public string user_id;
    public string username;
    public string password;
    public string email;

    // Constructor
    public DataUser(string id, string name, string pass, string email)
    {
        user_id = id;
        username = name;
        password = pass;
        this.email = email;
    }
    
    public DataUser() { }

    // Getters and Setters for user_id
    public string GetUserID()
    {
        return user_id;
    }

    public void SetUserID(string id)
    {
        user_id = id;
    }

    // Getters and Setters for username
    public string GetUsername()
    {
        return username;
    }

    public void SetUsername(string name)
    {
        username = name;
    }

    // Getters and Setters for password
    public string GetPassword()
    {
        return password;
    }

    public void SetPassword(string pass)
    {
        password = pass;
    }

    // Get user email
    public string GetEmail()
    {
        return email;
    }

    // Set user email
    public void SetEmail(string email)
    {
        this.email = email;
    }
}
