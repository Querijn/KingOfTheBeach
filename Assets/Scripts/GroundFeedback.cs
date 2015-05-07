using UnityEngine;
using System.Collections;

public class GroundFeedback : MonoBehaviour 
{
	public Color m_Orange;
	public Color m_Blue;

	public enum FlashSide
	{
		FlashOnLeft,
		FlashOnRight,
		FlashOnBoth
	};

	public float m_FlashTime = 500.0f;
	private float m_LFlashTimer;
	private float m_RFlashTimer;

	void Start () 
	{
		//FlashColour(FlashSide.FlashOnLeft, m_Blue);
		//FlashColour(FlashSide.FlashOnRight, m_Orange);
	}

	void Update () 
	{
		m_RFlashTimer -= Time.deltaTime*1000.0f;
		m_LFlashTimer -= Time.deltaTime*1000.0f;

		if(m_RFlashTimer<0)
			this.renderer.material.SetFloat("g_RightSize", 0.0f);
		else this.renderer.material.SetFloat("g_RightSize", 1-m_RFlashTimer/m_FlashTime);

		if(m_LFlashTimer<0) this.renderer.material.SetFloat("g_LeftSize", 0.0f);
		else this.renderer.material.SetFloat("g_LeftSize", 1-m_LFlashTimer/m_FlashTime);
	}

	public void FlashColour(FlashSide a_Side, Color a_Color)
	{
		//print ("Flashing");
		if(a_Side==FlashSide.FlashOnLeft)
		{
			m_LFlashTimer = m_FlashTime;
		}
		else if(a_Side==FlashSide.FlashOnRight)
		{
			m_RFlashTimer = m_FlashTime;
		}
		else
		{
			m_LFlashTimer = m_FlashTime;
			m_RFlashTimer = m_FlashTime;
		}
		renderer.material.SetColor("g_LeftColor", m_Orange);
		renderer.material.SetColor("g_RightColor", m_Blue);
	}
	
	public Color GetBlue()
	{
		return m_Blue;
	}
	
	public Color GetOrange()
	{
		return m_Orange;
	}
}
