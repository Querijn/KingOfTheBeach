using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletWall : MonoBehaviour {
	GameObject[] m_ColliderObjects;
	public Vector3 m_Bounds;
	public Direction m_ResolveDirection;
	Vector3 m_Max;
	Vector3 m_Min;
	Vector3 m_TargetPosition;
	public float m_Speed;
	public float m_SmoothSpeed = 0.1f;
	public float m_DeathDistance = 5;
	PlayerWall m_PlayerWall;
	float m_StartX;
	
	private GameManager m_GameManager;
	
	// Use this for initialization
	void Start () {
		m_GameManager = GameObject.FindObjectOfType<GameManager>();
		m_Max = transform.position + m_Bounds * 0.5f;
		m_Min = transform.position - m_Bounds * 0.5f;
		m_StartX = transform.position.x;
		m_PlayerWall = GetComponent<PlayerWall>();
		m_TargetPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		m_ColliderObjects = GameObject.FindGameObjectsWithTag("Bullet");
		CheckCollisions ();
		if (m_GameManager.GetGameState() == GameState.InGame)
		{
			if (transform.position.x != m_TargetPosition.x)
			{
				transform.Translate((m_TargetPosition.x - transform.position.x) * m_SmoothSpeed, 0, 0);
				m_Max = transform.position + m_Bounds * 0.5f;
				m_Min = transform.position - m_Bounds * 0.5f;
				m_PlayerWall.Recalculate();
			}
		}
	}
	
	void CheckCollisions() {
		if (m_GameManager.m_Invulnerable) return;
		for (int i = 0; i < m_ColliderObjects.Length; i ++)
		{
			switch (m_ResolveDirection)
			{
				case Direction.East:
					if (m_ColliderObjects[i].transform.position.x < m_Max.x )
					{
						Destroy (m_ColliderObjects[i]);
						m_TargetPosition.x += m_Speed;
						if (m_Max.x > -m_DeathDistance)
						{
							//m_TargetPosition.x = -DeathDistance-m_Max.x, 0, 0);
							m_Max = transform.position + m_Bounds * 0.5f;
							m_GameManager.GoGameOver ();
						}
					}
				break;
				case Direction.West:
					if (m_ColliderObjects[i].transform.position.x > m_Min.x)
					{
						Destroy (m_ColliderObjects[i]);
						m_TargetPosition.x -= m_Speed;						
						if (m_Min.x < m_DeathDistance)
						{
							//transform.Translate(-m_Min.x, 0, 0);
							m_Min = transform.position - m_Bounds * 0.5f;
							m_GameManager.GoGameOver ();
						}
					}
				break;
			}
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine (new Vector3(m_Min.x, transform.position.y, m_Min.z), new Vector3(m_Min.x, transform.position.y, m_Max.z));
		Gizmos.DrawLine (new Vector3(m_Min.x, transform.position.y, m_Max.z), new Vector3(m_Max.x, transform.position.y, m_Max.z));
		Gizmos.DrawLine (new Vector3(m_Max.x, transform.position.y, m_Max.z), new Vector3(m_Max.x, transform.position.y, m_Min.z));
		Gizmos.DrawLine (new Vector3(m_Max.x, transform.position.y, m_Min.z), new Vector3(m_Min.x, transform.position.y, m_Min.z));
		Gizmos.color = Color.white;		
	}
	
	public void ReduceTargetPosition(float a_Val)
	{
		switch (m_ResolveDirection)
		{
			case Direction.East:
				m_TargetPosition.x -= a_Val;
				if (m_TargetPosition.x < m_StartX)
				{
					m_TargetPosition.x = m_StartX;
				}
			break;
			case Direction.West:
				m_TargetPosition.x += a_Val;
				if (m_TargetPosition.x > m_StartX)
				{
					m_TargetPosition.x = m_StartX;
				}
			break;
		}					
	}
	
	public bool IsMoved()
	{
		return (m_TargetPosition.x != m_StartX);	
	}
}
