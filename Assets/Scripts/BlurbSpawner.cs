using UnityEngine;
using System.Collections;

public class BlurbSpawner : MonoBehaviour {
	public Blurb[] m_PositiveBlurbs;
	public Blurb[] m_NegativeBlurbs;
	
	
	void Start()
	{
		for (int i = 0; i < m_PositiveBlurbs.Length; i++)
		{
			m_PositiveBlurbs[i].SetMeshActive (false);	
		}
		for (int i = 0; i < m_NegativeBlurbs.Length; i++)
		{
			m_NegativeBlurbs[i].SetMeshActive (false);	
		}
	}
	
	
	public void SpawnBlurb(Vector3 a_Position, bool a_Positive)
	{		
		if (a_Positive)
		{
			int t_ID = Random.Range (0, m_PositiveBlurbs.Length);	
			m_PositiveBlurbs[t_ID].SetMeshActive (true);
			m_PositiveBlurbs[t_ID].transform.position = a_Position + transform.position;
			m_PositiveBlurbs[t_ID].transform.rotation = Quaternion.Euler (0,Random.Range (0,360),0);
		}
		else
		{
			int t_ID = Random.Range (0, m_NegativeBlurbs.Length);
			m_NegativeBlurbs[t_ID].SetMeshActive (true);
			m_NegativeBlurbs[t_ID].transform.position = a_Position + transform.position;
			m_NegativeBlurbs[t_ID].transform.rotation = Quaternion.Euler (0,Random.Range (0,360),0);
		}
	}
}
