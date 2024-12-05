using System;
using System.Collections;
using UnityEngine;

public class Physics2 : MonoBehaviour {

	private LineRenderer lrVel, lrAcc;
	public event EventHandler<EventArgs> OnReady;
	private RectTransform rt;

	[SerializeField] private RectTransform floor;
	private float floorY;

	[SerializeField] private Settings UIM;
	private float viscosity, angle, initial_velocity, gravity, mass, initialY;
	
	private int updateFrequency, nNewton;	
	[HideInInspector] public int nCol;

	[HideInInspector] public float[] col_arr;
	private Vector2[] vel_arr, pos_arr;

	enum Case {noCol, oneCol, manyCol, floor};
	private Case myCase;

	[HideInInspector] public bool start;
	private float time, deltaT;
	private float tau;

	private GameObject velLineObject, accLineObject;
	[SerializeField] private GameObject vTriangle;
    [SerializeField] private Color vColor, aColor;
    
	void Start() {
		rt = gameObject.GetComponent<RectTransform>();
		start = false;
		StartCoroutine(LateStart(1.0f));

		InstantiateVectors();
    }

    IEnumerator LateStart(float waitTime) {
        yield return new WaitForSeconds(waitTime);
		Reset();
	}
	
	void Update() {
		if(!start) return;
		deltaT += Time.deltaTime;
		float dt =  1.0f / updateFrequency;
		while(deltaT >= dt){
			Vector2 pos = Get_position(time);
			Vector2 vel = Get_velocity(time);
			Vector2 acc = Get_acceleration(time);
			

			lrVel.SetPosition(0, pos);
			lrVel.SetPosition(1, pos + vel);
			lrAcc.SetPosition(0, pos);
			lrAcc.SetPosition(1, pos + acc);

			rt.position = pos;
			time += dt;
			deltaT -= dt;
		}
    }

	/*-------------------------------------------------------------------------*\
	| Reset() atualiza as variaveis, calcula as colisões e reseta a simulação   |
	\*-------------------------------------------------------------------------*/
	public void Reset() {
		start = false;
		floorY = floor.position.y + floor.rect.height / 2.0f;
		
		viscosity = UIM.ui_viscosity;
		initial_velocity = UIM.ui_speed;
		angle = UIM.ui_angle;
		gravity = UIM.ui_gravity;
		mass = UIM.ui_mass;
		initialY = UIM.ui_height;
		updateFrequency = (int) UIM.ui_timestep;
		nNewton = (int) UIM.ui_iterations;
		nCol = (int) UIM.ui_collisions + 2;

		deltaT = 0.0f;
		tau = mass / viscosity;

		time = 0.0f;
		col_arr = new float[nCol]; // Tempo em que ocorrem as colisões
		vel_arr = new Vector2[nCol]; // Velocidade da particula após as colisões
		pos_arr = new Vector2[nCol]; // Posição da particula no momento da colisão

		float rad = angle * Mathf.PI / 180.0f;
		float iHorz = initial_velocity * Mathf.Cos(rad);
		float iVert = initial_velocity * Mathf.Sin(rad);

		if(angle == 0.0f && initialY == 0.0f) myCase = Case.floor;
		else if(gravity == 0.0f && angle >= 0.0f) myCase = Case.noCol;
		else if(gravity == 0.0f && angle < 0.0f) myCase = Case.oneCol;
		else myCase = Case.manyCol;

		float dt;
		col_arr[0] = 0.0f;
		vel_arr[0] = new Vector2(iHorz, iVert);
		pos_arr[0] = new Vector2(-8.0f, initialY);
		switch(myCase){
			case Case.floor:
			case Case.noCol:
				// Não há colisões, assim preenchemos os vetores com as condições iniciais
				for(int i = 1; i < nCol; i++){
					col_arr[i] = 0.0f;
					vel_arr[i] = new Vector2(iHorz, iVert);
					pos_arr[i] = new Vector2(-8.0f, initialY);
				}
				break;
			case Case.oneCol:
				// Como há uma unica colisão, o vetor possui apenas as condições inicias
				// e as condições após a primeira colisão, repetidas até o fim.
				col_arr[1] = GetRoot(iVert, initialY);
				dt = col_arr[1] - col_arr[0];
				vel_arr[1] = new Vector2(
					Get_dx(vel_arr[0].x, dt),
					-Get_dy(vel_arr[0].y, dt)
				);
				pos_arr[1] = pos_arr[0] + new Vector2(
					Get_x(vel_arr[0].x, dt),
					Get_y(vel_arr[0].y, dt)
				);
				for(int i = 2; i < nCol; i++){ 
					col_arr[i] = col_arr[1]; 
					pos_arr[i] = pos_arr[1]; 
					vel_arr[i] = vel_arr[1]; 
				}
				break;
			case Case.manyCol:
				// Calcula todas as colisões até o fim do vetor
				col_arr[1] = GetRoot(iVert, initialY);
				for(int i = 1; i < nCol - 1; i++){
					dt = col_arr[i] - col_arr[i-1];
					vel_arr[i] = new Vector2(
						Get_dx(vel_arr[i-1].x, dt),
						-Get_dy(vel_arr[i-1].y, dt)
					);
					pos_arr[i] = pos_arr[i-1] + new Vector2(
						Get_x(vel_arr[i-1].x, dt),
						Get_y(vel_arr[i-1].y, dt)
					);
					col_arr[i+1] = col_arr[i] + GetRoot(vel_arr[i].y, pos_arr[i].y);
				}
				break;
		}

		OnReady?.Invoke(this, EventArgs.Empty);
		start = true;
	}

