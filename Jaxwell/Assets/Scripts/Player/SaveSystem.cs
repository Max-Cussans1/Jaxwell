using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(PlayerState player)
    {
        //use binary formatters for saving since it's difficult to edit
        BinaryFormatter formatter = new BinaryFormatter();
        //unity function that gets a persistent data path regardless of OS, create save and file extension
        string path = Application.persistentDataPath + "/player.jxw";

        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            Debug.Log("Overwritten a save file in " + path);

            PlayerData playerData = new PlayerData(player);
            Debug.Log("Saved scene: " + playerData.sceneName);
            Debug.Log("Saved element: " + playerData.element);
            Debug.Log("Saved position: " + playerData.position[0] + ", " + playerData.position[1] + ", " + playerData.position[2]);

            //write to the file
            formatter.Serialize(stream, playerData);
            stream.Close();
        }
        else
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            Debug.Log("Created a new save file in " + path);

            PlayerData playerData = new PlayerData(player);

            //write to the file
            formatter.Serialize(stream, playerData);
            stream.Close();
        }
    }

    public static PlayerData Load()
    {
        string path = Application.persistentDataPath + "/player.jxw";

        if(File.Exists(path))
        {
            Debug.Log("Loading data from " + path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            //format the binary data from the save file so it can be accessed as PlayerData
            PlayerData loadedData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return loadedData;
        }
        else
        {
            Debug.Log("No save file found in " + path + ", loading a new game");
            return null;
        }
    }
}
