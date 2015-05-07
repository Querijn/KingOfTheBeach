using UnityEngine;
using System.Collections;

public enum GameState
{
	PreGame,
	InGame,
	GameOver
}

public class PlayerInfo
{
	public int m_Points;
	public float m_Lives;
	public ColorType m_ColorType;
	public Character m_Character;
	public float m_InvulnTime;
	public void Hit()
	{
		IsInvulnerable = true;
		m_InvulnTime = 0;
	}
	public bool IsInvulnerable;
}
[System.Serializable]
public class RelGUIBox
{
	public Rect m_RelativeRect;
	public Rect m_AbsoluteRect;
	public void Scale()
	{
		m_AbsoluteRect.x = m_RelativeRect.x * Screen.width;	
		m_AbsoluteRect.y = m_RelativeRect.y * Screen.height;
		m_AbsoluteRect.width = m_RelativeRect.width * Screen.height;
		m_AbsoluteRect.height = m_RelativeRect.height * Screen.height;
	}
}

public class GameManager : MonoBehaviour {
	public GUIStyle m_Style;
	GameState m_State;
	PlayerInfo m_OrangePlayerInfo;
	PlayerInfo m_BluePlayerInfo;
	BulletWall m_EastBulletWall;
	BulletWall m_WestBulletWall;
	BlurbSpawner m_BlurbSpawner;
	
	public GameObject m_OrangeStart;
	public GameObject m_BlueStart;
	
	public float m_StartLives;
	public float m_MaxInvulnTime;
	public float m_BulletDamage;
	public float m_PlayerCloneDamage;
	public Color m_BlueColor;
	public Color m_OrangeColor;
	
	public RelGUIBox m_ScoreBox;
	
	public Texture m_OrangeStartTex;
	public Texture m_BlueStartTex;
	
	public Texture m_OrangeEndTex;
	public Texture m_BlueEndTex;
	public Texture m_LoseOrangeEndTex;
	public Texture m_LoseBlueEndTex;
	public Texture m_WinOrangeEndTex;
	public Texture m_WinBlueEndTex;
	public RelGUIBox m_EndTexBox;
	public RelGUIBox m_EndScoreBox;
	
	public Texture m_RestartTex;
	public RelGUIBox m_RestartBox;
	
	public Texture m_LogoTex;
	public RelGUIBox m_LogoBox;
	
	public bool m_Invulnerable = false;
	
	//GUI
	Matrix4x4 m_OriginalMatrix;
	float m_RotatedWidthStartAlign;
	
	
	// Use this for initialization
	void Start () {
		m_State = GameState.PreGame;
		m_BlurbSpawner = GameObject.FindObjectOfType<BlurbSpawner>();
		BulletWall[] t_BulletWalls = GameObject.FindObjectsOfType<BulletWall>();
		
		for (int i = 0; i < t_BulletWalls.Length; i ++)
		{
			if (t_BulletWalls[i].m_ResolveDirection == Direction.East)
			{
				m_WestBulletWall = t_BulletWalls[i];
			}
			else
			{
				m_EastBulletWall = t_BulletWalls[i];
			}
		}
		
		m_OrangePlayerInfo = new PlayerInfo();
		m_OrangePlayerInfo.m_Points = 0;
		m_OrangePlayerInfo.m_ColorType = ColorType.Orange;
		m_OrangePlayerInfo.m_Lives = m_StartLives;
		m_BluePlayerInfo = new PlayerInfo();
		m_BluePlayerInfo.m_Points = 0;
		m_BluePlayerInfo.m_ColorType = ColorType.Blue;
		m_BluePlayerInfo.m_Lives = m_StartLives;
		
		Character[] t_Characters = GameObject.FindObjectsOfType<Character>();
		for (int i = 0; i < t_Characters.Length; i++)
		{
			if (t_Characters[i].transform.GetComponent<ColorPlayer>().m_ColorType == ColorType.Orange)
			{
				m_OrangePlayerInfo.m_Character = t_Characters[i];
			}
			else
			{
				m_BluePlayerInfo.m_Character = t_Characters[i];
			}
		}
		
		m_RotatedWidthStartAlign = Screen.width * 0.5f - Screen.height * 0.5f;
		m_EndTexBox.Scale();
		m_EndScoreBox.Scale();
		m_ScoreBox.Scale();
		m_RestartBox.Scale ();
		m_LogoBox.Scale ();
	}
	
