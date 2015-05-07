using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	protected GameManager m_GameManager;
	protected BoardSide m_BoardSide;
	protected ColorType m_ColorType;
	public float m_RotationRate;
	public void Spawn(GameManager a_GameManager, BoardSide a_BoardSide, ColorType a_ColorType)
	{
		m_BoardSide = a_BoardSide;
		m_GameManager = a_GameManager;
		m_ColorType = a_ColorType;
	}
	
	public virtual void PickupAction(ColorType a_ColorType)
	{
		
	}
	
	void Update()
	{
		transform.Rotate(0,m_RotationRate * Time.deltaTime, 0);	
	}
}
