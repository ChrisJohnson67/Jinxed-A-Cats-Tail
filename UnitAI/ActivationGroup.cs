using System.Collections.Generic;
using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ActivationGroup
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class ActivationGroup : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private List<PlatformerUnitModel> m_unitModelsToActivate = new List<PlatformerUnitModel>();

	[SerializeField]
	private List<LevelPiece> m_levelPiecesToActivate = new List<LevelPiece>();

	//--- NonSerialized ---

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void Awake()
	{
		foreach (var model in m_unitModelsToActivate)
		{
			if (model != null && model.EnemyAI != null)
			{
				model.EnemyAI.OnRenderVisible += OnActivated;
			}
		}
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions



	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks

	private void OnActivated()
	{
		foreach (var model in m_unitModelsToActivate)
		{
			if (model != null && model.EnemyAI != null)
			{
				model.EnemyAI.SetActivated();
			}
		}

		foreach (var piece in m_levelPiecesToActivate)
		{
			if (piece != null)
			{
				piece.SetActivated(true);
			}
		}
	}

	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{
	}

	protected virtual void OnDrawGizmos()
	{
		int num = 0;
		foreach (var model in m_unitModelsToActivate)
		{
			if (model != null)
			{
				UnityEditor.Handles.Label(model.ChatNode.transform.position, "grouped" + num);
				num++;
			}
		}
	}
#endif
}