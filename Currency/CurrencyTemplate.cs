using UnityEngine;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// CurrencyTemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "ScriptableObjects/CurrencyTemplate")]
public class CurrencyTemplate : DisplayTemplate
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private string m_currencyID;

	[SerializeField]
	private bool m_persistGlobal = true;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public string CurrencyID { get { return m_currencyID; } }
	public bool PersistGlobal { get { return m_persistGlobal; } }

	#endregion Accessors
}
