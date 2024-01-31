using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemHelper : MonoBehaviour
{
	#region Definitions


	#endregion Definitions

	#region Variables

	//--- Serialized ---
	[SerializeField]
	private List<ParticleSystem> m_systems = new List<ParticleSystem>();

	[SerializeField]
	private float m_simulateTimeAmount = 1f;

	//--- NonSerialized ---

	#endregion Variables

	#region Accessors


	#endregion Accessors

	#region Unity Messages

	private void OnEnable()
	{
		foreach (var system in m_systems)
		{
			system.Simulate(m_simulateTimeAmount, true, true, false);
			system.Play();
		}
	}

	#endregion Unity Messages

	#region Runtime Functions

	#endregion Runtime Functions

	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{
		m_systems.Clear();
		m_systems.AddRange(GetComponentsInChildren<ParticleSystem>());
	}
#endif
}