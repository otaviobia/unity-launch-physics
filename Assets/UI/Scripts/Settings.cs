using UnityEngine;

// Importando uma biblioteca de Tweening para animar elementos da interface por c�digo
using DG.Tweening;

/*
 * A classe Settings � respons�vel pela intera��o usu�rio-painel-c�digo das configura��es.
 */
public class Settings : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _settingsButton;
    [SerializeField] private RectTransform _settingsPanel;

    [HideInInspector] public float ui_gravity, ui_speed, ui_angle, ui_viscosity, ui_mass, ui_height, ui_timestep, ui_collisions, ui_iterations;
    private bool settingsPanelOpen = false;

    /*
     * OnSettinsButtonPressed � chamada ao apertar o bot�o de configura��es na interface.
     * Anima o bot�o de configura��es e abre o painel de configura��es tamb�m com anima��o.
     */
    public void OnSettingsButtonPressed()
    {
        _settingsButton.DORotateQuaternion(new(0, 0, settingsPanelOpen ? 0 : 1, 1), 1f);
        _settingsPanel.DOScaleY(settingsPanelOpen ? 0 : 1, .5f);
        settingsPanelOpen = !settingsPanelOpen;
    }

    /*
     * OnSliderChange � chamado ao mover um Slider nas configura��es.
     * Altera a vari�vel interna respectiva e atualiza o campo de texto com o valor inserido.
     */
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
            case "height":
                ui_height = thisSetting.slider.value;
                break;
            case "timestep":
                ui_timestep = thisSetting.slider.value;
                break;
            case "collisions":
                ui_collisions = thisSetting.slider.value;
                break;
            case "iterations":
                ui_iterations = thisSetting.slider.value;
                break;
            default:
                Debug.Log("O nome da configura��o n�o est� escrito corretamente!");
                break;
        }
    }

    /*
     * OnInputChange � chamado ao alterar um campo de texto nas configura��es.
     * Altera a vari�vel interna respectiva e atualiza o slider com o valor inserido.
     */
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
            case "height":
                ui_height = newValue;
                break;
            case "timestep":
                ui_timestep = newValue;
                break;
            case "collisions":
                ui_collisions = newValue;
                break;
            case "iterations":
                ui_iterations = newValue;
                break;
            default:
                Debug.Log("O nome da configura��o n�o est� escrito corretamente!");
                break;
        }
    }

    /*
     * ToggleGameObject recebe um GameObject gameObject e inverte seu estado ativo.
     */
    public void ToggleGameObject(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    /*
     * ShakeButton � chamado por um bot�o "caller" quando o mesmo � pressionado.
     * Utiliza a biblioteca de Tweening para animar brevemente o pressionamento.
     */
    public void ShakeButton(Transform caller)
    {
        caller.DOPunchScale(-Vector3.one * 0.1f, 0.1f, 0, 0);
    }
}
