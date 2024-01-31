using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// GlowButton
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class GlowButton : Button, IPointerEnterHandler, IPointerExitHandler
{
	//~~~~~ Defintions ~~~~~
	#region Definitions

	#endregion Definitions

	//~~~~~ Variables ~~~~~
	#region Variables

	[SerializeField]
	private GameObject m_glowObject;

	#endregion Variables

	//~~~~~ Accessors ~~~~~
	#region Accessors

	public GameObject GlowObject { get { return m_glowObject; } }

	#endregion Accessors


	//~~~~~ Runtime Functions ~~~~~
	#region Runtime Functions

	protected override void Awake()
	{
		base.Awake();
		UIUtils.SetActive(m_glowObject, false);
	}

	protected override void Start()
	{
		if (Application.isPlaying && EventSystem.current.currentSelectedGameObject == gameObject)
		{
			ShowGlow(true);
			OnSelect(null);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		UIUtils.SetActive(m_glowObject, false);
	}

	public override void OnPointerEnter(PointerEventData a_eventData)
	{
		base.OnPointerEnter(a_eventData);

		if (interactable)
			OnSelect(a_eventData);
	}

	public override void OnPointerExit(PointerEventData a_eventData)
	{
		base.OnPointerExit(a_eventData);

		OnDeselect(a_eventData);
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);

		ShowGlow(true);
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);

		ShowGlow(false);
	}

	public void ShowGlow(bool a_show)
	{
		UIUtils.SetActive(m_glowObject, a_show);
	}

	#endregion Runtime Functions

	//~~~~~ Callbacks ~~~~~
	#region Callbacks


	#endregion Callbacks

#if UNITY_EDITOR

	protected override void OnValidate()
	{
		base.OnValidate();

		if (m_glowObject == null)
		{
			var node = transform.FindNode("glow");
			if (node != null)
			{
				m_glowObject = node.gameObject;
			}
		}

	}

#endif
}
