public static class CheatCommands
{
	public static void Add100Gold()
	{
		SaveManager.Instance().WorldSaveData.AddSilverBells(100);
	}

	public static void ResetGame()
	{
		SaveManager.Instance().ResetSaveData();
	}


	public static void CompleteAllLevels()
	{
		foreach (var level in GameManager.Instance.LevelSettings.Levels)
		{
			SaveManager.Instance().WorldSaveData.CompleteLevel(level);
		}
		SaveManager.Instance().WorldSaveData.MarkSeenIntro();
	}

	public static void CompleteAllLevelsExceptLast()
	{
		foreach (var level in GameManager.Instance.LevelSettings.Levels)
		{
			if (level != 1389332259)
			{
				SaveManager.Instance().WorldSaveData.CompleteLevel(level);
			}
		}
		SaveManager.Instance().WorldSaveData.MarkSeenIntro();
	}

	public static void Set30Flowers()
	{
		foreach (var level in GameManager.Instance.LevelSettings.Levels)
		{
			for (int i = 0; i < 3; i++)
			{
				SaveManager.Instance().WorldSaveData.AddSecretTreat(level, i);
			}
		}
	}

	public static void CompleteNextLevel()
	{
		SaveManager.Instance().WorldSaveData.MarkSeenIntro();
		foreach (var level in GameManager.Instance.LevelSettings.Levels)
		{
			var levelSaveData = SaveManager.Instance().WorldSaveData.GetLevelSaveData(level);
			if (levelSaveData == null || !levelSaveData.Completed)
			{
				SaveManager.Instance().WorldSaveData.CompleteLevel(level);
				return;
			}
		}
	}
}
