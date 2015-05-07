using UnityEngine;
using System.Collections;


public enum ColorType
{
	Blue,
	Orange
}

public enum ObjectType
{
	Player,
	Clone,
	Bullet
}

public class ColorCollider : MonoBehaviour {
	public ColorType m_ColorType;
	public ObjectType m_ObjectType;
	public bool m_ChecksForCollision = true;
	SphereCollider m_Collider = null;
	float m_Radius;
	// Use this for initialization
	void Start () {
		m_Collider = GetComponent<SphereCollider>();
		if (m_Collider != null)
		{
			m_Radius = m_Collider.radius;	
		}
		Start_Imp();
	}
	protected virtual void Start_Imp()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		if (m_ChecksForCollision)
			HandleCollisions ();
		
		Update_Imp();
	}
	
	protected virtual void Update_Imp()
	{
		
	}
	
	void HandleCollisions()
	{
		if (m_Collider)
		{
			Collider[] t_OtherColliders = Physics.OverlapSphere(transform.position, m_Collider.radius);			
			for (int i = 0; i < t_OtherColliders.Length; i ++)
			{
				ColorCollider t_OtherCol = t_OtherColliders[i].transform.GetComponent<ColorCollider>();	
				if (t_OtherCol != null && t_OtherColliders[i] != m_Collider)
				{
					Collide(t_OtherCol);	
				}
			}
		}
	}
	
	void Collide(ColorCollider a_OtherCol)
	{
		if (a_OtherCol != null)
		{
			if (a_OtherCol.m_ColorType != m_ColorType)
			{
				CollideWithOtherColor (a_OtherCol);
			}
			else
			{
				CollideWithSameColor (a_OtherCol);
			}
		}		
	}
	public float GetRadius()
	{
		return m_Radius;	
	}
	public virtual void GetBroken()
	{
		Debug.Log ("Breaking");
	}
	
	protected virtual void CollideWithSameColor(ColorCollider a_Other)
	{
		Debug.Log ("SameColor");

	}
	
	protected virtual void CollideWithOtherColor(ColorCollider a_Other)
	{
		Debug.Log ("DifferentColor");
	}
}
