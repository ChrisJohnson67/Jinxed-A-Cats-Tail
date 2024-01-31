using Newtonsoft.Json;
using System.Collections.Generic;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class LevelSaveData
{

	[JsonProperty]
	private int m_levelTID;

	[JsonProperty]
	private List<int> m_treatsCollectedIndexes = new List<int>();

	[JsonProperty]
	private bool m_complete;

	[JsonProperty]
	private int m_savePoint;

	[JsonProperty]
	private bool m_seenUnlock;

	public int LevelTID { get { return m_levelTID; } }
	public int TreatsCollected { get { return m_treatsCollectedIndexes.Count; } }
	public bool Completed { get { return m_complete; } }
	public int SavePoint => m_savePoint;
	public bool SeenUnlock => m_seenUnlock;

	public LevelSaveData()
	{
	}

	public LevelSaveData(int a_levelTID)
	{
		m_levelTID = a_levelTID;

	}

	public void CompleteLevel()
	{
		m_complete = true;
	}

	public void MarkSeenUnlock()
	{
		m_seenUnlock = true;
	}

	public bool HasTreatBeenCollected(int a_index)
	{
		return m_treatsCollectedIndexes.Contains(a_index);
	}

	public bool AddTreat(int a_index)
	{
		if (!m_treatsCollectedIndexes.Contains(a_index))
		{
			m_treatsCollectedIndexes.Add(a_index);
			return true;
		}
		return false;
	}

	public void SetSavePoint(int a_point)
	{
		m_savePoint = a_point;
	}

	public void ClearSavePoint()
	{
		m_savePoint = 0;
	}
}