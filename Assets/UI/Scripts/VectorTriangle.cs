using UnityEngine;

public class VectorTriangle : MonoBehaviour
{
    [HideInInspector] public LineRenderer vectorLine;

    /*
     * GoToPosition recebe um componente LineRenderer e altera a posição e rotação
     * do triângulo para gerar a representação gráfica de um vetor.
     */
    public void GoToPosition(LineRenderer vectorLine)
    {
        Vector3 firstPos = vectorLine.GetPosition(0);
        Vector3 secondPos = vectorLine.GetPosition(1);

        transform.position = secondPos;

        Vector3 direction = secondPos - firstPos;
        float angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleZ);
    }

    /*
     * Em Unity a função Start é chamada uma vez ao iniciar o jogo.
     */
    private void Start()
    {
        GetComponent<SpriteRenderer>().color = vectorLine.startColor;
    }

    /*
    * Em Unity a função Update é chamada uma vez por frame.
    */
    void Update()
    {
        GoToPosition(vectorLine);
    }
}
