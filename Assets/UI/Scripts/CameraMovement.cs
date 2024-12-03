using UnityEngine;

/*
 *  A classe CameraMovement � respons�vel por permitir ao usu�rio o movimento lateral da c�mera.
 */
public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private GameObject leftButton;

    [Header("Variables")]
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float minXBoundary;

    private bool movingRight = false, movingLeft = false;

    /*
    * Em Unity a fun��o Update � chamada uma vez por frame.
    */
    private void Update()
    {
        float halfCameraWidth = sceneCamera.orthographicSize * sceneCamera.aspect;

        float leftEdge = sceneCamera.transform.position.x - halfCameraWidth;

        if (leftEdge <= minXBoundary)
        {
            movingLeft = false;
            leftButton.SetActive(false);
        }
        else
        {
            leftButton.SetActive(true);
        }

        if (movingLeft)
            sceneCamera.transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);

        if (movingRight)
            sceneCamera.transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
    }

    /*
     * StartMove recebe uma booleana que define o sentido do movimento
     * e � chamada quando o usu�rio pressiona o bot�o da seta.
     */
    public void StartMove(bool right)
    {
        if (right)
        {
            movingRight = true;
        }
        else
        {
            movingLeft = true;
        }
    }

    /*
    * StopMove recebe uma booleana que define o sentido do movimento
    * e � chamada quando o usu�rio deixa de pressionar o bot�o da seta.
    */
    public void StopMove(bool right)
    {
        if (right)
        {
            movingRight = false;
        }
        else
        {
            movingLeft = false;
        }
    }
}
