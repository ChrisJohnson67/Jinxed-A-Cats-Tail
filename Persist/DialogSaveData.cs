using Newtonsoft.Json;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class DialogSaveData
{

	[JsonProperty]
	private int m_dialogTID;

	[JsonProperty]
	private int m_triggeredCount = 0;

	public int DialogTID => m_dialogTID;
	public int TriggeredCount => m_triggeredCount;

	public DialogSaveData()
	{
	}

	public DialogSaveData(int a_dialogTID)
	{
		m_dialogTID = a_dialogTID;
		m_triggeredCount = 1;
	}

	public void MarkDialogComplete()
	{
		m_triggeredCount++;
	}
}