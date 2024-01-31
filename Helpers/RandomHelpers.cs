using System.Collections.Generic;
using System.Linq;

public static class RandomHelpers
{
	/// <summary>
	/// Chooses random indexes and returns them in a list
	/// </summary>
	/// <param name="a_maxNumber">Inclusive</param>
	public static List<int> GetUniqueRandomIndexes(int a_numberOfIndexes, int a_maxNumber)
	{
		List<int> encounterIndexes = Enumerable.Range(0, a_maxNumber).ToList();
		List<int> chosenIndexes = new List<int>(a_numberOfIndexes);
		for (int i = 0; i < a_numberOfIndexes && i < a_maxNumber; i++)
		{
			int index = encounterIndexes[UnityEngine.Random.Range(0, encounterIndexes.Count)];
			chosenIndexes.Add(index);
			encounterIndexes.Remove(index);
		}

		return chosenIndexes;
	}

	public static int GetRandomValue(int a_min, int a_max)
	{
		return UnityEngine.Random.Range(a_min, a_max);
	}

	public static float GetRandomValue(float a_min, float a_max)
	{
		return UnityEngine.Random.Range(a_min, a_max);
	}
}