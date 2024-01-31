using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class ClickArea : Graphic
{
	public override void SetLayoutDirty() { return; }
	public override void SetMaterialDirty() { return; }
	public override void SetVerticesDirty() { return; }

#if UNITY_EDITOR
	protected override void Reset()
	{
		base.Reset();
		raycastTarget = true;
		color = new Color(1f, 1f, 1f, 0f);
	}
#endif
}
