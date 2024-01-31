using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class UserSaveData : SaveData
{
	public const string c_SaveName = "UserData.dat";

	[JsonProperty]
	private string m_playerName;

	[JsonProperty]
	private bool m_fullscreen;

	[JsonProperty]
	private Vector2Int m_resolution;

	public string PlayerName { get { return m_playerName; } }
	public bool Fullscreen => m_fullscreen;
	public Vector2Int Resolution => m_resolution;

	public UserSaveData() : base()
	{
	}

	public void SetPlayerName(string a_name)
	{
		m_playerName = a_name;
		Save();
	}

	public override string GetSaveName()
	{
		return c_FileDelim + c_SaveName;
	}

	public override void InitNewPlayer()
	{
		SetPlayerName("Player" + UnityEngine.Random.Range(0, 9999));

		base.InitNewPlayer();
	}

	public void SetResolution(Vector2Int a_res)
	{
		m_fullscreen = Screen.fullScreen;
		m_resolution = a_res;
		Save();
	}

	public void SetFullscreen(bool a_fullscreen)
	{
		m_fullscreen = a_fullscreen;
		Save();
	}
}
