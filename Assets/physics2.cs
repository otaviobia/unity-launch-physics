using System;
using System.Collections;
using UnityEngine;

public class Physics2 : MonoBehaviour
{
	[SerializeField] private RectTransform floor;
	private float floorY;

	[SerializeField] private Settings UIM;
	[SerializeField] private float viscosity, angle, initial_velocity, gravity, mass, initialY;
	private Vector2 initialPos;

	private RectTransform rt;

	[HideInInspector] public float[] col_arr;
	[HideInInspector] private Vector2[] vel_arr, pos_arr;
	private float time;

	private float tau;
	private float lastCol, nextCol;
	private Vector2 initialVel;

	public event EventHandler<EventArgs> OnReady;

	[HideInInspector] public bool start;
	
	public int nCol;
	[SerializeField] private int nNewton;
	enum Case {noCol, oneCol, manyCol, floor};
	private Case myCase;

    // Start is called before the first frame update
    void Start()
    {
		rt = gameObject.GetComponent<RectTransform>();
		start = false;
		StartCoroutine(LateStart(1.0f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
		Reset();
	}

    public void Reset()
	{
		start = false;
		floorY = floor.position.y + floor.rect.height / 2.0f;
		
		viscosity = UIM.ui_viscosity;
		initial_velocity = UIM.ui_speed;
		angle = UIM.ui_angle;
		gravity = UIM.ui_gravity;
		mass = UIM.ui_mass;
		initialY = UIM.ui_height;
		
		tau = mass / viscosity;

		time = 0.0f;
		col_arr = new float[nCol];
		vel_arr = new Vector2[nCol];
		pos_arr = new Vector2[nCol];

		float rad = angle * Mathf.PI / 180.0f;
		float iHorz = initial_velocity * Mathf.Cos(rad);
		float iVert = initial_velocity * Mathf.Sin(rad);

		// Nunca colide
		// [TODO] viscosidade de gravidade == 0
		if(angle == 0.0f && initialY == 0.0f){
			for(int i = 0; i < nCol; i++){
				col_arr[i] = 0.0f;
				vel_arr[i] = new Vector2(iHorz, iVert);
				pos_arr[i] = new Vector2(-7.1f, initialY);
			}

			myCase = Case.floor;
			OnReady?.Invoke(this, EventArgs.Empty);
			start = true;
			return;
			
		} else if(gravity == 0.0f && angle >= 0){
			for(int i = 0; i < nCol; i++){
				col_arr[i] = 0.0f;
				vel_arr[i] = new Vector2(iHorz, iVert);
				pos_arr[i] = new Vector2(-7.1f, initialY);
			}

			myCase = Case.noCol;
			OnReady?.Invoke(this, EventArgs.Empty);
			start = true;
			return;
		} else if(gravity == 0.0f && angle < 0.0f){ // Uma colisão
			col_arr[0] = 0;  col_arr[1] = Newton(iVert, initialY);
			vel_arr[0] = new Vector2(iHorz, iVert);
			pos_arr[0] = new Vector2(-7.1f, initialY);
			float dt = col_arr[1] - col_arr[0];
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

			myCase = Case.oneCol;
			OnReady?.Invoke(this, EventArgs.Empty);
			start = true;
			return;
		}
		col_arr[0] = 0;  col_arr[1] = Newton(iVert, initialY);
		vel_arr[0] = new Vector2(iHorz, iVert);
		pos_arr[0] = new Vector2(-7.1f, initialY);
		for(int i = 1; i < nCol - 1; i++){
			float dt = col_arr[i] - col_arr[i-1];
			vel_arr[i] = new Vector2(
				Get_dx(vel_arr[i-1].x, dt),
				-Get_dy(vel_arr[i-1].y, dt)
			);
			pos_arr[i] = pos_arr[i-1] + new Vector2(
				Get_x(vel_arr[i-1].x, dt),
				Get_y(vel_arr[i-1].y, dt)
			);
			col_arr[i+1] = col_arr[i] + Newton(vel_arr[i].y, pos_arr[i].y);
		}
		myCase = Case.manyCol;
		OnReady?.Invoke(this, EventArgs.Empty);
		start = true;
	}

	void Update()
	{
		if(!start) return;
		rt.position = Get_position(time);

		time += Time.deltaTime;
	}

	public Vector3 Get_position(float time)
	{
        int i;
        for (i = 0; i < nCol - 2; i++)
        {
            if (time < col_arr[i + 1]) break;
        }
        float x = Get_x(vel_arr[i].x, time - col_arr[i]);
        float y = Get_y(vel_arr[i].y, time - col_arr[i]);
		if(i >= nCol - 2 && (myCase == Case.manyCol|| myCase == Case.floor)) y = Get_y(vel_arr[i].y, 0.0f);

        return pos_arr[i] + new Vector2(x, y);
    }

	float Get_x(float initial_horz, float time)
	{
		if(viscosity == 0.0f) {
			return initial_horz * time;
		}
		float x = tau * initial_horz;
		x *= 1.0f - Mathf.Exp(-time / tau);
		return(x);
	}

    float Get_y(float initial_vert, float time)
	{
		if(viscosity == 0.0f){
			return initial_vert * time - gravity * time * time / 2.0f;
		}
		float y = initial_vert * tau;
		y += gravity * tau * tau;
		y *= 1.0f - Mathf.Exp(-time/tau);
		y -= gravity * tau * time;
		return(y);
	}

	float Get_dx(float initial_horz, float time)
	{
		if(viscosity == 0.0f){
			return initial_horz;
		}
		float dx = initial_horz * Mathf.Exp(-time/tau);
		return dx;
	}

	float Get_dy(float initial_vert, float time)
	{
		if(viscosity == 0.0f){
			return initial_vert - gravity * time;
		}
		float dy = initial_vert + gravity * tau;
		dy *= Mathf.Exp(-time / tau);
		dy -= gravity * tau;
		return(dy);
	}
	
	float Get_t0(float initial_vert)
	{
		float t0 = gravity * tau;
		t0 /= initial_vert + gravity * tau;
		t0 = Mathf.Log(t0);
		t0 *= - tau;
		return(t0);
	}

	float Newton(float initial_vert, float initial_y)
	{
		if(viscosity == 0.0f){
			float k = initial_y;
			if(gravity == 0.0f){
				float t1 = -k / initial_vert;
				return t1;
			}
			float t0 = initial_vert * initial_vert + 2.0f * gravity * k;
			t0 = Mathf.Sqrt(t0);
			t0 += initial_vert;
			t0 /= gravity;
			return t0;
		}
		float t = 2.0f * Get_t0(initial_vert) + 0.1f; // angle = 0
		if(initial_vert < 0) t = 0;
		for(int i = 0; i < nNewton; i++)
		{
			float deltaY = initial_y - rt.rect.height/2.0f - floorY;
			t -= (Get_y(initial_vert, t) + deltaY)/Get_dy(initial_vert, t);
		}
		return t;
	}
}
