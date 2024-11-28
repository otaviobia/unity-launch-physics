using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physics : MonoBehaviour
{
	private Vector2 vel;
	private Vector2 acc;

	[SerializeField] private float gravity;
	[SerializeField, Range(0.0f, 90.0f)] private float angle;
	[SerializeField] private float initial_velocity;
	[SerializeField] private float viscosity;
	
	[SerializeField] private RectTransform floor;
	private float floorY;
	private float initialY;
	private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
		floorY = floor.position.y + floor.rect.height / 2.0f;
		rt = gameObject.GetComponent<RectTransform>();
		
		acc = ApplyForces();
		float rad = angle * Mathf.PI / 180.0f;
		vel = Vector2.up * Mathf.Sin(rad) + Vector2.right * Mathf.Cos(rad);
		vel *= initial_velocity;

		initialY = rt.position.y;
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