	/*-------------------------------------------------------------------------*\
	| Get_lastCol() retorna o indice da ultima colisão que ocorreu no tempo time|
	\*-------------------------------------------------------------------------*/
	private int Get_lastCol(float time){
        for (int i = 0; i < nCol - 2; i++) if (time < col_arr[i + 1]) return i;
		return nCol - 2;
	}

	/*-------------------------------------------------------------------------*\
	| Get_position() retorna a posição da particula após o tempo time do inicio |
	|                da simulação.                                              |
	\*-------------------------------------------------------------------------*/
	public Vector3 Get_position(float time) {
        int i = Get_lastCol(time);

		// Calcula a posição
        float x = Get_x(vel_arr[i].x, time - col_arr[i]);
        float y = Get_y(vel_arr[i].y, time - col_arr[i]);
		// Após o fim da ultima colisão não atualiza mais o y
		// Isso evita que a particula caia pelo chão
		if(i >= nCol - 2 && (myCase == Case.manyCol|| myCase == Case.floor))
			y = Get_y(vel_arr[i].y, 0.0f);

		// Transforma para ao sistema de coordenadas absoluto
        return pos_arr[i] + new Vector2(x, y);
    }

	private Vector3 Get_velocity(float time) {
        int i = Get_lastCol(time);

		// Calcula a velocidade
        float x = Get_dx(vel_arr[i].x, time - col_arr[i]);
        float y = Get_dy(vel_arr[i].y, time - col_arr[i]);
		// Após o fim da ultima colisão não atualiza mais o y
		if(i >= nCol - 2 && (myCase == Case.manyCol|| myCase == Case.floor))
			y = Get_dy(vel_arr[i].y, 0.0f);

        return new Vector2(x, y);
    }

	private Vector3 Get_acceleration(float time) {
        int i = Get_lastCol(time);

		// Calcula a aceleração
        float x = Get_ddx(vel_arr[i].x, time - col_arr[i]);
        float y = Get_ddy(vel_arr[i].y, time - col_arr[i]);
		// Após o fim da ultima colisão não atualiza mais o y
		if(i >= nCol - 2 && (myCase == Case.manyCol|| myCase == Case.floor))
			y = Get_ddy(vel_arr[i].y, 0.0f);

        return new Vector2(x, y);
    }

	/*-------------------------------------------------------------------------*\
	| Get_x() e Get_y() retornam as coordenadas da particula após o tempo time  |
	|                   desde a ultima colisão.                                 |
	\*-------------------------------------------------------------------------*/
	float Get_x(float initial_horz, float time) {
		if(viscosity == 0.0f) 
			return initial_horz * time;
		float x = tau * initial_horz;
		x *= 1.0f - Mathf.Exp(-time / tau);
		return(x);
	}

    float Get_y(float initial_vert, float time) {
		if(viscosity == 0.0f) 
			return initial_vert * time - gravity * time * time / 2.0f;
		float y = initial_vert * tau;
		y += gravity * tau * tau;
		y *= 1.0f - Mathf.Exp(-time/tau);
		y -= gravity * tau * time;
		return(y);
	}

	/*-------------------------------------------------------------------------*\
	| Get_dx() e Get_dy() retornam as coordenadas da velocidade da particula    |
	|                     após um tempo time desde a ultima colisão.            |
	\*-------------------------------------------------------------------------*/
	float Get_dx(float initial_horz, float time) {
		if(viscosity == 0.0f)
			return initial_horz;
		float dx = initial_horz * Mathf.Exp(-time/tau);
		return dx;
	}

