using UnityEngine;

public class FadeOutSprite : MonoBehaviour
{
	//~~~~~ Defintions ~~~~~
	#region Definitions


	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	//--- Serialized ---
	[SerializeField]
	private float m_fadeTime;

	[SerializeField]
	private SpriteRenderer m_sprite;

	//--- NonSerialized ---
	private float m_timer;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors


	#endregion Accessors

	//~~~~~ Unity Messages ~~~~~
	#region Unity Messages

	private void Update()
	{
		m_timer += Time.deltaTime;
		if (m_sprite != null)
		{
			var alpha = Mathf.Lerp(1f, 0f, m_timer / m_fadeTime);
			m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.g, m_sprite.color.b, alpha);
		}
	}

	#endregion Unity Messages

	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions



	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR
	private void OnValidate()
	{

	}
#endif
}