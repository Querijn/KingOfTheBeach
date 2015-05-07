using UnityEngine;
using System.Collections;

public class Blurb : MonoBehaviour {

	public GameObject m_Mesh;
	public bool m_IsActive;
	public float m_TimeOut;
	float m_Timer;
	void Update()
	{
		if (m_IsActive)
		{
			m_Timer += Time.deltaTime;
			if (m_Timer > m_TimeOut)
			{
				SetMeshActive(false);	
			}
		}
	}
	
	public void SetMeshActive(bool a_MeshActive)
	{
		if (a_MeshActive)
		{
			m_IsActive = true;
			m_Mesh.SetActive (true);
			m_Timer = 0;
			
		}
		else
		{
			m_Mesh.SetActive (false);
			m_IsActive = false;	
		}
	}
}
