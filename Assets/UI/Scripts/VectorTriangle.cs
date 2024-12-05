using UnityEngine;

public class VectorTriangle : MonoBehaviour
{
    [HideInInspector] public LineRenderer vectorLine;

    /*
     * GoToPosition recebe um componente LineRenderer e altera a posi��o e rota��o
     * do tri�ngulo para gerar a representa��o gr�fica de um vetor.
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
     * Em Unity a fun��o Start � chamada uma vez ao iniciar o jogo.
     */
    private void Start()
    {
        GetComponent<SpriteRenderer>().color = vectorLine.startColor;
    }

    /*
    * Em Unity a fun��o Update � chamada uma vez por frame.
    */
    void Update()
    {
        GoToPosition(vectorLine);
    }
}
