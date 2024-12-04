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

	[SerializeField] private float[] col_arr;
	[SerializeField] private Vector2[] vel_arr, pos_arr;
	private float time;

	private float tau;
	private float lastCol, nextCol;
	private Vector2 initialVel;

	public event EventHandler<EventArgs> OnReady;

	[HideInInspector] public bool start;
	
	// [TODO] Viscosity = 0.0f

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
		col_arr = new float[50];
		vel_arr = new Vector2[50];
		pos_arr = new Vector2[50];

		float rad = angle * Mathf.PI / 180.0f;
		float iHorz = initial_velocity * Mathf.Cos(rad);
		float iVert = initial_velocity * Mathf.Sin(rad);

		col_arr[0] = 0;  col_arr[1] = Newton(iVert, initialY);
		vel_arr[0] = new Vector2(iHorz, iVert);
		pos_arr[0] = new Vector2(-7.1f, initialY);
		for(int i = 1; i < 49; i++){
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
        for (i = 0; i < 48; i++)
        {
            if (time < col_arr[i + 1]) break;
        }
		if(i >= 48) time = col_arr[i];
        float x = Get_x(vel_arr[i].x, time - col_arr[i]);
        float y = Get_y(vel_arr[i].y, time - col_arr[i]);

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
			return 2.0f * initial_vert / gravity;
		}
		float t = 2.0f * Get_t0(initial_vert) + 0.1f; // angle = 0
		if(initial_vert < 0) t = 0;
		for(int i = 0; i < 20; i++)
		{
			float deltaY = initial_y - rt.rect.height/2.0f - floorY;
			t -= (Get_y(initial_vert, t) + deltaY)/Get_dy(initial_vert, t);
		}
		return t;
	}
}
