using UnityEngine;

// Importando uma biblioteca de Tweening para animar elementos da interface por código
using DG.Tweening;
using UnityEngine.UI;

public class SimulationSettings : MonoBehaviour
{
    // Aqui declaramos referências aos componentes da cena para que eles possam ter seus valores alterados por código
    [Header("References")]
    [SerializeField] private RectTransform _settingsButton;
    [SerializeField] private RectTransform _settingsPanel;

    [Header("Variables")]
    private bool settingsPanelOpen = false;

    public void OnSettingsButtonPressed()
    {
        if (settingsPanelOpen)
        {
            settingsPanelOpen = false;
            _settingsButton.DORotateQuaternion(new(0, 0, 0, 1), 1f);
            _settingsPanel.DOScaleY(0, .5f);
        }
        else
        {
            settingsPanelOpen = true;
            _settingsButton.DORotateQuaternion(new(0, 0, 1, 1), 1f);
            _settingsPanel.DOScaleY(1, .5f);
        }
        
    }
}
