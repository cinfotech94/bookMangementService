﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuthManagementService.Service.GenericServices.Interface;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace UserAuthManagementService.Service.GenericServices.Implemetation;
public class EncryptionService: IEncryptionService
{
    private readonly ILoggingService _loggingService;
    private readonly string _baseUrl;
    private readonly string _hashPath;
    private readonly string _unhashPath;
    private readonly string _encryptionPath;
    private readonly string _token;
    private readonly IRestHelper _restHelper;
    private readonly IConfiguration _configuration;
    public EncryptionService(ILoggingService loggingService, IRestHelper restHelper, IConfiguration configuration)
    {
        _loggingService = loggingService;
        _restHelper = restHelper;

    }

        // Encrypts a string using a key
        public string Encrypt(string dataToEncrypt, string key)
        {
            using (Aes aes = Aes.Create())
            {
                // Set the key and initialization vector (IV) based on the provided key
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                Array.Resize(ref keyBytes, 32); // AES requires a 256-bit key length

                aes.Key = keyBytes;
                aes.GenerateIV();

                // Create the encryptor
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    // Write the IV to the beginning of the stream (needed for decryption)
                    ms.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] dataBytes = Encoding.UTF8.GetBytes(dataToEncrypt);
                        cs.Write(dataBytes, 0, dataBytes.Length);
                    }

                    // Return the encrypted data as a Base64-encoded string
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        // Decrypts a string using a key
        public string Decrypt(string dataToDecrypt, string key)
        {
        string result = "";
        try
        {
            byte[] dataBytes = Convert.FromBase64String(dataToDecrypt);
            using (Aes aes = Aes.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                Array.Resize(ref keyBytes, 32);

                aes.Key = keyBytes;

                // Extract the IV from the encrypted data
                byte[] iv = new byte[aes.BlockSize / 8];
                Array.Copy(dataBytes, iv, iv.Length);
                aes.IV = iv;

                // Create the decryptor
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(dataBytes, iv.Length, dataBytes.Length - iv.Length))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader reader = new StreamReader(cs))
                {
                    // Return the decrypted data as a string
                    result= reader.ReadToEnd();
                }
            }
        }
        catch(Exception ex)
        {
            result= "";
        }
        return result;
    }
    public (string, Exception) Maskpan(string data, string caller, string correltionId)
    {
        try
        {
            if (data != "" || data.Length > 10)
            {
                data = data.Substring(0, 6) + "****".PadLeft(data.Length - 10, '*') + data.Substring(data.Length - 4, 4);
            }
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex.ToString(),nameof(Maskpan),ex, caller,correltionId);
            return ("0", ex);
        }
        return (data, null);
    }
}

