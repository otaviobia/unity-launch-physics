using System;
using UnityEngine;

/*
 * A classe TrajectoryDrawer � respons�vel por desenhar na tela a trajet�ria do lan�amento.
 */
public class TrajectoryDrawer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer trajectoryRenderer;
    [SerializeField] private Physics2 physics2;

    [Header("Trajectory Line")]
    [SerializeField] private int pointsToGenerate = 10000;
    [SerializeField] private float deltaT = 0.1f;

    /*
     * Em Unity a fun��o Start � chamada uma vez ao iniciar o jogo.
     */
    private void Start()
    {
        physics2.OnReady += OnReadyToGenerate;
    }

    /*
     * OnReadyToGenerate � chamada quando o script de f�sica termina de calcular as 
     * condi��es iniciais usando Eventos. Chama a fun��o para desenhar a trajet�ria.
     */
    private void OnReadyToGenerate(object sender, EventArgs e)
    {
        GenerateTrajectoryLine(pointsToGenerate, deltaT);
    }

    /*
     * GenerateTrajectoryLine desenha na tela a trajet�ria do proj�til utilizando o componente LineRenderer.
     * Recebe uma quantidade de pontos (int points) e dist�ncia entre pontos (float dt).
     */
    public void GenerateTrajectoryLine(int points, float dt)
    {
        trajectoryRenderer.positionCount = 0;

		int colIndex = 0;
        for (float i = 0f; i < dt * points; i += dt)
        {
            trajectoryRenderer.SetPosition(++trajectoryRenderer.positionCount - 1, physics2.Get_position(i));
			if(colIndex < physics2.nCol - 1 && i <= physics2.col_arr[colIndex] && i+dt > physics2.col_arr[colIndex]){
				trajectoryRenderer.SetPosition(++trajectoryRenderer.positionCount - 1, physics2.Get_position(physics2.col_arr[colIndex++]));
			}
        }
    }
}
