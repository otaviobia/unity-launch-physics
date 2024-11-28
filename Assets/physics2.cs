using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physics2 : MonoBehaviour
{
	[SerializeField] private float gravity;
	[SerializeField, Range(-90.0f, 90.0f)] private float angle;
	[SerializeField] private float initial_velocity;
	[SerializeField] private float viscosity;
	
	[SerializeField] private RectTransform floor;
	private float floorY;
	private Vector2 initialPos;
	private RectTransform rt;
	private float tau;

	private float lastCol;
	[SerializeField] private float nextCol;
	private Vector2 initialVel;

    // Start is called before the first frame update
    void Start()
    {
		floorY = floor.position.y + floor.rect.height / 2.0f;
		rt = gameObject.GetComponent<RectTransform>();
		
		initialPos = rt.position;
		tau = 1.0f / viscosity;

		float rad = angle * Mathf.PI / 180.0f;
		float iHorz = initial_velocity * Mathf.Cos(rad);
		float iVert = initial_velocity * Mathf.Sin(rad);
		
		initialVel = new Vector2(iHorz, iVert);

		lastCol = 0;
		nextCol = Newton(initialVel.y);
    }
	
	float Get_x(float initial_horz, float time){
		float x = tau * initial_horz;
		x *= 1.0f - Mathf.Exp(-time / tau);
		return(x);
	}

	float Get_y(float initial_vert, float time){
		float y = initial_vert * tau;
		y += gravity * tau * tau;
		y *= 1.0f - Mathf.Exp(-time/tau);
		y -= gravity * tau * time;
		return(y);
	}

    // Update is called once per frame
    void Update()
    {

		float x = Get_x(initialVel.x, Time.time - lastCol);
		float y = Get_y(initialVel.y, Time.time - lastCol);
		
		rt.position = initialPos + new Vector2(x, y);
		if(Time.time - lastCol >= nextCol){
			initialVel = new Vector2(
					Get_dx(initialVel.x, Time.time - lastCol),
					-Get_dy(initialVel.y, Time.time - lastCol)
			);
			initialPos = rt.position;
			lastCol += nextCol;
			nextCol = Newton(initialVel.y);
		}

	}
	
	float Get_dx(float initial_horz, float time){
		float dx = initial_horz * Mathf.Exp(-time/tau);
		return dx;
	}

	float Get_dy(float initial_vert, float time){
		float dy = initial_vert + gravity * tau;
		dy *= Mathf.Exp(-time / tau);
		dy -= gravity * tau;
		return(dy);
	}
	
	float Get_t0(float initial_vert){
		float t0 = gravity * tau;
		t0 /= initial_vert + gravity * tau;
		t0 = Mathf.Log(t0);
		t0 *= - tau;
		return(t0);
	}

	float Newton(float initial_vert){
		float t = 2.0f * Get_t0(initial_vert) + 0.1f; // angle = 0
		if(initial_vert < 0) t = 0;
		for(int i = 0; i < 20; i++){
			float deltaY = initialPos.y - rt.rect.height/2.0f - floorY;
			t -= (Get_y(initial_vert, t) + deltaY)/Get_dy(initial_vert, t);
		}
		return t;
	}

	
}
