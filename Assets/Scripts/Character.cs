using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class Character : MonoBehaviour 
{
	public GameObject m_Clone = null;
	private GameObject m_ActualClone = null;
	[Range(1,2)] public int m_Player = 1;

	public bool m_PlayerTeleports = false;
	[Range(10,2000)] public float m_Speed = 50.0f;

	private bool m_Moving = false;
	private bool m_TouchIsDown = false;
	private int m_TouchFinger = 0;
	private Vector3 m_Velocity = new Vector3(0,0,0);
	private Vector3 m_Destination = new Vector3(0,0,0);
	private Vector3 m_MeshUpVector = Vector3.up;
	private Vector3 m_MeshForwardVector = Vector3.forward;
	public Vector3 m_UpVectorSwayer;
	public float m_UpVectorRotateSpeed;
	private InputWrapper m_InputWrapper = null;
	private GameManager m_GameManager;
	private ColorPlayer m_ColorPlayer;
	
	public GameObject m_Mesh;
	public HealthCircle m_HealthCircle;
	public LayerMask m_GroundLayer;

	public AudioSource m_SuckSource;
	public AudioSource m_HitSource;
	
	public AudioClip m_PlayerHitClip;
	public AudioClip m_InvulnBulletClip;
	

	void Start () 
	{
		if(m_ActualClone == null)
		{
			m_ActualClone = (GameObject)Instantiate(m_Clone, this.transform.position, Quaternion.identity);
			m_ActualClone.GetComponent<Clone>().SetParent(this.gameObject);
		}
		m_GameManager = GameObject.FindObjectOfType<GameManager>();
		m_InputWrapper = GameObject.FindObjectOfType<InputWrapper>();
		m_ColorPlayer = GetComponent<ColorPlayer>();
		m_UpVectorSwayer.Normalize ();
	}
	
	void OnApplicationQuit()
	{
		//print ("Quitting");
		Destroy (m_ActualClone);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_GameManager != null)
		{
			if (m_HealthCircle != null)
			{
				float t_Health = 1.0f - (m_GameManager.GetPlayerInfo(m_ColorPlayer.m_ColorType).m_Lives / m_GameManager.m_StartLives);
				m_HealthCircle.m_CutoffValue = t_Health;
			}
			if (!m_GameManager.CanDoGameInput())
			{
				return;
			}			
		}
		if (m_InputWrapper != null)
		{

			GameTouch t_Touch = null;
			if (m_Player == 1)
			{
				t_Touch = m_InputWrapper.GetGameTouch(BoardSide.Left);
				//t_Touch = m_InputWrapper.GetGameIDTouch(0);
			}
			else
			{
				t_Touch = m_InputWrapper.GetGameTouch(BoardSide.Right);
				//t_Touch = m_InputWrapper.GetGameIDTouch(1);
			}
			if (t_Touch != null)
			{
				if (m_TouchFinger != t_Touch.m_FingerID)
				{
					m_TouchIsDown = false;
					m_TouchFinger = t_Touch.m_FingerID;
				}
				Ray t_Ray = Camera.main.ScreenPointToRay(t_Touch.m_Touch.position);
				RaycastHit t_RayHit;
				if (Physics.Raycast (t_Ray, out t_RayHit, 1000, m_GroundLayer)) 
				{
					Vector3 t_Position = t_RayHit.point;

					MoveTo(t_Position);
				}	
			}
			else
			{
				m_TouchIsDown = false;	
			}
		}
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			if(Input.GetMouseButton(0))
			{
				Ray t_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);	
				RaycastHit t_RayHit;
				if (Physics.Raycast (t_Ray, out t_RayHit, 1000, m_GroundLayer)) 
				{
					Vector3 t_Position = t_RayHit.point;
		
				if((t_Position.x<0 && m_Player==1) ||
				   (t_Position.x>0 && m_Player==2))
						MoveTo(t_Position);		
				}
			}
		}
		
		m_HealthCircle.m_IsHolding = m_Moving;
		m_ActualClone.GetComponent<Clone>().m_HealthCircle.m_IsHolding = m_Moving;

		if (!m_PlayerTeleports && m_Moving) MovementUpdate();
		m_UpVectorSwayer = Quaternion.AngleAxis (m_UpVectorRotateSpeed, Vector3.up) * m_UpVectorSwayer;
		m_MeshUpVector = Vector3.Lerp(m_MeshUpVector, m_UpVectorSwayer, 0.1f);
		m_Mesh.transform.rotation = Quaternion.Lerp (m_Mesh.transform.rotation, Quaternion.LookRotation(m_MeshForwardVector, m_MeshUpVector), 0.25f);
	}

	void MoveTo(Vector3 a_Position)
	{
		if (m_PlayerTeleports) this.transform.position = a_Position;
		else 
		{
			m_Destination = a_Position;
			if (!m_TouchIsDown)
			{				
				if((a_Position-transform.position).sqrMagnitude>2.2f*2.2f)
					return;	
				m_TouchIsDown = true;
				//m_HealthCircle.PlayEffect();
				//m_ActualClone.GetComponent<Clone>().m_HealthCircle.PlayEffect();
			}
			m_Moving = true;
		}
	}

	void MovementUpdate()
	{		
		m_Velocity = (m_Destination-transform.position).normalized*m_Speed*Time.deltaTime;
		
		if (m_Velocity.sqrMagnitude > m_Speed * m_Speed) 
		{
			m_Velocity.Normalize();
			m_Velocity *= m_Speed;
			
		}
		if (m_Velocity.sqrMagnitude > (m_Destination-transform.position).sqrMagnitude)
		{
			m_Velocity = (m_Destination-transform.position);
		}
		transform.position += m_Velocity;
		
		if (m_Velocity.magnitude > 0.15f)
		{
			if (m_Mesh != null)
			{
				//m_Mesh.transform.rotation = Quaternion.Lerp (m_Mesh.transform.rotation, Quaternion.LookRotation(m_Velocity, Vector3.up), 0.2f);
				m_MeshForwardVector = m_Velocity.normalized;
			}
			
			//transform.LookAt(transform.position + m_Velocity);		
		}
		
		if ((m_Destination - transform.position).sqrMagnitude < 1.0f)
		{
			m_Moving = false;

			transform.position = m_Destination;
		}
	}
	public bool IsControlled()
	{
		return m_TouchIsDown;	
	}
	public ColorPlayer GetColorPlayer()
	{
		return m_ColorPlayer;	
	}
	
	public void PlaySuckSound()
	{
		m_SuckSource.PlayOneShot(m_SuckSource.clip);
	}
	
	public void PlayHitSound()
	{
		m_HitSource.PlayOneShot(m_HitSource.clip);
	}
	
	public void PlayPlayerHitSound()
	{
		m_HitSource.PlayOneShot(m_PlayerHitClip);
	}
	public void PlayInvulnBulletSound()
	{
		m_HitSource.PlayOneShot(m_InvulnBulletClip);
	}
}
