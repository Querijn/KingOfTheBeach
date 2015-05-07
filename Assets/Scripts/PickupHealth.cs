using UnityEngine;
using System.Collections;

public class PickupHealth : Pickup {
	public float m_HealthValue;
	public override void PickupAction(ColorType a_ColorType)
	{
		PlayerInfo t_Info = m_GameManager.GetPlayerInfo(a_ColorType);
		t_Info.m_Lives += m_HealthValue;
		if (t_Info.m_Lives > m_GameManager.m_StartLives) t_Info.m_Lives = m_GameManager.m_StartLives;
		
		Destroy(this.gameObject);
	}
}