	float Get_dy(float initial_vert, float time) {
		if(viscosity == 0.0f)
			return initial_vert - gravity * time;
		float dy = initial_vert + gravity * tau;
		dy *= Mathf.Exp(-time / tau);
		dy -= gravity * tau;
		return(dy);
	}

	/*-------------------------------------------------------------------------*\
	| Get_ddx() e Get_ddy() retornam as coordenadas da aceleração da particula  |
	|                     após um tempo time desde a ultima colisão.            |
	\*-------------------------------------------------------------------------*/
	float Get_ddx(float initial_horz, float time){
		float ddx = Mathf.Exp(-time / tau);
		ddx *= -initial_horz / tau;
		return ddx;
	}

	float Get_ddy(float initial_vert, float time){
		float ddy = Mathf.Exp(-time / tau);
		ddy *= - initial_vert / tau - gravity;
		return ddy;
	}

	/*-------------------------------------------------------------------------*\
	| Get_t0() retorna uma estimativa inicial para a raiz de Get_y(), sendo     |
	|          usada no método de Newton.                                       |
	\*-------------------------------------------------------------------------*/
	float Get_t0(float initial_vert) { // Dy == 0
		// Caso a trajétoria inicie com velocidade vertical negativa,
		// seu inicio é uma boa aproximação para a raiz desejada.
		if(initial_vert < 0) return 0.0f;

		// tm é o ponto em que Get_dy == 0, ou seja, o ponto de máximo da trajetória
		float tm = gravity * tau;
		tm /= initial_vert + gravity * tau;
		tm = Mathf.Log(tm);
		tm *= - tau;

		return 2.0f * tm + 0.1f; // Usa o tm para estimar um zero de Get_y()
	}

	/*-------------------------------------------------------------------------*\
	| GetRoot() retorna a raiz de Get_y(), assim conseguimos calcular a próxima |
	|           colisão da particula.                                           |
	\*-------------------------------------------------------------------------*/
	float GetRoot(float initial_vert, float initial_y) {
		float t;
		if(viscosity == 0.0f && gravity == 0.0f){ // Mov. linear
			t = -initial_y / initial_vert;
		} else if(viscosity == 0.0f){ // Mov. Parabolico
			t = initial_vert * initial_vert + 2.0f * gravity * initial_y;
			t = Mathf.Sqrt(t);
			t += initial_vert;
			t /= gravity;
		} else { // Método de Newton
			t = Get_t0(initial_vert);
			if(initial_vert < 0) t = 0.0f;
			for(int i = 0; i < nNewton; i++) {
				float deltaY = initial_y - rt.rect.height/2.0f - floorY;
				t -= (Get_y(initial_vert, t) + deltaY)/Get_dy(initial_vert, t);
			}
		}
		return t;
	}

    /*-------------------------------------------------------------------------*\
	| InstantiateVectors() cria LineRenderers e VectorTriangles para mostrar    |
	| visualmente os vetores aceleração e velocidade dos objetos de teste.      |
	\*-------------------------------------------------------------------------*/
    void InstantiateVectors()
    {
        velLineObject = new GameObject("Vetor Velocidade");
        velLineObject.transform.parent = transform;

        accLineObject = new GameObject("Vetor Aceleração");
        accLineObject.transform.parent = transform;

        lrVel = velLineObject.AddComponent<LineRenderer>();
        SetupLineRenderer(lrVel, vColor);
        GameObject velTriangle = Instantiate(vTriangle, velLineObject.transform);
        velTriangle.GetComponent<VectorTriangle>().vectorLine = lrVel;

        lrAcc = accLineObject.AddComponent<LineRenderer>();
        SetupLineRenderer(lrAcc, aColor);
        GameObject accTriangle = Instantiate(vTriangle, accLineObject.transform);
        accTriangle.GetComponent<VectorTriangle>().vectorLine = lrAcc;
    }

    /*-------------------------------------------------------------------------*\
	| SetupLineRenderer() atribui valores para um componente LineRenderer.      |
	\*-------------------------------------------------------------------------*/
    void SetupLineRenderer(LineRenderer lr, Color color)
    {
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.widthMultiplier = 0.05f;
        lr.startColor = color;
        lr.endColor = color;
    }

    /*-------------------------------------------------------------------------*\
	| ToggleVector() é chamado por UI e alterna a visibilidade de um vetor.     |
	\*-------------------------------------------------------------------------*/
    public void ToggleVector(bool acceleration)
	{
		if (acceleration)
			accLineObject.SetActive(!accLineObject.activeSelf);
		else
			velLineObject.SetActive(!velLineObject.activeSelf);
    }
}
