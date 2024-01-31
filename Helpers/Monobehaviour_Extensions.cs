using UnityEngine;
public static class MonoBehaviour_Extensions
{
	public static GameObject FindObjectWithName(this GameObject a_object, string a_name)
	{
		if (a_object.name.Equals(a_name))
		{
			return a_object;
		}
		for (int i = 0; i < a_object.transform.childCount; i++)
		{
			var child = a_object.transform.GetChild(i);
			var foundObj = FindObjectWithName(child.gameObject, a_name);
			if (foundObj != null && foundObj.name.Equals(a_name))
			{
				return foundObj;
			}
		}
		return null;
	}
}