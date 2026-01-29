using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;

public static class DataCrypto
{
    private static string secretKey = "MyGameSecret_99";

    public static string GenerateHash(string data)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data + secretKey));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));
            return builder.ToString();
        }
    }

    public static void SaveSecureInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.SetString(key + "_hash", GenerateHash(value.ToString()));
    }

    public static int GetSecureInt(string key, int defaultValue)
    {
        if (!PlayerPrefs.HasKey(key)) return defaultValue;

        int value = PlayerPrefs.GetInt(key, defaultValue);

        if (!PlayerPrefs.HasKey(key + "_hash"))
        {
            SaveSecureInt(key, value);
            return value;
        }

        string savedHash = PlayerPrefs.GetString(key + "_hash", "");
        string currentHash = GenerateHash(value.ToString());

        if (savedHash == currentHash) return value;
        Debug.LogError("ВНИМАНИЕ: Данные ключа " + key + " были изменены или повреждены!");
        SaveSecureInt(key, 0);
        return 0;
    }
}