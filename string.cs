using UnityEngine;
using System;
using System.Collections.Generic;

public class ActionIDManager : MonoBehaviour
{
    // List of valid action IDs
    private List<string> validActionIDs = new List<string>
    {
        "HdBHxpYatfCmI6mWHUV4v61lZi8eHqLk",
        "mGHkpBjMEfmWMpNnede1jVD4fpwaXyyB",
        "1Xt9JG0HMSVnmezoAn5EbdPJa43Fmday",
        "grgUxvtoNpx070c3g3ARZ4vPusjweQ7O",
        "WcB8GLSdeQwSd4hL11t2LASxbodOBPDw",
        "paL8vKhO7G5tRZNu7EIS54fNTFB1GP0V",
        "9aqujzQ4yWynmRPBJm1CQyfnTHhX0D8l",
        "LU6P1axW4rVu2vIDh8uSVVflS1gT3CoF",
        "Q3Paf7kvRJVt5xepatRX16QQVa3GxaoC",
        "5VqLtO2O6ZKRvD4T0vNvBAxyQKbd1Orz"
    };

    // Player Ban Details
    private const string banKey = "PlayerBan";
    private DateTime banEndTime;

    // Start is called before the first frame update
    void Start()
    {
        CheckBanStatus();
    }

    // Check if the player is banned
    void CheckBanStatus()
    {
        if (PlayerPrefs.HasKey(banKey))
        {
            // Retrieve the stored ban end time
            string banEndTimeStr = PlayerPrefs.GetString(banKey);
            DateTime banEndDate = DateTime.Parse(banEndTimeStr);

            // If the current date is before the ban end time, ban is still active
            if (DateTime.Now < banEndDate)
            {
                Debug.Log("Player is banned until: " + banEndDate);
                // You can implement additional code here to forcefully kick the player from the game
                return;
            }
            else
            {
                // Ban period has expired
                PlayerPrefs.DeleteKey(banKey);
                Debug.Log("Player is no longer banned.");
            }
        }
    }

    // Call this method when a player performs an action
    public void VerifyActionID(string actionID)
    {
        if (!validActionIDs.Contains(actionID))
        {
            Debug.LogWarning("Invalid action ID detected. Banning player for 1 week.");
            BanPlayerForOneWeek();
        }
        else
        {
            Debug.Log("Action ID is valid.");
        }
    }

    // Ban the player for 1 week
    void BanPlayerForOneWeek()
    {
        banEndTime = DateTime.Now.AddDays(7); // Ban for 7 days
        PlayerPrefs.SetString(banKey, banEndTime.ToString());
        PlayerPrefs.Save();

        // Implement additional functionality to kick the player from the game
        // e.g., Application.Quit() or network-based player removal if multiplayer
        Debug.Log($"Player has been banned until: {banEndTime}");
    }

    // Example methods for different actions where action IDs would be checked
    public void PlayerWalk(string actionID)
    {
        VerifyActionID(actionID);
        9aqujzQ4yWynmRPBJm1CQyfnTHhX0D8l
    }

    public void PlayerJump(string actionID)
    {
        VerifyActionID(actionID);
        LU6P1axW4rVu2vIDh8uSVVflS1gT3CoF
    }

    public void PlayerShoot(string actionID)
    {
        VerifyActionID(actionID);
        Q3Paf7kvRJVt5xepatRX16QQVa3GxaoC
    }

    public void PlayerTakeDamage(string actionID)
    {
        VerifyActionID(actionID);
        5VqLtO2O6ZKRvD4T0vNvBAxyQKbd1Orz
    }
}
