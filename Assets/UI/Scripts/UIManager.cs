using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Importando uma biblioteca de Tweening para animar elementos da interface por código
using DG.Tweening;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    // Aqui declaramos referências aos componentes da cena para
    // que eles possam ter os seus valores alterados no código
    [Header("References")]
    [SerializeField] private RectTransform _settingsButton;
    [SerializeField] private RectTransform _settingsPanel;
    [SerializeField] private SettingProps[] _settingsItems;

    [Header("Variables")]
    private bool settingsPanelOpen = false;
    [HideInInspector] public float ui_gravity, ui_speed, ui_angle, ui_viscosity, ui_mass;

    // Função chamada ao apertar o botão de configurações - Abre ou fecha o painel de configurações
    public void OnSettingsButtonPressed()
    {
        _settingsButton.DORotateQuaternion(new(0, 0, settingsPanelOpen ? 0 : 1, 1), 1f);
        _settingsPanel.DOScaleY(settingsPanelOpen ? 0 : 1, .5f);
        settingsPanelOpen = !settingsPanelOpen;
    }

    public void OnSliderChange(GameObject callerParent)
    {
        SettingProps thisSetting = callerParent.GetComponent<SettingProps>();
        thisSetting.input.text = thisSetting.slider.value.ToString();
        switch (thisSetting.settingName) 
        {
            case "gravity":
                ui_gravity = thisSetting.slider.value;
                break;
            case "speed":
                ui_speed = thisSetting.slider.value;
                break;
            case "angle":
                ui_angle = thisSetting.slider.value;
                break;
            case "viscosity":
                ui_viscosity = thisSetting.slider.value;
                break;
            case "mass":
                ui_mass = thisSetting.slider.value;
                break;
            default:
                Debug.Log("O nome da configuração não está escrita corretamente!");
                break;
        }
    }

    public void OnInputChange(GameObject callerParent)
    {
        SettingProps thisSetting = callerParent.GetComponent<SettingProps>();
        float newValue = float.Parse(thisSetting.input.text);
        thisSetting.slider.value = newValue;
        switch (thisSetting.settingName)
        {
            case "gravity":
                ui_gravity = newValue;
                break;
            case "speed":
                ui_speed = newValue;
                break;
            case "angle":
                ui_angle = newValue;
                break;
            case "viscosity":
                ui_viscosity = newValue;
                break;
            case "mass":
                ui_mass = newValue;
                break;
            default:
                Debug.Log("O nome da configuração não está escrita corretamente!");
                break;
        }
    }
}
