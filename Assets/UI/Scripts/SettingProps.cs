using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * A classe SettingsProps define um conjunto Slider e Campo de Texto de configurações.
 */
public class SettingProps : MonoBehaviour
{
    [Header("Values")]
    public string settingName;
    [SerializeField] private float defaultValue;
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;

    [HideInInspector] public Slider slider;
    [HideInInspector] public TMP_InputField input;

    /*
     * Em Unity a função Start é chamada uma vez ao iniciar o jogo.
     */
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
