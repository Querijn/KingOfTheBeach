using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
	
	public Transform m_North;
	public Transform m_South;
	public Transform m_East;
	public Transform m_West;
	
	float m_MinX;
	float m_MaxX;
	float m_MinZ;
	float m_MaxZ;
	// Use this for initialization
	void Start () {
		m_MinX = m_West.position.x;
		m_MaxX = m_East.position.x;
		m_MinZ = m_South.position.z;
		m_MaxZ = m_North.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		CheckBullets();
	}
	
	void CheckBullets()
	{
		ColorBullet[] t_Bullets = GameObject.FindObjectsOfType<ColorBullet>();
		for (int i = 0; i < t_Bullets.Length; i ++)
		{
			ColorBullet t_Bullet = t_Bullets[i];
			float t_Radius = t_Bullet.GetRadius();
			if (t_Bullet.transform.position.x - t_Radius < m_MinX ||
				t_Bullet.transform.position.x + t_Radius > m_MaxX ||
				t_Bullet.transform.position.z - t_Radius < m_MinZ ||
				t_Bullet.transform.position.z + t_Radius > m_MaxZ)
			{
				t_Bullet.GetBroken ();	
			}
		}
	}
}
