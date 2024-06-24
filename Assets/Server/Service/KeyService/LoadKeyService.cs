using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PEMReader : MonoBehaviour
{
    public static string LoadPEMFile(string pemFilePath)
    {
        try
        {
            string pemContent = File.ReadAllText(pemFilePath);
            string publicKey = ExtractPublicKey(pemContent);
            return publicKey;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error reading PEM file: " + ex.Message);
            return null;
        }
    }

    private static string ExtractPublicKey(string pemContent)
    {
        string header = "-----BEGIN PUBLIC KEY-----";
        string footer = "-----END PUBLIC KEY-----";

        int start = pemContent.IndexOf(header, StringComparison.Ordinal);
        int end = pemContent.IndexOf(footer, StringComparison.Ordinal);

        if (start >= 0 && end >= 0)
        {
            start += header.Length;
            string base64 = pemContent.Substring(start, end - start).Replace("\n", "").Replace("\r", "").Trim();
            byte[] keyBytes = Convert.FromBase64String(base64);
            X509Certificate2 cert = new X509Certificate2(keyBytes);
            return cert.GetPublicKeyString();
        }
        else
        {
            Debug.LogError("PEM content does not contain a valid public key.");
            return null;
        }
    }
}
