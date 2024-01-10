using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// https://www.youtube.com/watch?v=XOjd_qU2Ido&t=251s
// This class was created with help from the video above

public static class SaveSystem
{

    static string highscorePath = Application.persistentDataPath + "/highscores.txt"; // Path for highscores
    static string userSettingsPath = Application.persistentDataPath + "/userSettings.txt"; // Path for User Settings

    // Saves highscore list
    public static void saveHighscore(sceneLoader loader)
    {
        // create BinaryFormatter
        BinaryFormatter formatter = new BinaryFormatter();
        // open a new FileStream
        FileStream stream = new FileStream(highscorePath, FileMode.Create);

        // Create highscoreData Object
        highscoreData data = new highscoreData(loader);

        // save data
        formatter.Serialize(stream, data);
        // close filestream
        stream.Close();
    }

    // Loads highscore lists
    public static highscoreData loadHighscore()
    {
        // check if file Exists
        if (File.Exists(highscorePath))
        {
            // create BinaryFormatter
            BinaryFormatter formatter = new BinaryFormatter();
            // open a new FileStream
            FileStream stream = new FileStream(highscorePath, FileMode.Open);

            // get data
            highscoreData data = formatter.Deserialize(stream) as highscoreData;

            // close file stream
            stream.Close();

            return data;
        }
        else
        {
            // return null
            return null;
        }
    }

    // deletes highscore list
    public static void deleteHighScore()
    {
        File.Delete(highscorePath);
    }


    // saves User settings
    public static void saveUserSettings(SettingsMenu settings)
    {
        // create BinaryFormatter
        BinaryFormatter formatter = new BinaryFormatter();
        // open a new FileStream
        FileStream stream = new FileStream(userSettingsPath, FileMode.Create);

        // Create userSettings Object
        userSettings data = new userSettings(settings);

        // save data
        formatter.Serialize(stream, data);
        // close filestream
        stream.Close();
    }

    // create defult user settings
    public static void createDefaultSetting()
    {
        // create BinaryFormatter
        BinaryFormatter formatter = new BinaryFormatter();
        // open a new FileStream
        FileStream stream = new FileStream(userSettingsPath, FileMode.Create);

        // creates defult userSettings file
        userSettings data = new userSettings(1.5f, 1.5f, false, false, 60);

        // save data
        formatter.Serialize(stream, data);
        // close filestream
        stream.Close();
    }

    // loads user settings
    public static userSettings loadUserSettings()
    {
        // check if file Exists
        if (File.Exists(userSettingsPath))
        {
            // create BinaryFormatter
            BinaryFormatter formatter = new BinaryFormatter();
            // open a new FileStream
            FileStream stream = new FileStream(userSettingsPath, FileMode.Open);

            // get data
            userSettings data = formatter.Deserialize(stream) as userSettings;

            // close file stream
            stream.Close();

            return data;
        }
        else
        {
            // return null if file does not exist
            return null;
        }
    }
}