	// Update is called once per frame
	void Update () {
		switch (m_State)
		{
			case GameState.PreGame:
				PreGameUpdate();
			break;
			case GameState.InGame:
				InGameUpdate();
			break;
			case GameState.GameOver:
				GameOverUpdate();
			break;
		}
	}
	
	void PreGameUpdate()
	{
		if ((m_OrangePlayerInfo.m_Character.IsControlled() && m_BluePlayerInfo.m_Character.IsControlled()) || Input.GetKeyDown (KeyCode.Space))
		{
			//m_State = GameState.InGame;	
		}
		float t_Distance = 3;
		if (m_OrangeStart != null)
		{
			if (Vector3.Distance (m_OrangePlayerInfo.m_Character.transform.position, m_OrangeStart.transform.position) < t_Distance)
			{
				Destroy(m_OrangeStart);
				m_OrangeStart = null;
			}
		}
		if (m_BlueStart != null)
		{
			if (Vector3.Distance (m_BluePlayerInfo.m_Character.transform.position, m_BlueStart.transform.position) < t_Distance)
			{
				Destroy(m_BlueStart);
				m_BlueStart = null;
			}
		}
		if (m_BlueStart == null &&
			m_OrangeStart == null)
		{
			m_State = GameState.InGame;	
		}
	}
	void InGameUpdate()
	{
		if (m_OrangePlayerInfo.IsInvulnerable)
		{
			m_OrangePlayerInfo.m_InvulnTime += Time.deltaTime;
			if (m_OrangePlayerInfo.m_InvulnTime > m_MaxInvulnTime)
			{
				m_OrangePlayerInfo.IsInvulnerable = false;
			}	
		}
		if (m_BluePlayerInfo.IsInvulnerable)
		{	
			m_BluePlayerInfo.m_InvulnTime += Time.deltaTime;
			if (m_BluePlayerInfo.m_InvulnTime > m_MaxInvulnTime)
			{
				m_BluePlayerInfo.IsInvulnerable = false;
			}	
		}
		if (m_BluePlayerInfo.m_Lives <= 0 || m_OrangePlayerInfo.m_Lives <= 0)
		{
			if(!m_Invulnerable) GoGameOver ();
		}
	}
	void GameOverUpdate()
	{
		
	}
	
	void OnGUI()
	{
		StartGUIOperations ();
		DebugGUI ();
		switch (m_State)
		{
			case GameState.PreGame:
				PreGameGUI ();
			break;
			case GameState.InGame:
				InGameGUI ();
			break;
			case GameState.GameOver:
				GameOverGUI();
			break;
		}
		ResetGUIDirection();
	}
	
