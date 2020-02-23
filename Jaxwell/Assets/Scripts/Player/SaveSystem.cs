using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{

    public static void Save(PlayerState player, string path)
    {
        //use binary formatters for saving since it's difficult to edit
        BinaryFormatter formatter = new BinaryFormatter();

        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            DebugHelper.Log("Overwritten a save file in " + path);

            PlayerData playerData = new PlayerData(player);
            DebugHelper.Log("Saved scene: " + playerData.sceneName);
            DebugHelper.Log("Saved element: " + playerData.element);
            DebugHelper.Log("Saved position: " + playerData.position[0] + ", " + playerData.position[1] + ", " + playerData.position[2]);

            //write to the file
            formatter.Serialize(stream, playerData);
            stream.Close();
        }
        else
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            DebugHelper.Log("Created a new save file in " + path);


            PlayerData playerData = new PlayerData(player);
            DebugHelper.Log("Saved scene: " + playerData.sceneName);
            DebugHelper.Log("Saved element: " + playerData.element);
            DebugHelper.Log("Saved position: " + playerData.position[0] + ", " + playerData.position[1] + ", " + playerData.position[2]);

            //write to the file
            formatter.Serialize(stream, playerData);
            stream.Close();
        }
    }

    public static PlayerData Load(string path)
    {
        if(File.Exists(path))
        {
            DebugHelper.Log("Loading data from " + path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            //format the binary data from the save file so it can be accessed as PlayerData
            PlayerData loadedData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return loadedData;
        }
        else
        {
            DebugHelper.Log("No save file found in " + path);
            return null;
        }
    }
}
