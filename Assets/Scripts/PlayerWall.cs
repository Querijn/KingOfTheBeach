using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum Direction
{
	North,
	South,
	East,
	West
}
public class PlayerWall : MonoBehaviour {
	List<GameObject> m_ColliderObjects;
	List<SphereCollider> m_Colliders;
	bool m_HasRetrievedObjects;
	public Vector3 m_Bounds;
	public Direction m_ResolveDirection;
	Vector3 m_Max;
	Vector3 m_Min;
	// Use this for initialization
	void Start () {
		m_HasRetrievedObjects = false;
		Recalculate ();
	}
	
	public void Recalculate()
	{
		m_Max = transform.position + m_Bounds *0.5f;
		m_Min = transform.position - m_Bounds *0.5f;	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!m_HasRetrievedObjects)
		{
			m_HasRetrievedObjects = true;
			m_ColliderObjects = new List<GameObject>();
			m_Colliders = new List<SphereCollider>();
			Character[] t_Characters = GameObject.FindObjectsOfType<Character>();
			Clone[] t_Clones = GameObject.FindObjectsOfType<Clone>();
			for (int i = 0; i < t_Characters.Length; i ++)
			{
				m_ColliderObjects.Add (t_Characters[i].gameObject);	
				m_Colliders.Add (t_Characters[i].gameObject.GetComponent<SphereCollider>());
			}
			for (int i = 0; i < t_Clones.Length; i ++)
			{
				m_ColliderObjects.Add (t_Clones[i].gameObject);	
				m_Colliders.Add (t_Clones[i].gameObject.GetComponent<SphereCollider>());
			}	
		}
		CheckCollisions (0,2);
	}
	
	void LateUpdate()
	{	
		CheckCollisions (2,4);
	}
	
	void CheckCollisions(int a_Start, int a_End) {
		for (int i = a_Start; i < a_End; i ++)
		{
			SphereCollider t_Coll = m_Colliders[i];
			if (t_Coll.transform.position.x + t_Coll.radius > m_Min.x &&
				t_Coll.transform.position.x - t_Coll.radius < m_Max.x &&
				t_Coll.transform.position.z + t_Coll.radius > m_Min.z &&
				t_Coll.transform.position.z - t_Coll.radius < m_Max.z)
			{
				switch (m_ResolveDirection)
				{
					case Direction.North:
						t_Coll.transform.Translate(0,0, (m_Max.z + t_Coll.radius) - t_Coll.transform.position.z);
					break;
					case Direction.South:
						t_Coll.transform.Translate(0,0, (m_Min.z - t_Coll.radius) - t_Coll.transform.position.z);
					break;
					case Direction.East:
						t_Coll.transform.Translate((m_Max.x + t_Coll.radius) - t_Coll.transform.position.x,0,0);
					break;
					case Direction.West:
						t_Coll.transform.Translate((m_Min.x - t_Coll.radius) - t_Coll.transform.position.x,0,0);
					break;
				}
			}
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawLine (new Vector3(m_Min.x, transform.position.y, m_Min.z), new Vector3(m_Min.x, transform.position.y, m_Max.z));
		Gizmos.DrawLine (new Vector3(m_Min.x, transform.position.y, m_Max.z), new Vector3(m_Max.x, transform.position.y, m_Max.z));
		Gizmos.DrawLine (new Vector3(m_Max.x, transform.position.y, m_Max.z), new Vector3(m_Max.x, transform.position.y, m_Min.z));
		Gizmos.DrawLine (new Vector3(m_Max.x, transform.position.y, m_Min.z), new Vector3(m_Min.x, transform.position.y, m_Min.z));
		Gizmos.color = Color.white;
		
	}
}