	void PreGameGUI()
	{
		SetGUIDirection (BoardSide.Right);
		GUI.DrawTexture(new Rect(Screen.width * 0.5f - m_LogoBox.m_AbsoluteRect.width * 0.5f,
								 Screen.height * 0.5f - m_LogoBox.m_AbsoluteRect.height * 0.5f,
								 m_LogoBox.m_AbsoluteRect.width, m_LogoBox.m_AbsoluteRect.height), m_LogoTex);
		
		SetGUIDirection (BoardSide.Left);		
		GUI.DrawTexture (new Rect(m_RotatedWidthStartAlign + m_EndTexBox.m_AbsoluteRect.x, m_EndTexBox.m_AbsoluteRect.y - m_EndTexBox.m_AbsoluteRect.height * 0.5f, m_EndTexBox.m_AbsoluteRect.width, m_EndTexBox.m_AbsoluteRect.height), m_OrangeStartTex);
		SetGUIDirection (BoardSide.Right);
		GUI.DrawTexture (new Rect(m_RotatedWidthStartAlign + m_EndTexBox.m_AbsoluteRect.x, m_EndTexBox.m_AbsoluteRect.y - m_EndTexBox.m_AbsoluteRect.height * 0.5f, m_EndTexBox.m_AbsoluteRect.width, m_EndTexBox.m_AbsoluteRect.height), m_BlueStartTex);
		
	}
	void InGameGUI()
	{
		DrawPoints ();
	}
	void GameOverGUI()
	{
		Texture t_BlueTex;
		Texture t_OrangeTex;
		if (m_BluePlayerInfo.m_Points > m_OrangePlayerInfo.m_Points)
		{
			t_BlueTex = m_WinBlueEndTex;
			t_OrangeTex = m_LoseOrangeEndTex;
		}
		else if (m_BluePlayerInfo.m_Points == m_OrangePlayerInfo.m_Points)
		{
			t_BlueTex = m_WinBlueEndTex;
			t_OrangeTex = m_WinOrangeEndTex;
		}
		else
		{
			t_BlueTex = m_LoseBlueEndTex;
			t_OrangeTex = m_WinOrangeEndTex;
		}
		
		SetGUIDirection (BoardSide.Left);
		m_Style.normal.textColor = m_OrangeColor;
		
		GUI.DrawTexture (new Rect(m_RotatedWidthStartAlign + m_EndTexBox.m_AbsoluteRect.x, m_EndTexBox.m_AbsoluteRect.y - m_EndTexBox.m_AbsoluteRect.height * 0.5f, m_EndTexBox.m_AbsoluteRect.width, m_EndTexBox.m_AbsoluteRect.height), t_OrangeTex);
		SetGUIDirection (BoardSide.Right);
		m_Style.normal.textColor = m_BlueColor;
		GUI.DrawTexture (new Rect(m_RotatedWidthStartAlign + m_EndTexBox.m_AbsoluteRect.x, m_EndTexBox.m_AbsoluteRect.y - m_EndTexBox.m_AbsoluteRect.height * 0.5f, m_EndTexBox.m_AbsoluteRect.width, m_EndTexBox.m_AbsoluteRect.height), t_BlueTex);
		
		DrawGameOverPoints ();
		ResetGUIDirection();
		if (GUI.Button (new Rect(Screen.width * 0.5f - m_RestartBox.m_AbsoluteRect.width * 0.5f,
								 Screen.height * 0.5f - m_RestartBox.m_AbsoluteRect.height * 0.5f,
								 m_RestartBox.m_AbsoluteRect.width, m_RestartBox.m_AbsoluteRect.height), m_RestartTex, m_Style))
		{
			Application.LoadLevel (Application.loadedLevel);
		}
	}
	void StartGUIOperations()
	{
		m_OriginalMatrix = GUI.matrix;
	}
	void SetGUIDirection(BoardSide a_BoardSide)
	{
		switch (a_BoardSide)
		{
			case BoardSide.Left:
				{
					ResetGUIDirection ();
					Vector2 t_Pivot = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
					GUIUtility.RotateAroundPivot(90, t_Pivot);
				}
			break;
			case BoardSide.Right:
				{
					ResetGUIDirection ();
					Vector2 t_Pivot = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
					GUIUtility.RotateAroundPivot(-90, t_Pivot);
				}
			break;
		}
	}
	void ResetGUIDirection()
	{
		GUI.matrix = m_OriginalMatrix;	
	}
	
	void DrawPoints()
	{
		SetGUIDirection(BoardSide.Left);
		m_Style.normal.textColor = m_OrangeColor;
		//GUI.Label (new Rect(m_RotatedWidthStartAlign + Screen.height * 0.05f, Screen.height * 0.5f + Screen.height * 0.1f, 40, 40), m_OrangePlayerInfo.m_Points.ToString (), m_Style);
		GUI.Label (new Rect(m_RotatedWidthStartAlign + m_ScoreBox.m_AbsoluteRect.x, m_ScoreBox.m_AbsoluteRect.y - m_ScoreBox.m_AbsoluteRect.height * 0.5f, m_ScoreBox.m_AbsoluteRect.width, m_ScoreBox.m_AbsoluteRect.height), m_OrangePlayerInfo.m_Points.ToString (), m_Style);
		SetGUIDirection(BoardSide.Right);
		m_Style.normal.textColor = m_BlueColor;
		GUI.Label (new Rect(m_RotatedWidthStartAlign + m_ScoreBox.m_AbsoluteRect.x, m_ScoreBox.m_AbsoluteRect.y - m_ScoreBox.m_AbsoluteRect.height * 0.5f, m_ScoreBox.m_AbsoluteRect.width, m_ScoreBox.m_AbsoluteRect.height), m_BluePlayerInfo.m_Points.ToString (), m_Style);
		m_Style.normal.textColor = Color.white;
	}
	
