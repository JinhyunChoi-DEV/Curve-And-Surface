using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CalculateType
{
    NLI_Form = 0, BB_Form, Midpoint_Subdivision, Count
}

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private DataContainer data;
    [SerializeField] private LineDraw lineDraw;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text pointText;
    [SerializeField] private TMP_Text tValueText;
    [SerializeField] private TMP_Dropdown dropdown;

    public CalculateType Type => type;
    public float TValue => slider.value;

    private CalculateType type;

    void Awake()
    {
        slider.value = 0.5f;
        type = CalculateType.NLI_Form;

        slider.onValueChanged.AddListener(UpdateTValue);

        dropdown.options.Clear();
        for (int i = 0; i < (int)CalculateType.Count; ++i)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = ((CalculateType)i).ToString();
            dropdown.options.Add(option);
        }

        dropdown.onValueChanged.AddListener(UpdateDropDown);
    }

    void Update()
    {
        typeText.text = type.ToString();
        pointText.text = data.PointCounts.ToString();
        tValueText.text = slider.value.ToString("F2");
    }

    void UpdateTValue(float value)
    {
        lineDraw.UpdateTValue();
    }

    void UpdateDropDown(int index)
    {
        type = (CalculateType)index;
        lineDraw.RenderLine();
    }
}
