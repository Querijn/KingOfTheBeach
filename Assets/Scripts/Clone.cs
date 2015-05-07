using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class Clone : MonoBehaviour 
{
	public GameObject m_Parent;
	Character m_Character;
	public bool m_UseLineSymmetry = true;
	public GameObject m_Mesh;
	public HealthCircle m_HealthCircle;
	private GameManager m_GameManager;
	// Use this for initialization
	void Start () 
	{
		m_GameManager = GameObject.FindObjectOfType<GameManager>();
		//m_Parent = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_Parent != null)
		{
			transform.position = new Vector3(-m_Parent.transform.position.x, m_Parent.transform.position.y, m_Parent.transform.position.z*((m_UseLineSymmetry)?(1):(-1)));
			if (m_Mesh != null)
			{
				m_Mesh.transform.rotation = new Quaternion(m_Character.m_Mesh.transform.rotation.x, m_Character.m_Mesh.transform.rotation.y, m_Character.m_Mesh.transform.rotation.z, m_Character.m_Mesh.transform.rotation.w);
				m_Mesh.transform.Rotate (0,180,0,Space.World);
				
			}
		}
		if (m_Character != null)
		{
			if (m_HealthCircle != null)
			{
				float t_Health = 1.0f - (m_GameManager.GetPlayerInfo(m_Character.GetColorPlayer().m_ColorType).m_Lives / m_GameManager.m_StartLives);
				m_HealthCircle.m_CutoffValue = t_Health;
			}	
		}
	}
	
	public Character GetCharacter()
	{
		return m_Character;	
	}

	public void SetParent(GameObject a_Parent)
	{
		this.m_Parent = a_Parent;
		m_Character = a_Parent.GetComponent<Character>();
	}
}
