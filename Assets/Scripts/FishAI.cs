using UnityEngine;
using System.Collections;

public class FishAI : MonoBehaviour 
{
	private Character[] m_Players;
	private Clone[] m_Clones;

	Vector3 m_Velocity = new Vector3(0.0f,0.0f,0.0f);

	void Start () 
	{
		m_Players = GameObject.FindObjectsOfType<Character>();
		m_Clones = GameObject.FindObjectsOfType<Clone>();

		this.transform.localPosition = new Vector3(Random.Range(-20.0f, 20.0f), -5, Random.Range (-10.0f, 10.0f));


		foreach(Character t_Player in m_Players)
		{
			Vector3 t_Dist = (transform.position-t_Player.transform.position);
			t_Dist.y = 0;
			if(t_Dist.sqrMagnitude<30.0f)
				this.transform.position += t_Dist;
		}		

		foreach(Clone t_Player in m_Clones)
		{
			Vector3 t_Dist = (transform.position-t_Player.transform.position);
			t_Dist.y = 0;
			if(t_Dist.sqrMagnitude<30.0f)
				this.transform.position += t_Dist;
		}
	}

	void Update () 
	{
		m_Velocity += new Vector3(Random.Range(-.1f, .1f), 0, Random.Range(-.1f, .1f));

		// Stay in bounds
		if(transform.localPosition.x<-20.0f)
			m_Velocity.x += 1.0f;
		else if(transform.localPosition.x>20.0f)
			m_Velocity.x -= 1.0f;	

		if(transform.localPosition.z<-10.0f)
			m_Velocity.z += 1.0f;
		else if(transform.localPosition.z>10.0f)
			m_Velocity.z -= 1.0f;
		
		// and away from players 
		bool t_Terrified = false;
		foreach(Character t_Player in m_Players)
		{
			Vector3 t_Dist = (transform.position-t_Player.transform.position);
			t_Dist.y = 0;
			if(t_Dist.sqrMagnitude<30.0f)
			{
				m_Velocity += t_Dist;
				t_Terrified = true;
				print ("Terrified!");
			}
		}

		foreach(Clone t_Player in m_Clones)
		{
			Vector3 t_Dist = (transform.position-t_Player.transform.position);
			t_Dist.y = 0;
			if(t_Dist.sqrMagnitude<30.0f)
			{
				m_Velocity += t_Dist;
				t_Terrified = true;
				//print ("Terrified!");
			}
		}

		m_Velocity.Normalize();

		Vector3 t_Pos = transform.position;

		this.transform.position += m_Velocity*0.01f*((t_Terrified==true)?(3.0f):(1.0f));

		t_Pos = transform.position-t_Pos;

		//	transform.RotateAround(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);

		this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.LookRotation(m_Velocity), 0.1f);
	}
}
