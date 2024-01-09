using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// https://www.youtube.com/watch?v=XOjd_qU2Ido&t=251s

public static class SaveSystem 
{
    public static void saveHighscore(sceneLoader loader)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/highscores.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        highscoreData data = new highscoreData(loader);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static highscoreData loadHighscore()
    {
        string path = Application.persistentDataPath + "/highscores.txt";
        if (File.Exists(path)) 
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            highscoreData data = formatter.Deserialize(stream) as highscoreData;

            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}