	void DrawGameOverPoints()
	{
		SetGUIDirection(BoardSide.Left);
		m_Style.normal.textColor = m_OrangeColor;
		GUI.Label (new Rect(m_RotatedWidthStartAlign + m_EndTexBox.m_AbsoluteRect.x +  m_EndScoreBox.m_AbsoluteRect.x,m_EndTexBox.m_AbsoluteRect.y +  m_EndScoreBox.m_AbsoluteRect.y - m_EndScoreBox.m_AbsoluteRect.height * 0.5f , m_EndScoreBox.m_AbsoluteRect.width, m_EndScoreBox.m_AbsoluteRect.height), m_OrangePlayerInfo.m_Points.ToString (), m_Style);
		SetGUIDirection(BoardSide.Right);
		m_Style.normal.textColor = m_BlueColor;
		GUI.Label (new Rect(m_RotatedWidthStartAlign + m_EndTexBox.m_AbsoluteRect.x + m_EndScoreBox.m_AbsoluteRect.x,m_EndTexBox.m_AbsoluteRect.y +  m_EndScoreBox.m_AbsoluteRect.y - m_EndScoreBox.m_AbsoluteRect.height * 0.5f, m_EndScoreBox.m_AbsoluteRect.width, m_EndScoreBox.m_AbsoluteRect.height), m_BluePlayerInfo.m_Points.ToString (), m_Style);
		m_Style.normal.textColor = Color.white;
	}
	
	void DebugGUI()
	{
 		//DrawPoints ();
		//GameOverGUI();	
		//GUI.Label (new Rect(m_RotatedWidthStartAlign, Screen.height * 0.5f, 40, 40), "Bada", m_Style);
		//SetGUIDirection(BoardSide.Right);
		//GUI.Label (new Rect(m_RotatedWidthStartAlign, Screen.height * 0.5f, 40, 40), "Bede", m_Style);	
		
	}
	
	public void ProcessPoint(Character a_Character, Vector3 a_Position)
	{
		if (m_State != GameState.InGame) return;
		PlayerInfo t_PlayerInfo = GetPlayerInfo(a_Character.GetColorPlayer().m_ColorType);
		t_PlayerInfo.m_Points ++;
		if (t_PlayerInfo.m_Points % 5 == 0)
		{
			m_BlurbSpawner.SpawnBlurb (a_Position, true);
		}
	}
	public void ProcessBulletHit(Character a_Character, Vector3 a_Position)
	{
		if (m_State != GameState.InGame) return;
		PlayerInfo t_PlayerInfo = GetPlayerInfo(a_Character.GetColorPlayer().m_ColorType);
		if (t_PlayerInfo.IsInvulnerable) return;
		t_PlayerInfo.m_Lives -= m_BulletDamage;
		t_PlayerInfo.Hit ();
		m_BlurbSpawner.SpawnBlurb (a_Position, false);
	}
	public void ProcessPlayerCloneHit(Character a_Character, Vector3 a_Position)
	{
		if (m_State != GameState.InGame) return;
		PlayerInfo t_PlayerInfo = GetPlayerInfo(a_Character.GetColorPlayer().m_ColorType);
		if (t_PlayerInfo.IsInvulnerable) return;
		t_PlayerInfo.m_Lives -= m_PlayerCloneDamage;
		t_PlayerInfo.Hit ();
		m_BlurbSpawner.SpawnBlurb (a_Position, false);
	}
	
	public bool CanDoGameInput()
	{
		return (m_State == GameState.InGame || m_State == GameState.PreGame);
	}
	
	public GameState GetGameState()
	{
		return m_State;
	}
	
	public PlayerInfo GetPlayerInfo(ColorType a_ColType)
	{
		switch (a_ColType)
		{
			case ColorType.Blue:
				return m_BluePlayerInfo;
			break;
			case ColorType.Orange:
				return m_OrangePlayerInfo;
			break;
		}
		return null;
	}
	
	public BulletWall GetBulletWall(Direction a_Direction)
	{
		switch (a_Direction)
		{
			case Direction.East:
				return m_EastBulletWall;
			break;
			case Direction.West:
				return m_WestBulletWall;
			break;
		}
		return null;
	}
	
	public void GoGameOver()
	{
		m_State = GameState.GameOver;
		ColorBullet[] t_ColorBullets = GameObject.FindObjectsOfType<ColorBullet>();
		for (int i = 0; i < t_ColorBullets.Length; i ++)
		{
			Destroy (t_ColorBullets[i].gameObject);	
		}
	}
}
