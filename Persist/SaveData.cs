using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public abstract class SaveData
{
	public const string c_FileDelim = "/";

    public SaveData()
    {
    }

    public abstract string GetSaveName();

    public virtual void InitNewPlayer()
    {
        Save();
    }

    public virtual void ValidateData()
    {

    }

    public static T Load<T>(string a_saveName) where T : SaveData
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        bool fileCreated = false;
        if (!File.Exists(a_saveName))
        {
            var fs = File.Create(a_saveName);
            fs.Close();
            fileCreated = true;
        }

        if (!fileCreated)
        {
            var saveData = JsonConvert.DeserializeObject<T>(File.ReadAllText(a_saveName), SaveManager.Instance().JSONSettings);
            if (saveData != null)
            {
                return saveData;
            }
        }

        T data = Activator.CreateInstance<T>();
        data.InitNewPlayer();
        return data;
    }

    public virtual void Save()
    {
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }
        string output = JsonConvert.SerializeObject(this, Formatting.Indented, SaveManager.Instance().JSONSettings);
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + GetSaveName());

        sw.Write(output);
        sw.Close();
    }
}