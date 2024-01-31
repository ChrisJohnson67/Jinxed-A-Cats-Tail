using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu, Serializable]
public class CurrencySettings : TemplateObject
{

	public const string c_GoldCurrency = "gold";

	[SerializeField, TemplateIDField(typeof(CurrencyTemplate), "Currencies", "")]
	private List<int> m_currencies = new List<int>();

	public IList<int> CurrenciesList { get { return m_currencies.AsReadOnly(); } }


	public CurrencyTemplate GetCurrency(string a_id)
	{
		foreach (var currencyTID in m_currencies)
		{
			var template = AssetCacher.Instance.CacheAsset<CurrencyTemplate>(currencyTID);
			if (template != null && template.CurrencyID.Equals(a_id))
			{
				return template;
			}
		}
		return null;
	}

}
