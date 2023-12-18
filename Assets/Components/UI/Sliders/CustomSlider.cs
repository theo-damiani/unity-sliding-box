using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CustomSlider : Slider
{
    public TextMeshProUGUI valueTMP;
    public bool snapToDecimal;
    public enum DecimalDigits { Zero, One, Two, Three }
    public DecimalDigits numDecimalDigits = default;
    public Color color = Color.black;
    public bool applyColorToValue;
    public FloatVariable dynamicMaxValue;

    public Image handleStroke;
    public Image handleImage;

    protected override void OnEnable()
    {
        base.OnEnable();
        onValueChanged.AddListener(UpdateValue);
        ApplyColor();

        if (dynamicMaxValue)
        {
            GameEvent gameEvent = dynamicMaxValue.OnUpdateEvent;
            if (gameEvent)
            {
                gameEvent.OnRaise += OnDynamicMaxValueUpdate;
            }
        }
    }

    protected override void OnDisable()
    {
        onValueChanged.RemoveListener(UpdateValue);
        base.OnDisable();

        if (dynamicMaxValue)
        {
            GameEvent gameEvent = dynamicMaxValue.OnUpdateEvent;
            if (gameEvent)
            {
                gameEvent.OnRaise -= OnDynamicMaxValueUpdate;
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        onValueChanged.AddListener(UpdateValue);
        UpdateValue(value);
        ApplyColor();
    }

    protected override void OnDestroy()
    {
        onValueChanged.RemoveListener(UpdateValue);
        base.OnDestroy();
    }

    public void ApplyColor()
    {
        if (!fillRect)
        {
            Debug.LogWarning("Fill Rect has not been assigned");
            return;
        }

        if (!handleRect)
        {
            Debug.LogWarning("Handle Rect has not been assigned");
            return;
        }

        fillRect.GetComponent<Image>().color = color;
        handleRect.GetChild(0).GetComponent<Image>().color = color;

        UpdateInteracivitySliderUI();
    }

    private void UpdateValueLabel(float value)
    {
        string format = "0.";
        for (int i = 0; i < (int)numDecimalDigits; i++)
        {
            format += "0";
        }

        // Add a minus sign spacer for positive values so the actual digits are always aligned
        float threshold = -0.5f * Mathf.Pow(10f, -(int)numDecimalDigits);
        string spacer = value > threshold ? "<color=#ffffff00>-</color>" : "";

        if (valueTMP) {
            valueTMP.text = spacer + value.ToString(format);
        }
    }

    private bool ValueSatisfyRestriction(float value)
    {
        if (dynamicMaxValue && (value > dynamicMaxValue.Value))
        {
            this.value = dynamicMaxValue.Value;
            return false;       
        }
        return true;
    }

    public void UpdateValue(float value)
    {
        if (snapToDecimal)
        {
            float factor = Mathf.Pow(10f, (int)numDecimalDigits);
            this.value = Mathf.Round(factor * value) / factor;
        }

        if (!ValueSatisfyRestriction(value))
        {
            return;
        }

        UpdateValueLabel(value);
    }

    private void OnDynamicMaxValueUpdate()
    {
        if (ValueSatisfyRestriction(value))
        {
            return;
        }

        UpdateValueLabel(value);
    }

    public void UpdateInteracivitySliderUI()
    {
        if (handleImage && handleStroke)
        {
            if (interactable)
            {
                Color c = color;
                handleStroke.color = c;

                Color cBg = Color.white;
                handleImage.color = cBg;
            }
            else
            {
                Color c = color;
                c.a = 0.51f;
                handleStroke.color = c;

                Color cBg = Color.white;
                cBg.a = 0.39f;
                handleImage.color = cBg;
            }
        }
    }
}
