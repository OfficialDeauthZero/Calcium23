using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

public class MultiplayerIDManager : MonoBehaviour
{
    // Player ID variables
    private string playerID;
    private const string ownerID = "29GBH23jJ42jlN1X"; // Fixed owner ID
    public bool isOwner { get; private set; }

    // UI Components
    public Button checkIDButton; // Button to check ID
    public Text playerStatusText; // UI Text to display player status

    // Multiplayer Rooms
    private List<string> multiplayerRooms = new List<string>() { "Room1", "Room2", "Room3" };
    private string currentRoom;

    // Utilities for Owner
    public GameObject banHammerPrefab;
    public GameObject kickHammerPrefab;
    public GameObject ownerGunPrefab;

    void Start()
    {
        InitializePlayerID();
        CheckOwnerPrivileges();

        // Add a button listener for ID checks
        if (checkIDButton != null)
            checkIDButton.onClick.AddListener(CheckPlayerID);

        // Assign the player to a random multiplayer room
        AssignToRandomRoom();

        // Set player utilities if the player is the owner
        if (isOwner)
        {
            SpawnOwnerTools();
        }
    }

    void InitializePlayerID()
    {
        if (PlayerPrefs.HasKey("PlayerID"))
        {
            playerID = PlayerPrefs.GetString("PlayerID");
        }
        else
        {
            playerID = GenerateRandomID(16);
            PlayerPrefs.SetString("PlayerID", playerID);
            PlayerPrefs.Save();
        }
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

    void CheckOwnerPrivileges()
    {
        // Verify if this player is the owner
        isOwner = playerID == ownerID;

        if (playerStatusText != null)
        {
            if (isOwner)
                playerStatusText.text = "Status: Owner";
            else
                playerStatusText.text = "Status: Regular Player";
        }

        Debug.Log(isOwner ? "Owner Privileges Enabled" : "Regular Player Privileges");
    }

    void CheckPlayerID()
    {
        // Ensure player ID is valid (alphanumeric check)
        if (string.IsNullOrEmpty(playerID) || !IsIDValid(playerID))
        {
            Debug.LogWarning("Invalid ID detected. Regenerating ID...");
            playerID = GenerateRandomID(16);
            PlayerPrefs.SetString("PlayerID", playerID);
            PlayerPrefs.Save();
        }

        Debug.Log($"Player ID: {playerID}");
        Debug.Log(isOwner ? "You are the Owner." : "You are a Regular Player.");
    }

    bool IsIDValid(string id)
    {
        foreach (char c in id)
        {
            if (!char.IsLetterOrDigit(c))
                return false;
        }
        return true;
    }

    void AssignToRandomRoom()
    {
        System.Random random = new System.Random();
        currentRoom = multiplayerRooms[random.Next(multiplayerRooms.Count)];
        Debug.Log($"Assigned to Multiplayer Room: {currentRoom}");
    }

    void SpawnOwnerTools()
    {
        // Spawn ban hammer, kick hammer, and owner gun
        if (banHammerPrefab != null)
        {
            Instantiate(banHammerPrefab, transform.position + Vector3.forward, Quaternion.identity);
            Debug.Log("Ban Hammer Spawned");
        }

        if (kickHammerPrefab != null)
        {
            Instantiate(kickHammerPrefab, transform.position + Vector3.forward * 2, Quaternion.identity);
            Debug.Log("Kick Hammer Spawned");
        }

        if (ownerGunPrefab != null)
        {
            Instantiate(ownerGunPrefab, transform.position + Vector3.forward * 3, Quaternion.identity);
            Debug.Log("Owner Gun Spawned");
        }
    }
}
