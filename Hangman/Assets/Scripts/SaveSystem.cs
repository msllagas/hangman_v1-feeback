using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    // Takes a parameter of type StatsData to save the stats on the "stats.save" file    
    public static void SaveStats ( StatsData stats )
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/stats.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        StatsData data = stats;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    /* Takes a parameter of type Stats which initially save it into a file "stats.save" and its location varies on the device

       Android: exact location of this directory can vary depending on the device and Android version.

       Windows: C:\Users\<UserName>\AppData\LocalLow\<CompanyName>\<ProductName>\

           Here, <UserName> is the name of the user account, <CompanyName> is the name of the company specified 
           in the Unity project settings, and <ProductName> is the name of the product specified in the Unity project settings.
    */
    public static void InitSave(Stats stats)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/stats.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        StatsData data = new StatsData(stats);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    // Load the stats file from the specified location and returns a type StatsData, otherwise returns null
    public static StatsData LoadStats()
    {
        // Define the path to the saved file
        string path = Application.persistentDataPath + "/stats.save";
        // Check if the file exists
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            StatsData data = formatter.Deserialize(stream) as StatsData;

            // Check if the bgUnlocked array is null and initialize it if it is
            if (data.bgUnlocked == null)
                data.bgUnlocked = new bool[11] { true, false, false, false, false, false, false, false, false, false, false };

            // Check if the isApplied array is null and initialize it if it is
            if (data.isApplied == null)
                data.isApplied = new bool[11] { true, false, false, false, false, false, false, false, false, false, false };

            // Check if the isNewPlayer value is null and initialize it if it is
            if (data.isNewPlayer == null)
                data.isNewPlayer = true;

            // Check if the points value is 0 and if it is then give it a value of 1000
            if (data.points == 0)
            {
                data.points = 1000;
            }
            stream.Close();
            // Return the deserialized data
            return data;
        }
        else
        {
            // Return null if the file does not exist
            return null;
        }
    }

}
