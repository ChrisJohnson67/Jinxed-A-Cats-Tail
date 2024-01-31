using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelSettings")]
public class LevelSettings : TemplateObject
{
	//~~~~~ Definitions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField, TemplateIDField(typeof(BackgroundLevelDisplay), "Background Level Display", "")]
	private int m_backgroundDisplayTID;

	[SerializeField, TemplateIDField(typeof(SpriteRenderer), "Sky Level Display", "")]
	private int m_backgroundSkyDisplayTID;

	[SerializeField, TemplateIDField(typeof(SpriteRenderer), "Underground Level Display", "")]
	private int m_backgroundUndergroundDisplayTID;

	[SerializeField]
	private List<float> m_parallaxValues = new List<float>();

	[SerializeField, TemplateIDField(typeof(LevelTemplate), "Levels", "")]
	private List<int> m_levelTIDs;

	[SerializeField, TemplateIDField(typeof(LevelTemplate), "Shopekeeper Level", "")]
	private int m_shopkeeperLevelTID;

	[SerializeField]
	private int m_unlockShopkeeperLevel = 4;

	[SerializeField]
	private float m_secretAreaShowTime = 0.25f;

	[SerializeField]
	private int m_baseHeartPurchasePrice = 20;

	[SerializeField]
	private int m_additionalHeartPrice = 10;

	[SerializeField, TemplateIDField(typeof(SawFollower), "Saw", "")]
	private int m_sawFollowerTID;


	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int BackgroundDisplayTID => m_backgroundDisplayTID;
	public int BackgroundSkyDisplayTID => m_backgroundSkyDisplayTID;
	public int BackgroundUndergroundDisplayTID => m_backgroundUndergroundDisplayTID;
	public IList<float> ParallaxValues => m_parallaxValues;
	public List<int> Levels => m_levelTIDs;
	public float SecretAreaShowTime => m_secretAreaShowTime;
	public int BaseHeartPurchasePrice => m_baseHeartPurchasePrice;
	public int AdditionalHeartPrice => m_additionalHeartPrice;
	public int UnlockShopkeeperLevel => m_unlockShopkeeperLevel;
	public int ShopkeeperLevel => m_shopkeeperLevelTID;
	public int SawFollowerTID => m_sawFollowerTID;

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public float GetParallaxValue(int a_index)
	{
		if (a_index < 0)
			return 1f;

		if (a_index >= m_parallaxValues.Count)
			return m_parallaxValues[0];
		if (a_index < m_parallaxValues.Count)
		{
			return m_parallaxValues[a_index];
		}
		return 1f;
	}

	public int GetHeartPurchasePrice()
	{
		int extraHearts = SaveManager.Instance().WorldSaveData.ExtraHeartsPurchased;
		return m_baseHeartPurchasePrice + extraHearts * m_additionalHeartPrice;
	}

	public int GetLevelNumberFromTID(int a_levelTID)
	{
		return m_levelTIDs.IndexOf(a_levelTID);
	}

	public int GetLevelNumber(int a_levelTID)
	{
		int levelNum = 1;
		for (int i = 0; i < m_levelTIDs.Count; i++)
		{
			int levelTID = m_levelTIDs[i];
			if (levelTID == a_levelTID)
				return levelNum;

			var level = AssetCacher.Instance.CacheAsset<LevelTemplate>(levelTID);
			if (level.ShowNumbering)
			{
				levelNum++;
			}
		}
		return levelNum;
	}

	#endregion Runtime Functions
}
