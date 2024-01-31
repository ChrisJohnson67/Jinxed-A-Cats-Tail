using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Used to represent a meter that fills over time
/// </summary>
public class FillMeter : MonoBehaviour
{
	#region Definitions

	//--- Constants ---

	/// <summary>
	/// Animator Bool which is true while the meter fill animation is in progress
	/// </summary>
	private const string METER_FILLING_BOOL = "bFilling";
	/// <summary>
	/// Animator Trigger set when the meter finishes animating to completely full
	/// </summary>
	private const string METER_FULL_TRIGGER = "tFull";
	/// <summary>
	/// Animator Float for the percent full the meter currently is
	/// </summary>
	private const string METER_PERCENT_FLOAT = "fPercent";

	//--- Enumerations ---

	/// <summary>
	/// How interpolation should accelerate and decelerate over time
	/// </summary>
	private enum InterpolationMode
	{
		Linear,
		SmoothInOut,
		Accelerate,
		Decelerate,
	}

	//--- Events ---

	/// <summary>
	/// Triggered when a fill starts
	/// </summary>
	public event Action OnFillStart;
	/// <summary>
	/// Triggered when a fill steps
	/// </summary>
	public event Action OnFillStep;
	/// <summary>
	/// Triggered when a fill stops
	/// </summary>
	public event Action OnFillStop;
	/// <summary>
	/// Triggered when the meter is full
	/// </summary>
	public event Action OnFull;

	/// <summary>
	/// Coroutine to run when the meter is full
	/// </summary>
	public Func<IEnumerator> RunWhenMeterFull;

	#endregion Definitions

	#region Variables

	//--- Serialized ---

	[Header("Meter Settings")]

	/// <summary>
	/// Seconds over which the meter should fill to its new value
	/// </summary>
	[SerializeField, Tooltip("Seconds (value axis) over which the meter should fill to its new value, based on the percent change of the fill (time axis, 0-1)")]
	private AnimationCurve m_meterFillTime = new AnimationCurve();

	/// <summary>
	/// Seconds over which the meter should fill to its new value
	/// </summary>
	[SerializeField, Tooltip("How the meter should accelerate and decelerate over the course of each fill")]
	private InterpolationMode m_meterFillMode = InterpolationMode.SmoothInOut;

	/// <summary>
	/// Default amount representing a filled meter
	/// </summary>
	[SerializeField, Tooltip("Default amount representing a filled meter")]
	private int m_meterMaxDefault = 1;

	/// <summary>
	/// If true, cap the meter value to the meter max
	/// </summary>
	[SerializeField, Tooltip("If true, cap the meter value to the meter max")]
	private bool m_capToMax = true;

	[Space(6f)]
	/// <summary>
	/// Animator to update as the meter fills
	/// </summary>
	[SerializeField, Tooltip("Animator to update as the meter fills; must have the following parameters:\n  \"" + METER_PERCENT_FLOAT + "\" - Float for the percent full the meter currently is\n  \"" + METER_FILLING_BOOL + "\" - Bool which is true while the meter fill animation is in progress\n  \"" + METER_FULL_TRIGGER + "\" - Trigger set when the meter finishes animating to completely full")]
	private Animator m_meterAnimator;

	/// <summary>
	/// Image to fill from 0 to 1 as the meter is filled
	/// </summary>
	[SerializeField, Tooltip("Image to fill from 0 to 1 as the meter is filled")]
	private Image m_meterImage;

	/// <summary>
	/// Slider to fill from 0 to 1 as the meter is filled
	/// </summary>
	[SerializeField, Tooltip("Slider to fill from 0 to 1 as the meter is filled")]
	private Slider m_meterSlider;

	[Header("Text Settings")]

	/// <summary>
	/// Text to fill with the count remaining to fill ($AMT$), the current amount ($CUR$), and/or the current max ($MAX$)
	/// </summary>
	[SerializeField, Tooltip("Text to fill with the count remaining to fill ($AMT$), the current amount ($CUR$), and/or the current max ($MAX$)")]
	private TMP_Text m_meterText;

	/// <summary>
	/// If true, update the text as the meter animates, so that the count rolls up over time
	/// </summary>
	[SerializeField, Tooltip("If true, update the text as the meter animates, so that the count rolls up over time")]
	private bool m_rollUpMeterText;

	//--- NonSerialized ---

	/// <summary>
	/// Coroutine that is actively filling the meter
	/// </summary>
	private Coroutine m_meterFill;
	/// <summary>
	/// Current display value of the fill amount
	/// </summary>
	private float m_fillAmount;

	/// <summary>
	/// Current fill amount of the meter
	/// </summary>
	private int m_meterCount;
	/// <summary>
	/// Amount representing a filled meter
	/// </summary>
	private int m_meterMax;

	#endregion Variables

	#region Accessors

	//--- Runtime ---

	/// <summary>
	/// Whether or not the meter is currently animating
	/// </summary>
	public bool IsAnimating { get { return m_meterFill != null; } }

	/// <summary>
	/// Current fill amount of the meter
	/// </summary>
	public int MeterCount
	{
		get { return m_meterCount; }
		private set
		{
			m_meterCount = value;

			if (m_capToMax && (m_meterCount > MeterMax))
				m_meterCount = MeterMax;
		}
	}
	/// <summary>
	/// Amount representing a filled meter
	/// </summary>
	public int MeterMax
	{
		get { return m_meterMax <= 0 ? m_meterMaxDefault : m_meterMax; }
		set { m_meterMax = value; }
	}

