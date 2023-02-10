using Firebase.Database;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{

    private string userID;
    private DatabaseReference dbReference;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the deviceuniqueIdentifier from the users device and use it as a unique userID for the realtime database
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        CreateUser();
    }

    // Create a new user data into the firebase realtime database
    public void CreateUser()
    {
        StatsData statsList = SaveSystem.LoadStats();
        float motLevPerc = (statsList.motivationLevel / 3) * 100;
        float aveMLPerc = (statsList.centralTend / 3) * 100;
        Player newPlayer = new Player(statsList.fullname, statsList.motivationLevel, statsList.centralTend, motLevPerc, aveMLPerc);

        // Convert the newPlayer data into JSON(JavaScript Object Notation)
        string json = JsonUtility.ToJson(newPlayer);

        dbReference.Child("players").Child(userID).SetRawJsonValueAsync(json);
    }
}
