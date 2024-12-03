using UnityEngine;

/*
 * A classe CameraZoom permite ao usuário controlar o zoom da câmera,
 * facilitando a visualização completa de qualquer trajetória gerada.
 */
public class CameraZoom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera sceneCamera;

    [Header("Preferences")]
    [SerializeField] private float zoomAmount;
    [SerializeField] private int maxZoom;

    private float defaultZoom;
    private Vector3 defaultPosition;

    /*
     * Em Unity a função Start é chamada uma vez ao iniciar o jogo.
     */
    private void Start()
    {
        defaultZoom = sceneCamera.orthographicSize;
        defaultPosition = sceneCamera.transform.position;
    }

    /*
     * A função ZoomReset é chamada ao apertar o botão da lupa com a seta de retorno na interface.
     * Volta aos padrões de tamanho ortográfico da câmera e tela.
     */
    public void ZoomReset()
    {
        sceneCamera.orthographicSize = defaultZoom;
        sceneCamera.transform.position = defaultPosition;
    }

    /*
     * A função ZoomIn é chamada ao apertar o botão da lupa com o sinal positivo na interface.
     * Se for possível dar zoom, diminui o tamanho ortográfico da câmera e recentraliza a tela.
     */
    public void ZoomIn()
    {
        if (sceneCamera.orthographicSize > zoomAmount)
        {
            sceneCamera.orthographicSize -= zoomAmount;
            sceneCamera.transform.position = new(sceneCamera.transform.position.x 
                - 1.8f * zoomAmount, sceneCamera.transform.position.y - zoomAmount, -10);
        }
    }

    /*
     * A função ZoomOut é chamada ao apertar o botão da lupa com o sinal negativo na interface.
     * Se for possível dar zoom, aumenta o tamanho ortográfico da câmera e recentraliza a tela.
     */
    public void ZoomOut()
    {
        if (sceneCamera.orthographicSize - defaultZoom < zoomAmount * maxZoom)
        {
            sceneCamera.orthographicSize += zoomAmount;
            sceneCamera.transform.position = new(sceneCamera.transform.position.x 
                + 1.8f * zoomAmount, sceneCamera.transform.position.y + zoomAmount, -10);
        }
    }
}
