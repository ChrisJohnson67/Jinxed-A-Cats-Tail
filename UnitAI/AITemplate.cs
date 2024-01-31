using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// AITemplate
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
[CreateAssetMenu(menuName = "ScriptableObjects/AITemplate")]
public class AITemplate : TemplateObject
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField, TemplateIDField(typeof(AIStateTemplate), "Starting AI State", "")]
	private int m_startingStateTID;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public int StartingStateTID { get { return m_startingStateTID; } }

	#endregion Accessors

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public virtual UnitAI CreateInstance(PlatformerUnitInstance a_unit)
	{
		return new UnitAI(this, a_unit);
	}

	#endregion Runtime Functions

}
