using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physics : MonoBehaviour
{
	private Vector2 vel, acc;
	private RectTransform rt;

	[SerializeField] private UIManager UIM;
	private float viscosity, initial_velocity, angle, gravity, mass, initialY;
	
	[SerializeField] private RectTransform floor;
	private float floorY;


    // Start is called before the first frame update
    void Start()
    {
		rt = gameObject.GetComponent<RectTransform>();
		StartCoroutine(LateStart(1.0f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
		Reset();
	}

	public void Reset(){
		floorY = floor.position.y + floor.rect.height / 2.0f;

		viscosity = UIM.ui_viscosity;
		initial_velocity = UIM.ui_speed;
		angle = UIM.ui_angle;
		gravity = UIM.ui_gravity;
		mass = 1.0f;
		initialY = -3.8f;

		rt.position = new Vector2(-7.1f, -3.8f);

		acc = ApplyForces();
		float rad = angle * Mathf.PI / 180.0f;
		vel = Vector2.up * Mathf.Sin(rad) + Vector2.right * Mathf.Cos(rad);
		vel *= initial_velocity;
	}
	
	Vector2 ApplyForces()
	{
		Vector2 Fg = Vector2.down * gravity;
		Vector2 Fv = -vel * viscosity;
		return(Fg + Fv);
	}

    // Update is called once per frame
    void Update()
    {
		// Verlet Integration
		float dt = Time.deltaTime;
		Vector2 pos = transform.position;

		Vector2 newPos = pos + vel*dt + acc*(dt*dt/2.0f);
		Vector2 newAcc = ApplyForces();
		Vector2 newVel = vel + (acc + newAcc)*(dt/2.0f);

		// Resolve Collisions
		float bottomY = newPos.y - rt.rect.height / 2.0f; 
		if(bottomY <= floorY){ // Torricelli
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
		float kinectEnergy = vel.sqrMagnitude / 2.0f;
		float potentialEnergy = gravity * (bottomY - floorY);
		
		float tau = 1.0f / viscosity;
		float kE = initial_velocity * initial_velocity;
		kE -= gravity * gravity * tau * tau;
		kE *= Mathf.Exp(-2.0f * Time.time / tau);
		kE += tau * tau * gravity * gravity;
		kE *= 1.0f / 2.0f;

		float pE = initial_velocity * Mathf.Sin(angle * Mathf.PI / 180.0f);
		pE *= tau;
		pE += gravity * tau * tau;
		pE *= 1 - Mathf.Exp(-Time.time / tau);
		pE -= gravity * tau * Time.time;
		pE *= 1.0f * gravity;
    }

	
}
