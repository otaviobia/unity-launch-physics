using UnityEngine;

/*
 * A classe CameraZoom permite ao usu�rio controlar o zoom da c�mera,
 * facilitando a visualiza��o completa de qualquer trajet�ria gerada.
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
     * Em Unity a fun��o Start � chamada uma vez ao iniciar o jogo.
     */
    private void Start()
    {
        defaultZoom = sceneCamera.orthographicSize;
        defaultPosition = sceneCamera.transform.position;
    }

    /*
     * A fun��o ZoomReset � chamada ao apertar o bot�o da lupa com a seta de retorno na interface.
     * Volta aos padr�es de tamanho ortogr�fico da c�mera e tela.
     */
    public void ZoomReset()
    {
        sceneCamera.orthographicSize = defaultZoom;
        sceneCamera.transform.position = defaultPosition;
    }

    /*
     * A fun��o ZoomIn � chamada ao apertar o bot�o da lupa com o sinal positivo na interface.
     * Se for poss�vel dar zoom, diminui o tamanho ortogr�fico da c�mera e recentraliza a tela.
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
     * A fun��o ZoomOut � chamada ao apertar o bot�o da lupa com o sinal negativo na interface.
     * Se for poss�vel dar zoom, aumenta o tamanho ortogr�fico da c�mera e recentraliza a tela.
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
