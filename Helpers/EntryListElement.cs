using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// EntryListElement
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/// <summary>
/// UI Element for managing a list of entries
/// </summary>
public class EntryListElement : MonoBehaviour
{
	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField, Tooltip("The transform to use as the parent of added entries.")]
	private Transform m_entryParent = null;

	[SerializeField, Tooltip("Whether or not entries should be pulled from the game object pool.")]
	private bool m_usePool = false;

	//--- NonSerialized ---
	protected List<Component> m_entries = new List<Component>();

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public Transform EntryParent { get { return m_entryParent; } }
	public int EntryCount { get { return m_entries.Count; } }

	public List<Component> Entries { get { return m_entries; } }

	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	/// <summary>Called when the object becomes inactive in the scene.</summary>
	void OnDisable()
	{
		ClearEntries();
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public T AddEntry<T>(int a_entryTID) where T : Component
	{
		T entry = null;

		if (m_usePool)
		{
			entry = GameObjectPool.GetFromPool<T>(a_entryTID);
		}
		else
		{
			entry = AssetCacher.Instance.InstantiateComponent<T>(a_entryTID);
		}

		if (entry == null)
			return null;

		Transform entryTransform = entry.transform;

		entryTransform.SetParent(m_entryParent, false);
		entryTransform.SetAsLastSibling();
		UIUtils.SetActive(entry, true);

		m_entries.Add(entry);

		return entry;
	}

	public T AddEntry<T>(T a_entry) where T : Component
	{
		T entry = null;

		entry = GameObject.Instantiate<T>(a_entry);

		if (entry == null)
			return null;

		Transform entryTransform = entry.transform;

		entryTransform.SetParent(m_entryParent, false);
		entryTransform.SetAsLastSibling();
		UIUtils.SetActive(entry, true);

		m_entries.Add(entry);

		return entry;
	}

	public T FindEntry<T>(System.Predicate<T> a_match) where T : Component
	{
		return m_entries.Find(e => e is T && a_match(e as T)) as T;
	}

	public T GetEntryAtIndex<T>(int a_index) where T : Component
	{
		if (a_index < m_entries.Count)
		{
			return m_entries[a_index] as T;
		}
		return null;
	}

	public bool FirstEntryToLast()
	{
		if (m_entries.Count == 0 || m_entryParent.childCount == 0)
			return false;

		if (m_entryParent == null)
			return false;

		m_entryParent.GetChild(0).SetAsLastSibling();
		return true;
	}

	public void RemoveEntry(Component a_entry)
	{
		if (a_entry == null)
			return;

		m_entries.Remove(a_entry);

		if (m_usePool)
			GameObjectPool.ReleaseToPool(a_entry);
		else
			Destroy(a_entry.gameObject);
	}

	public void ClearEntries()
	{
		int entryCount = m_entries.Count;
		for (int i = entryCount - 1; i >= 0; --i)
		{
			RemoveEntry(m_entries[i]);
		}
	}

	#endregion Runtime Functions
}