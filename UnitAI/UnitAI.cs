//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// UnitAI
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class UnitAI
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	protected AIState m_currentState;
	protected PlatformerUnitInstance m_unitInstance;
	protected AITemplate m_aiTemplate;
	protected bool m_active;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public AITemplate Template { get { return m_aiTemplate; } }
	public PlatformerUnitInstance UnitInstance { get { return m_unitInstance; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	public UnitAI(AITemplate a_template, PlatformerUnitInstance a_unit)
	{
		m_unitInstance = a_unit;
		m_aiTemplate = a_template;
		m_active = true;

		var startingState = AssetCacher.Instance.CacheAsset<AIStateTemplate>(Template.StartingStateTID);
		if (startingState != null)
		{
			//ChangeState(startingState.CreateInstance(this));
		}
	}

	public virtual void Update(float a_deltaTime)
	{
		if (!m_active)
			return;

		if (m_currentState != null)
			m_currentState.Update(a_deltaTime);
	}

	public virtual void FixedUpdate(float a_deltaTime)
	{
		if (!m_active)
			return;

		if (m_currentState != null)
			m_currentState.FixedUpdate(a_deltaTime);
	}

	public void Activate(bool a_active)
	{
		m_active = a_active;
	}

	public void ChangeState(AIState a_newState)
	{
		if (m_currentState != null)
			m_currentState.ExitState();

		m_currentState = a_newState;
		m_currentState.EnterState();
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

}
