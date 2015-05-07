using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum BoardSide
{
	Left,
	Right,
	None
}

public class GameTouch
{
	public BoardSide m_Side;
	public int m_FingerID;
	public Touch m_Touch;	
}

public class InputWrapper : MonoBehaviour {
	
	List<GameTouch> m_GameTouches;
	public GUIStyle m_Style;
	void Start()
	{
		m_GameTouches = new List<GameTouch>();
	}
	
	void Update()
	{
		UpdateTouches();		
	}
	
	void UpdateTouches()
	{
		
		for (int i = 0; i < m_GameTouches.Count; i ++)
		{
			bool t_Found = false;
			for (int j = 0; j < Input.touchCount; j ++)
			{
				if (Input.GetTouch(j).fingerId == m_GameTouches[i].m_FingerID)
				{
					t_Found = true;
					m_GameTouches[i].m_Touch = Input.GetTouch(j);
					m_GameTouches[i].m_Side = GetBoardSide(Input.GetTouch(j).position);
					break;
				}
			}
			if (!t_Found)
			{
				m_GameTouches.Remove (m_GameTouches[i]);
			}
		}
		for (int i = 0; i < Input.touchCount; i ++)
		{
			bool t_Found = false;
			for (int j = 0; j < m_GameTouches.Count; j ++)
			{
				if (Input.touches[i].fingerId == m_GameTouches[j].m_FingerID)
				{
					t_Found = true;
					break;
				}
			}
			if (!t_Found)
			{
				GameTouch t_GameTouch = new GameTouch();
				t_GameTouch.m_Touch = Input.GetTouch (i);
				t_GameTouch.m_FingerID = Input.GetTouch(i).fingerId;
				t_GameTouch.m_Side = GetBoardSide(Input.GetTouch (i).position);
				m_GameTouches.Add(t_GameTouch);	
			}
		}
	}
	
	void OnGUI()
	{
		//for (int i = 0; i < m_GameTouches.Count; i ++)
		//{
			//GUI.Box (new Rect(0, i * 120, Screen.width, Screen.height),
			//		m_GameTouches[i].m_FingerID.ToString (), m_Style);
			//GUI.Box (new Rect(0, i * 120 + 40, Screen.width, Screen.height),
			//		m_GameTouches[i].m_Touch.fingerId.ToString (), m_Style);
			//GUI.Box (new Rect(0, i * 120 + 80, Screen.width, Screen.height),
			//		m_GameTouches[i].m_Touch.position.ToString (), m_Style);
		//}
	}
	
	public GameTouch GetGameTouch(BoardSide a_Side)
	{
		for (int i = 0; i < m_GameTouches.Count; i ++)
		{
			if (m_GameTouches[i].m_Side == a_Side)
			{
				return m_GameTouches[i];
			}
		}
		return null;
	}
	
	public GameTouch GetGameIDTouch(int a_ID)
	{
		if (a_ID < m_GameTouches.Count)
		{
			return m_GameTouches[a_ID];
		}
		
		return null;
	}
	
	BoardSide GetBoardSide(Vector2 a_Input)
	{
		if (a_Input.x < Screen.width*0.5f)
		{
			return BoardSide.Left;
		}
		return BoardSide.Right;
	}
}
