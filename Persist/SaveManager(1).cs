using Newtonsoft.Json;
using System;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
	public static Action OnDataReset;

	[JsonProperty]
	private CurrencySaveData m_currencySaveData;

	[JsonProperty]
	private WorldSaveData m_worldSaveData;

	[JsonProperty]
	private UserSaveData m_userSaveData;

	private JsonSerializerSettings m_jsonSettings;


	public CurrencySaveData CurrencySaveData { get { return m_currencySaveData; } }
	public WorldSaveData WorldSaveData { get { return m_worldSaveData; } }
	public UserSaveData UserSaveData { get { return m_userSaveData; } }
	public JsonSerializerSettings JSONSettings { get { return m_jsonSettings; } }

	public SaveManager()
	{
		m_jsonSettings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.All
		};
	}

	public void Load()
	{
		var path = Application.persistentDataPath + SaveData.c_FileDelim;
		m_currencySaveData = SaveData.Load<CurrencySaveData>(path + CurrencySaveData.c_SaveName);
		m_worldSaveData = SaveData.Load<WorldSaveData>(path + WorldSaveData.c_SaveName);
		m_userSaveData = SaveData.Load<UserSaveData>(path + UserSaveData.c_SaveName);
	}

	public void Save()
	{
		m_currencySaveData.Save();
		m_userSaveData.Save();
		m_worldSaveData.Save();
	}

	public void ResetSaveData()
	{
		m_currencySaveData = new CurrencySaveData();
		m_currencySaveData.InitNewPlayer();
		m_userSaveData = new UserSaveData();
		m_userSaveData.InitNewPlayer();
		m_worldSaveData = new WorldSaveData();
		m_worldSaveData.InitNewPlayer();
		Save();

		OnDataReset?.Invoke();
	}
}
