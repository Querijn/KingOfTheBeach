using UnityEngine;
using System.Collections;

public class ColorClone : ColorCollider {
	private GameManager m_GameManager;
	private Character m_Character;
	public ParticleSystem m_DamageParticles;
	public GameObject m_Shield;
	protected override void Start_Imp()
	{
		m_GameManager = GameObject.FindObjectOfType<GameManager>();
		m_Character = transform.GetComponent<Clone>().m_Parent.GetComponent<Character>();
	}
	
	protected override void Update_Imp()
	{
		m_Shield.SetActive (m_GameManager.GetPlayerInfo(m_ColorType).IsInvulnerable);
	}
	
	protected override void CollideWithSameColor(ColorCollider a_Other)
	{
		switch (a_Other.m_ObjectType)
		{
			case ObjectType.Bullet:
				a_Other.GetBroken();
				if (m_GameManager != null)
				{
				m_GameManager.ProcessPoint(m_Character, transform.position);
				m_Character.PlaySuckSound();
				}
			break;
		}
	}
	
	protected override void CollideWithOtherColor(ColorCollider a_Other)
	{
		switch (a_Other.m_ObjectType)
		{
			case ObjectType.Bullet:
				if (m_GameManager != null)
				{
					if (m_GameManager.GetPlayerInfo(m_ColorType).IsInvulnerable)
					{
						m_Character.PlayInvulnBulletSound();
					}
					else
					{
						m_GameManager.ProcessBulletHit(m_Character, transform.position);
						m_Character.PlayHitSound();
						m_DamageParticles.Play ();
					}				
				}
				a_Other.GetBroken();
			break;
		}
	}
}
