using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RequirementList
{
    [SerializeField]
    private List<RequirementTemplate> m_reqs = new List<RequirementTemplate>();

    public List<RequirementTemplate> Reqs { get { return m_reqs; } }

    public bool MeetsRequirements(EventInfo a_info)
    {
        bool meetsReqs = true;
        for (int i = 0; i < m_reqs.Count && meetsReqs; i++)
        {
            var req = m_reqs[i];
            bool metReq = req.MeetsRequirements(a_info);
            meetsReqs = meetsReqs && (req.ApplyNot ^ metReq);
        }
        return meetsReqs;
    }
}
