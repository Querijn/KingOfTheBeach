using UnityEngine;
using System.Collections;

public class PickupManager : MonoBehaviour {
	
	public GameObject m_HealthPickup;
	public GameObject m_WallPickup;	
	public float m_PickupRadius;
	public float m_SpawnDistance;
	
	public Vector3 m_SpawnBoundsMax;
	public Vector3 m_SpawnBoundsMin;
	
	public float m_MinSpawnInterval;
	public float m_MaxSpawnInterval;
	
	float m_SpawnTime;
	float m_SpawnInterval;
	
	PatternScript m_PatternScript;
	GameManager m_GameManager;
	bool m_HasRetrievedObjects;
	float m_CompareRadius;
	Character[] m_Characters;
	Clone[] m_Clones;
	Character m_OrangeCharacter;
	Clone m_OrangeClone;
	Character m_BlueCharacter;
	Clone m_BlueClone;
	
	void Start () {
		m_GameManager = GameObject.FindObjectOfType<GameManager>();
		m_PatternScript = GameObject.FindObjectOfType<PatternScript>();
		m_HasRetrievedObjects = false;
		m_SpawnDistance *= m_SpawnDistance;
		CalcSpawnInterval ();
	}
	
	void CalcSpawnInterval()
	{
		m_SpawnInterval = Random.Range (m_MinSpawnInterval, m_MaxSpawnInterval);
		m_SpawnTime = 0;
	}
	
	void Update () {
		if (!m_HasRetrievedObjects)
		{
			m_Characters = GameObject.FindObjectsOfType<Character>();
			m_Clones = GameObject.FindObjectsOfType<Clone>();
			
			for (int i = 0; i < m_Characters.Length; i ++)
			{
				if (m_Characters[i].GetColorPlayer().m_ColorType == ColorType.Blue)
				{
					m_BlueCharacter = m_Characters[i];
				}
				else
				{
					m_OrangeCharacter = m_Characters[i];
				}
			}	
			for (int i = 0; i < m_Clones.Length; i ++)
			{
				if (m_Clones[i].GetCharacter ().GetColorPlayer().m_ColorType == ColorType.Blue)
				{
					m_BlueClone = m_Clones[i];
				}
				else
				{
					m_OrangeClone = m_Clones[i];
				}
			}
			
			m_CompareRadius = m_PickupRadius + m_Characters[0].GetComponent<SphereCollider>().radius;
			m_CompareRadius *= m_CompareRadius;
			m_HasRetrievedObjects = true;
			return;
		}
		if (m_GameManager.GetGameState() != GameState.InGame) return;
		m_SpawnTime += Time.deltaTime;
		if (m_SpawnTime > m_SpawnInterval)
		{
			CalcSpawnInterval();
			SpawnPickup (GetPickup ());
		}
		if (Input.GetKeyDown (KeyCode.G))
		{
			SpawnPickup (GetPickup ());
		}
		UpdatePickups ();
	}
	
	GameObject GetPickup()
	{
		int t_Check = Random.Range (0,2);
		switch (t_Check)
		{
			default:
				if (ShouldSpawnHealth())
				{
					return m_HealthPickup;
				}
				else if (ShouldSpawnWall ())
				{
					return m_WallPickup;
				}
			break;
			case 1:
				if (ShouldSpawnWall())
				{
					return m_WallPickup;
				}
				else if (ShouldSpawnHealth ())
				{
					return m_HealthPickup;
				}
			break;
		}
		return null;
	}
	
	bool ShouldSpawnHealth()
	{
		if (m_GameManager.GetPlayerInfo(ColorType.Blue).m_Lives != m_GameManager.m_StartLives ||
			m_GameManager.GetPlayerInfo(ColorType.Orange).m_Lives != m_GameManager.m_StartLives)
		{
			return true;	
		}
		return false;
	}
	bool ShouldSpawnWall()
	{
		return (m_GameManager.GetBulletWall(Direction.East).IsMoved () ||
				m_GameManager.GetBulletWall(Direction.West).IsMoved ());
	}
	
