using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private float zoomAmount;
    [SerializeField] private int maxZoom;

    private float defaultSize;

    private void Start()
    {
        defaultSize = sceneCamera.orthographicSize;
    }

    public void ZoomReset()
    {
        sceneCamera.orthographicSize = defaultSize;
        sceneCamera.transform.position = new(0, 4, -10);
    }

    public void ZoomIn()
    {
        if (sceneCamera.orthographicSize > zoomAmount)
        {
            sceneCamera.orthographicSize -= zoomAmount;
            sceneCamera.transform.position = new(sceneCamera.transform.position.x - 1.8f * zoomAmount, sceneCamera.transform.position.y - zoomAmount, -10);
        }
    }

    public void ZoomOut()
    {
        if (sceneCamera.orthographicSize - defaultSize < zoomAmount * maxZoom)
        {
            sceneCamera.orthographicSize += zoomAmount;
            sceneCamera.transform.position = new(sceneCamera.transform.position.x + 1.8f * zoomAmount, sceneCamera.transform.position.y + zoomAmount, -10);
        }
    }
}
