using UnityEngine;
using System.Text;

public class PlayerIDAndPrivileges : MonoBehaviour
{
    private string playerID;

    // Owner ID - first player
    private const string ownerID = "29GBH23jJ42jlN1X";

    // Flag to determine if this player is the owner
    public bool isOwner { get; private set; }

    void Start()
    {
        InitializePlayerID();
        CheckOwnerPrivileges();
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
            // If no ID exists, assign the special owner ID first
            if (!PlayerPrefs.HasKey("FirstPlayerAssigned"))
            {
                playerID = ownerID;
                PlayerPrefs.SetString("FirstPlayerAssigned", "true");
                Debug.Log("Assigned Owner ID to First Player.");
            }
            else
            {
                playerID = GenerateRandomID(16); // Generate a random ID for other players
                Debug.Log("Generated New Player ID.");
            }
            PlayerPrefs.SetString("PlayerID", playerID);
            PlayerPrefs.Save();
        }
        Debug.Log($"Player ID: {playerID}");
    }

    void CheckOwnerPrivileges()
    {
        // Check if this player's ID matches the owner ID
        if (playerID == ownerID)
        {
            isOwner = true;
            Debug.Log("You are the Owner! Special privileges enabled.");
            EnableOwnerUtilities();
        }
        else
        {
            isOwner = false;
            Debug.Log("You are a Regular Player.");
        }
    }

    void EnableOwnerUtilities()
    {
        // Example: Enable access to specific utilities, rooms, or areas
        Debug.Log("Owner Utilities Enabled: Access to special rooms, tools, and commands.");
        // Add your logic here for enabling utilities or access.
    }

    string GenerateRandomID(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder result = new StringBuilder(length);
        System.Random random = new System.Random();

        for (int i = 0; i < length; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }
        return result.ToString();
    }
}
