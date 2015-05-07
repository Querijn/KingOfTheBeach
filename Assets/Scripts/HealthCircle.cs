using UnityEngine;
using System.Collections;

public class HealthCircle : MonoBehaviour {
	public float m_CutoffValue;
	public float m_MinCutoffValue;
	public float m_MaxCutoffValue;
	public bool m_LockRotation;
	public bool m_IsClone;
	public Vector3 m_LockSetting;
	Quaternion m_LockQuat;


	bool m_PlayingEffect = false;
	public bool m_IsHolding = false;
	float m_EffectTimer = 0.0f;

	// Use this for initialization
	void Start () {
		m_LockQuat = Quaternion.Euler(m_LockSetting);
	}

	public void PlayEffect()
	{
		m_PlayingEffect = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float t_Cutoff = Mathf.Clamp (m_CutoffValue, m_MinCutoffValue, m_MaxCutoffValue);
		renderer.material.SetFloat("_Cutoff",  t_Cutoff);
		renderer.material.SetColor("_Color",  Color.white);
		if (m_LockRotation)
		{
			transform.rotation = m_LockQuat;
		}
		
		m_EffectTimer += Time.deltaTime;
		if(m_PlayingEffect)
		{
			renderer.material.SetFloat("_Float", m_EffectTimer-Mathf.Floor(m_EffectTimer));

			if(m_EffectTimer>3.0f)
			{
				m_PlayingEffect = false;
				renderer.material.SetFloat("_Float", 0.0f);
			}
		}

		if(m_IsHolding)
			renderer.material.SetFloat("_Float2", 1.5f
			                           //2.0f-(m_EffectTimer-Mathf.Floor(m_EffectTimer))
			                           );
		else if (m_IsClone)
		{
			renderer.material.SetFloat("_Float2", 0.5f);	
		}	
			else
		{
			renderer.material.SetFloat("_Float2", 1.0f);
		}
	}
}
