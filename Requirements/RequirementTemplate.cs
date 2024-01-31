using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[Serializable, CreateAssetMenu(menuName = "Requirements")]
public abstract class RequirementTemplate : TemplateObject
{
    [SerializeField]
    protected bool m_applyNot = false;

    public bool ApplyNot { get { return m_applyNot; } }

    public virtual bool MeetsRequirements(EventInfo a_data)
    {
        return true;
    }
}
