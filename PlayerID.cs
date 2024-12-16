using UnityEngine;
using System.Text;

public class PlayerIDGenerator : MonoBehaviour
{
    private string playerID;

    void Start()
    {
        InitializePlayerID();
    }

    void InitializePlayerID()
    {
        // Check if the ID already exists in PlayerPrefs
        if (PlayerPrefs.HasKey("PlayerID"))
        {
            playerID = PlayerPrefs.GetString("PlayerID");
            Debug.Log($"Loaded Player ID: {playerID}");
        }
        else
        {
            playerID = GenerateRandomID(16); // Generate a 16-character random ID
            PlayerPrefs.SetString("PlayerID", playerID);
            PlayerPrefs.Save();
            Debug.Log($"Generated New Player ID: {playerID}");
        }
    }

    string GenerateRandomID(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-=_+[]{}\|;:'",.<>/?`~";
        StringBuilder result = new StringBuilder(length);
        System.Random random = new System.Random();

        for (int i = 0; i < length; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }
        return result.ToString();
    }
}
