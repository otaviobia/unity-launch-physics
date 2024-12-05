using UnityEngine;

/*
 *  A classe CameraMovement é responsável por permitir ao usuário o movimento lateral da câmera.
 */
public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private GameObject leftButton;

    [Header("Variables")]
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float minXBoundary;
    [SerializeField] private float minYBoundary;

    private bool movingRight = false, movingLeft = false;

    /*
    * Em Unity a função Update é chamada uma vez por frame.
    */
    private void Update()
    {
        float leftEdge = sceneCamera.transform.position.x - sceneCamera.orthographicSize * sceneCamera.aspect;
        float bottomEdge = sceneCamera.transform.position.y - sceneCamera.orthographicSize;

        if (leftEdge <= minXBoundary)
        {
            movingLeft = false;
            leftButton.SetActive(false);
        }
        else
        {
            leftButton.SetActive(true);
        }

        if (Input.GetKey(KeyCode.UpArrow)) sceneCamera.transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.DownArrow) && bottomEdge > minYBoundary) sceneCamera.transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.RightArrow)) sceneCamera.transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.LeftArrow) && leftEdge > minXBoundary) sceneCamera.transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);

        if (movingLeft)
            sceneCamera.transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);

        if (movingRight)
            sceneCamera.transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
    }

    /*
     * StartMove recebe uma booleana que define o sentido do movimento
     * e é chamada quando o usuário pressiona o botão da seta.
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
    * e é chamada quando o usuário deixa de pressionar o botão da seta.
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
