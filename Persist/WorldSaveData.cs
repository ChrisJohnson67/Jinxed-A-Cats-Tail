using Newtonsoft.Json;
using System.Collections.Generic;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class WorldSaveData : SaveData
{
	public const string c_SaveName = "WorldSaveData.dat";

	[JsonProperty]
	private bool m_seenIntro;

	[JsonProperty]
	private List<LevelSaveData> m_levelList = new List<LevelSaveData>();

	[JsonProperty]
	private int m_silverBells;

	[JsonProperty]
	private List<DialogSaveData> m_dialogList = new List<DialogSaveData>();

	[JsonProperty]
	private int m_extraHeartsPurchased;


	public int SilverBells { get { return m_silverBells; } }
	public bool SeenIntro { get { return m_seenIntro; } }
	public int ExtraHeartsPurchased { get { return m_extraHeartsPurchased; } }

	public WorldSaveData() : base()
	{
	}

	public override string GetSaveName()
	{
		return c_FileDelim + c_SaveName;
	}

	public override void InitNewPlayer()
	{

		base.InitNewPlayer();
	}

	public LevelSaveData GetLevelSaveData(int a_levelTID)
	{
		var levelSaveData = m_levelList.Find(x => x.LevelTID == a_levelTID);
		if (levelSaveData == null)
		{
			levelSaveData = new LevelSaveData(a_levelTID);
			m_levelList.Add(levelSaveData);
			Save();
		}
		return levelSaveData;
	}

	public LevelSaveData GetLastLevelCompleted()
	{
		for (int i = m_levelList.Count - 1; i >= 0; i--)
		{
			var levelSaveData = m_levelList[i];
			if (levelSaveData.Completed)
			{
				return levelSaveData;
			}
		}
		return null;
	}

	public void MarkSeenIntro()
	{
		m_seenIntro = true;
		var levelSaveData = GetLevelSaveData(GameManager.Instance.LevelSettings.Levels[0]);
		if (levelSaveData != null)
		{
			levelSaveData.MarkSeenUnlock();
		}
		Save();
	}

	public void CompleteLevel(int a_levelTID)
	{
		var levelSaveData = GetLevelSaveData(a_levelTID);
		if (levelSaveData == null)
		{
			levelSaveData = new LevelSaveData(a_levelTID);
			m_levelList.Add(levelSaveData);
		}
		levelSaveData.CompleteLevel();
		levelSaveData.ClearSavePoint();

		Save();
	}

	public void AddSilverBells(int a_amt, bool a_save = true)
	{
		m_silverBells += a_amt;
		if (a_save)
			Save();
	}

	public void AddSecretTreat(int a_levelTID, int a_treatIndex)
	{
		var levelSaveData = GetLevelSaveData(a_levelTID);
		if (levelSaveData == null)
		{
			levelSaveData = new LevelSaveData(a_levelTID);
			m_levelList.Add(levelSaveData);
		}
		levelSaveData.AddTreat(a_treatIndex);
		Save();
	}

	public int GetTotalSecretTreats()
	{
		int total = 0;
		foreach (var level in m_levelList)
		{
			total += level.TreatsCollected;
		}
		return total;
	}

	public void MarkSeenUnlock(int a_levelTID)
	{
		var levelSaveData = GetLevelSaveData(a_levelTID);
		if (levelSaveData == null)
		{
			levelSaveData = new LevelSaveData(a_levelTID);
			m_levelList.Add(levelSaveData);
		}
		levelSaveData.MarkSeenUnlock();
		Save();
	}

	public void SetLevelSavePoint(int a_levelTID, int a_savePoint)
	{
		var levelSaveData = GetLevelSaveData(a_levelTID);
		if (levelSaveData == null)
		{
			levelSaveData = new LevelSaveData(a_levelTID);
			m_levelList.Add(levelSaveData);
		}
		levelSaveData.SetSavePoint(a_savePoint);
		Save();
	}

	public void ClearLevelSavePoint(int a_levelTID)
	{
		var levelSaveData = GetLevelSaveData(a_levelTID);
		if (levelSaveData == null)
		{
			levelSaveData = new LevelSaveData(a_levelTID);
			m_levelList.Add(levelSaveData);
		}
		levelSaveData.ClearSavePoint();
		Save();
	}

	public int GetLevelSavePoint(int a_levelTID)
	{
		var levelSaveData = GetLevelSaveData(a_levelTID);
		return levelSaveData != null ? levelSaveData.SavePoint : 0;
	}

	public void CompleteDialog(int a_dialogTID)
	{
		var dialogData = GetDialogSaveData(a_dialogTID);
		if (dialogData == null)
		{
			dialogData = new DialogSaveData(a_dialogTID);
		}
		else
		{
			dialogData.MarkDialogComplete();
		}
		Save();
	}

	public void PurchaseHeart()
	{
		int price = GameManager.Instance.LevelSettings.GetHeartPurchasePrice();
		if (price <= m_silverBells)
		{
			m_extraHeartsPurchased++;
			m_silverBells -= price;
		}
		Save();
	}

	public bool HasDialogBeenTriggered(int a_dialogTID)
	{
		var dialogData = GetDialogSaveData(a_dialogTID);
		return dialogData != null;
	}

	public DialogSaveData GetDialogSaveData(int a_dialogTID)
	{
		return m_dialogList.Find(x => x.DialogTID == a_dialogTID);
	}
}