	/// <summary>
	/// Amount of the Image shown
	/// </summary>
	public float FillAmount
	{
		get { return m_fillAmount; }
		set
		{
			m_fillAmount = value;

			m_meterImage.fillAmount = m_fillAmount;

			if (m_meterSlider != null)
				m_meterSlider.value = m_fillAmount;
		}
	}

	#endregion Accessors

	#region Unity Methods

	/// <summary>
	/// Called when the object becomes inactive in the scene
	/// </summary>
	protected virtual void OnDisable()
	{
		// Abort any meter fill in progress
		Abort();

		// Clear any instance data
		MeterCount = 0;
		MeterMax = 0;
	}

	/// <summary>
	/// Called when the object becomes active in the scene
	/// </summary>
	protected virtual void OnEnable()
	{
		InitMeter(true);
	}

	/// <summary>
	/// Called when the script is loaded or a value is changed in the inspector
	/// </summary>
	protected virtual void OnValidate()
	{
		// Validate full amount
		if (m_meterMaxDefault <= 0)
		{
			m_meterMaxDefault = 1;
		}
	}

	#endregion Unity Methods

	#region Runtime Methods

	/// <summary>
	/// Increment the meter
	/// </summary>
	public void Increment(int a_amount = 1)
	{
		int oldCount = MeterCount;
		MeterCount += a_amount;

		// Refresh only if there was a change
		if (MeterCount != oldCount)
			RestartMeterFill();
	}

	/// <summary>
	/// Immediately sets the meter to a specific count and range, without animating
	/// </summary>
	public void SetMeter(int a_fillCount, int a_fillMax)
	{
		// Set the count
		MeterMax = a_fillMax;
		MeterCount = a_fillCount;

		// Initialize the meter
		InitMeter(true);

		// Force the animator update immediately so it appears correct the first frame
		UIUtils.UpdateAnimator(m_meterAnimator);
	}

	#endregion Runtime Methods

	#region Internal Methods

	/// <summary>
	/// Abort any meter fill in progress
	/// </summary>
	protected virtual void Abort()
	{
		if (m_meterFill != null)
		{
			StopCoroutine(m_meterFill);
			m_meterFill = null;
		}
	}

	/// <summary>
	/// Initialize the meter
	/// </summary>
	protected virtual void InitMeter(bool a_fillNow)
	{
		// Initialize the meter
		float fill = (float)MeterCount / (float)MeterMax;
		if (a_fillNow)
		{
			FillAmount = fill;

			// Initialize the meter text
			RefreshMeterText(MeterCount);
		}

		// Initialize the animator
		UIUtils.SetFloat(m_meterAnimator, METER_PERCENT_FLOAT, fill);
	}

	/// <summary>
	/// Initialize the meter text with the specified count
	/// </summary>
	private void RefreshMeterText(int a_current)
	{
		int max = MeterMax;


		// Otherwise, show the meter text
		UIUtils.SetActive(m_meterText, true);

		// Update the shown text
		m_meterText.text = a_current + "/" + max;
	}

	/// <summary>
	/// Notify when a fill event is triggered
	/// </summary>
	private void Notify(Action OnFillEvent)
	{
		if (OnFillEvent != null)
		{
			OnFillEvent();
		}
	}

	/// <summary>
	/// Start the meter filling
	/// </summary>
	protected virtual void RestartMeterFill()
	{
		Abort();

		InitMeter(false);
		if (!m_rollUpMeterText)
			RefreshMeterText(MeterCount);

		m_meterFill = StartCoroutine(WaitForMeterFill());
	}

	#endregion Internal Methods

	#region Coroutines

	/// <summary>
	/// Fills the meter over time
	/// </summary>
	private IEnumerator WaitForMeterFill()
	{
		// Get the starting and target fill amounts
		float max = (float)MeterMax;
		float start = FillAmount * max;
		float target = (float)MeterCount;
		UIUtils.SetBool(m_meterAnimator, METER_FILLING_BOOL, true);
		Notify(OnFillStart);

		// Fill the meter over time
		float duration = m_meterFillTime.Evaluate((target - start) / max);
		if (duration > 0f)
		{
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float percent = elapsed / duration;

				// Calculate the current fill based on the interpolation mode
				float fill = 0f;
				switch (m_meterFillMode)
				{
					case InterpolationMode.Linear:
						fill = Mathf.Lerp(start, target, percent);
						break;
					case InterpolationMode.SmoothInOut:
						fill = Mathf.SmoothStep(start, target, percent);
						break;
					case InterpolationMode.Accelerate:
						// NOTE: Double the range and halve the percent to accelerate to the max speed of the parabola.
						fill = Mathf.SmoothStep(start, target * 2f, percent * 0.5f);
						break;
					case InterpolationMode.Decelerate:
						// NOTE: Double the range and halve the percent to decelerate from the max speed of the parabola.
						fill = Mathf.SmoothStep(start - target, target, 0.5f + (percent * 0.5f));
						break;
				}

				FillAmount = fill / max;
				if (m_rollUpMeterText)
					RefreshMeterText(Mathf.FloorToInt(fill));

				Notify(OnFillStep);

				yield return null;
			}
		}

		// The fill has completed
		FillAmount = target / max;
		if (m_rollUpMeterText)
			RefreshMeterText(MeterCount);

		UIUtils.SetBool(m_meterAnimator, METER_FILLING_BOOL, false);
		Notify(OnFillStop);

		// Trigger when meter is full
		if (FillAmount >= 1f)
		{
			UIUtils.SetTrigger(m_meterAnimator, METER_FULL_TRIGGER);
			Notify(OnFull);
			if (RunWhenMeterFull != null)
			{
				yield return StartCoroutine(RunWhenMeterFull());
			}
		}

		// Clear the coroutine
		m_meterFill = null;
	}

	#endregion Coroutines
}

