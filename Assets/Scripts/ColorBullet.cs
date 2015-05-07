using UnityEngine;
using System.Collections;

public class ColorBullet : ColorCollider 
{
	public float m_Speed = 50.0f;
	public float m_RotateSpeedMult;
	float m_RotateSpeed;
	
	
	public override void GetBroken()
	{
		Destroy (this.gameObject);
	}
	
	protected override void CollideWithSameColor(ColorCollider a_Other)
	{
		
	}

	protected override void CollideWithOtherColor(ColorCollider a_Other)
	{
		
	}

	void Update()
	{
		this.transform.position += new Vector3(m_Speed*((transform.position.x<=0.0f)?(-1.0f):(1.0f))*Time.deltaTime*0.01f, 0.0f, 0.0f); 
		transform.Rotate(m_RotateSpeed,0,0,Space.World);
	}

	public void SetSpeed(float a_Speed)
	{
		m_Speed = a_Speed;
		float t_Multiply = 1;
		if (Random.Range (0,2) == 0)
		{
			t_Multiply = -1;
		}
		m_RotateSpeed = a_Speed * 0.01f * t_Multiply * m_RotateSpeedMult;
		transform.rotation = Quaternion.LookRotation (Vector3.forward, new Vector3((transform.position.x<=0.0f)?(-1.0f):(1.0f), 0, 0));
	}
}
