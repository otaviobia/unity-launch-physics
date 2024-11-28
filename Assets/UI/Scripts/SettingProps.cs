using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingProps : MonoBehaviour
{
    [Header("Values")]
    public string settingName;
    public float defaultValue;
    public float minValue;
    public float maxValue;

    [HideInInspector] public Slider slider;
    [HideInInspector] public TMP_InputField input;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        input = GetComponentInChildren<TMP_InputField>();

        slider.minValue = minValue;
        slider.maxValue = maxValue;
        slider.value = defaultValue;
        input.placeholder.GetComponent<TextMeshProUGUI>().text = defaultValue.ToString();
    }
}
