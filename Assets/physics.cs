using System.Collections;
using UnityEngine;

public class Physics : MonoBehaviour
{
	private Vector2 vel, acc;
	private RectTransform rt;

	[SerializeField] private Settings UIM;
	private float viscosity, initial_velocity, angle, gravity, mass, initialY;
	private int updateFrequency;
	
	[SerializeField] private RectTransform floor;
	private float floorY;
	private bool start;
	
	private float deltaT;

    void Start() {
		start = false;
		rt = gameObject.GetComponent<RectTransform>();
		StartCoroutine(LateStart(1.0f));
    }

    IEnumerator LateStart(float waitTime) {
        yield return new WaitForSeconds(waitTime);
		Reset();
	}

	/*-------------------------------------------------------------------------*\
	| Reset() atualiza as variaveis e reseta a simulação                        |
	\*-------------------------------------------------------------------------*/
	public void Reset(){
		start = false;
		floorY = floor.position.y + floor.rect.height / 2.0f;

		viscosity = UIM.ui_viscosity;
		initial_velocity = UIM.ui_speed;
		angle = UIM.ui_angle;
		gravity = UIM.ui_gravity;
		mass = UIM.ui_mass;
		initialY = UIM.ui_height;
		updateFrequency = (int) UIM.ui_timestep;

		deltaT = 0.0f;
		rt.position = new Vector2(-8.0f, initialY);

		acc = Vector2.zero;
		float rad = angle * Mathf.PI / 180.0f;
		vel = Vector2.up * Mathf.Sin(rad) + Vector2.right * Mathf.Cos(rad);
		vel *= initial_velocity;
		start = true;
	}
	
	/*-------------------------------------------------------------------------*\
	| ApplyForces() calcula as forças gravitacional e viscosa,                  |
	|               retornando a força resultante                               |
	\*-------------------------------------------------------------------------*/
	Vector2 ApplyForces() {
		Vector2 Fg = Vector2.down * gravity * mass;
		Vector2 Fv = -vel * viscosity;
		return(Fg + Fv);
	}


	/*-------------------------------------------------------------------------*\
	| UpdatePos() atualiza a particula após uma pequena variação de tempo dt,   |
	|             usando a integração de verlet para calcular as novas condições|
	\*-------------------------------------------------------------------------*/
	void UpdatePos(float dt){
		Vector2 pos = transform.position;

		// Verlet Integration
		Vector2 newPos = pos + vel*dt + acc*(dt*dt/2.0f);
		Vector2 newAcc = ApplyForces() / mass;
		Vector2 newVel = vel + (acc + newAcc)*(dt/2.0f);

		// Resolve Collisions
		float bottomY = newPos.y - rt.rect.height / 2.0f; 
		if(bottomY < floorY){ // Torricelli
			float deltaS = floorY - bottomY;
			float v0 = Mathf.Sqrt(newVel.y * newVel.y + 2.0f * newAcc.y * deltaS);
			newPos += Vector2.up * deltaS;
			newVel += Vector2.up * (v0 - newVel.y);
		}
		
		// Update variables
		transform.position = newPos;
		vel = newVel;
		acc = newAcc;
		
		// Update energy
		float kinectEnergy = mass * vel.sqrMagnitude / 2.0f;
		float potentialEnergy = mass * gravity * (bottomY - floorY);
	}

    void Update() {
		if(!start) return;
		deltaT += Time.deltaTime;
		float dt =  1.0f / updateFrequency;
		while(deltaT >= dt){
			UpdatePos(dt);
			deltaT -= dt;
		}
    }
}