	void UpdatePickups()
	{
		Pickup[] t_Pickups = GameObject.FindObjectsOfType<Pickup>();
		
		for (int i = 0; i < t_Pickups.Length; i ++)
		{
			for (int j = 0; j < m_Characters.Length; j ++)
			{
				float t_Mag = Vector3.SqrMagnitude(m_Characters[j].transform.position - t_Pickups[i].transform.position);
				if (t_Mag < m_CompareRadius)
				{
					t_Pickups[i].PickupAction(m_Characters[j].GetColorPlayer().m_ColorType);
				}
			}
			for (int j = 0; j < m_Clones.Length; j ++)
			{
				float t_Mag = Vector3.SqrMagnitude(m_Clones[j].transform.position - t_Pickups[i].transform.position);
				if (t_Mag < m_CompareRadius)
				{
					t_Pickups[i].PickupAction(m_Clones[j].GetCharacter().GetColorPlayer().m_ColorType);
				}
			}
		}
	}
	
	void SpawnPickup(GameObject a_PickUp)
	{
		if (a_PickUp == null) return;
		BoardSide t_BoardSide = GetBoardSide();
		if (t_BoardSide != BoardSide.None)
		{
			ColorType t_Type = GetColorType (t_BoardSide);
			Character t_Char = null;
			Clone t_Clone = null;		
			if (t_Type == ColorType.Blue)
			{
				t_Char = m_BlueCharacter;
				t_Clone = m_BlueClone;
			}
			else
			{
				t_Char = m_OrangeCharacter;
				t_Clone = m_OrangeClone;
			}
			Vector3 t_SpawnLocation = GetSpawnLocation(t_BoardSide);
			if (!GoodSpawn(t_SpawnLocation, t_Clone, t_Char)) return;
			GameObject t_Pickup = GameObject.Instantiate (a_PickUp, t_SpawnLocation, Quaternion.identity) as GameObject;
			t_Pickup.GetComponent<Pickup>().Spawn (m_GameManager, t_BoardSide, GetColorType (t_BoardSide));
		}
	}
	
	BoardSide GetBoardSide()
	{
		return m_PatternScript.GetDirection();
	}
	
	ColorType GetColorType(BoardSide a_Side)
	{
		switch (a_Side)
		{
			case BoardSide.Left:
				return ColorType.Orange;
			break;
			case BoardSide.Right:
				return ColorType.Blue;
			break;
		}
		return ColorType.Orange;
	}
				
	Vector3 GetSpawnLocation(BoardSide a_Side)
	{
		Vector3 t_RetVec = Vector3.zero;
		switch (a_Side)
		{
			case BoardSide.Left:
				t_RetVec.Set (Random.Range (-m_SpawnBoundsMax.x, -m_SpawnBoundsMin.x),0.5f,Random.Range (-m_SpawnBoundsMax.z, m_SpawnBoundsMax.z));
				
				//t_RetVec = new Vector3(-5,0,0);
			break;
			case BoardSide.Right:
				t_RetVec.Set (Random.Range (m_SpawnBoundsMin.x, m_SpawnBoundsMax.x),0.5f,Random.Range (-m_SpawnBoundsMax.z, m_SpawnBoundsMax.z));
				//t_RetVec = new Vector3( 5,0,0);
			break;
		}
		return t_RetVec;
	}
	
	bool GoodSpawn(Vector3 a_Position, Clone a_Clone, Character a_Character)
	{
		float t_Dist = Vector3.SqrMagnitude(a_Position - a_Clone.transform.position);
		if (t_Dist < m_SpawnDistance) return false;
		t_Dist = Vector3.SqrMagnitude(a_Position - a_Character.transform.position);
		if (t_Dist < m_SpawnDistance) return false;
		return true;
	}
}
