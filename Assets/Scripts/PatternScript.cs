using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PatternScript : MonoBehaviour 
{
	public GameObject m_BlueBullet, m_OrangeBullet;
	public GameObject m_Cannon;
	public Vector3 m_CannonOffset;
	public float m_CannonAngle;
	public Texture2D[] m_PatternTextures; 
	public List<Pattern> m_Patterns = new List<Pattern>();
	
	ParticleSystem[] m_CannonOrangeParticlesLeft;
	ParticleSystem[] m_CannonOrangeParticlesRight;
	
	ParticleSystem[] m_CannonBlueParticlesLeft;
	ParticleSystem[] m_CannonBlueParticlesRight;

	public int m_SwapSidePatternCount = 3;
	private int m_PatternsPassed = 0;
	private float m_Direction = 0;

	public int[] m_IntroWaves;
	private int m_CurrentWave = -1;

	public enum ForcedDir
	{
		Left,
		Right,
		Both
	};

	public bool m_ForceDirection = false;
	public ForcedDir m_ForcedDirection = ForcedDir.Right;

	int m_CurrentPattern = -1;
	int m_Frame = 0;
	public float m_FrameInterval = 50.0f;
	float m_FrameTimer;

	[Range(10.0f, 2500.0f)] public float m_Speed = 50.0f;
	public float m_DifficultySpeedIncrement = 5.0f;
	[Range(1, 150)] public int m_IncreaseEveryAmountOfWaves = 1;
	
	private GameManager m_GameManager;
	
	public AudioSource m_LeftSound;
	public AudioSource m_RightSound;
	public AudioSource m_BothSound;
	public AudioSource m_CannonSound;
	public AudioSource m_Cannon2Sound;

	private GroundFeedback m_Floor;
	
	void Start() 
	{
		m_Floor = GameObject.FindGameObjectWithTag("GroundPlane").GetComponent<GroundFeedback>();
		float t_YHeight = 20.0f;
		bool t_IsThereATexture = false;
		m_GameManager = GameObject.FindObjectOfType<GameManager>();
		foreach(Texture2D t_Texture in m_PatternTextures)
		{
			t_IsThereATexture = true;
			Pattern t_Pattern = new Pattern();
			//print ("Adding pattern " +t_Texture);
			float t_LaneYMultiplier = t_YHeight / t_Texture.height;

			for(int x = 0; x<t_Texture.width; x++)
			{
				////print ("Frame "+x);
				Frame t_Frame = new Frame();
				int t_Count = 0;

				for(int y = 0; y<t_Texture.height; y++)
				{
					Color t_Color = t_Texture.GetPixel(x, y);
					if(t_Color.a!=0)
					{
						////print ("Number "+(t_Count+1));
						////print ("y = '"+y+"'\nLane = '"+(y*t_LaneYMultiplier)+"'\n");
						////print ("Type = "+((t_Color.r<0.5)?(0):(1)).ToString()+"'\nSpeed = '"+(t_Color.g*255.0f)+"'");
						t_Frame.m_Y.Add(y*t_LaneYMultiplier-(t_YHeight*0.5f));
						t_Frame.m_Index.Add (y);
						t_Frame.m_Direction.Add(-1);

						t_Frame.m_Type.Add((t_Color.r<0.5f)?(0):(1));
						////print (t_Color.g*255.0f);
						t_Frame.m_Speed.Add(m_Speed);
						t_Count++;
					}
				}

				//if(t_Count>0)
				{
					t_Pattern.m_Frames.Add(t_Frame);
					////print ("Added this frame ("+x+")");
				}
			}

			/*
			for(int x = t_Texture.width-1; x>=t_Texture.width/2; x--)
			{
				////print ("Frame "+x);
				//Frame t_Frame = new Frame();
				int t_FrameToAddTo = t_Texture.width-1-x;
				int t_Count = 0;
				
				for(int y = 0; y<t_Texture.height; y++)
				{
					Color t_Color = t_Texture.GetPixel(x, y);
					if(t_Color.a!=0)
					{
						////print ("Number "+(t_Count+1));
						////print ("y = '"+y+"'\nLane = '"+(y*t_LaneYMultiplier)+"'\n");
						////print ("Type = "+((t_Color.r<0.5)?(0):(1)).ToString()+"'\nSpeed = '"+(t_Color.g*255.0f)+"'");
						////print(t_FrameToAddTo);
						t_Pattern.m_Frames[t_FrameToAddTo].m_Y.Add(y*t_LaneYMultiplier-10.0f);

						t_Pattern.m_Frames[t_FrameToAddTo].m_Direction.Add(1);
						
						t_Pattern.m_Frames[t_FrameToAddTo].m_Type.Add((t_Color.r<0.5f)?(0):(1));
						////print (t_Color.g*255.0f);
						t_Pattern.m_Frames[t_FrameToAddTo].m_Speed.Add(m_Speed);
						t_Count++;
					}
				}

			}*/

			m_Patterns.Add (t_Pattern);
		}
		if (t_IsThereATexture)
		{
			int t_Count = m_PatternTextures[0].height;
			float t_Interval = t_YHeight / ((float)t_Count);
			float t_Y = - t_YHeight * 0.5f;
			m_CannonOrangeParticlesLeft = new ParticleSystem[t_Count];
			m_CannonOrangeParticlesRight = new ParticleSystem[t_Count];
			m_CannonBlueParticlesLeft = new ParticleSystem[t_Count];
			m_CannonBlueParticlesRight = new ParticleSystem[t_Count];
			for (int i = 0; i < t_Count; i ++)
			{
				GameObject t_G = GameObject.Instantiate (m_Cannon, new Vector3(-m_CannonOffset.x, m_CannonOffset.y, t_Y), Quaternion.Euler (0,0,m_CannonAngle)) as GameObject;
				m_CannonOrangeParticlesLeft[i] = t_G.GetComponentInChildren<CannonParticles>().m_OrangeParticles;
				m_CannonBlueParticlesLeft[i] = t_G.GetComponentInChildren<CannonParticles>().m_BlueParticles;
				t_G = GameObject.Instantiate (m_Cannon, new Vector3( m_CannonOffset.x, m_CannonOffset.y, t_Y), Quaternion.Euler (0,180,m_CannonAngle)) as GameObject;
				m_CannonOrangeParticlesRight[i] = t_G.GetComponentInChildren<CannonParticles>().m_OrangeParticles;
				m_CannonBlueParticlesRight[i] = t_G.GetComponentInChildren<CannonParticles>().m_BlueParticles;
				t_Y += t_Interval;
			}
		}
	}

	void Update () 
	{
		if (m_GameManager.GetGameState() != GameState.InGame)
		{
			return;
		}
		if(m_Patterns.Count==0)
			return;

		////print (m_CurrentPattern);

		if(m_CurrentPattern<0)
		{
			if(m_CurrentWave<m_IntroWaves.Length-1) 
			{
				m_CurrentWave++;
				//print ("Playing " + m_CurrentWave);
				m_CurrentPattern = m_IntroWaves[m_CurrentWave];
			}
			else
			{
				m_CurrentPattern = Random.Range(0,m_Patterns.Count);
			}

			
			if(m_PatternsPassed==0) m_Floor.FlashColour(GroundFeedback.FlashSide.FlashOnLeft, m_Floor.GetBlue());

			//print ("Spawning pattern "+m_PatternTextures[m_CurrentPattern]);
			m_Frame = 0;
			m_FrameTimer = m_FrameInterval;
		}

		m_FrameTimer -= Time.deltaTime*1000.0f;

		if(m_FrameTimer<0.0f)
		{
			m_FrameTimer = m_FrameInterval;
			m_Frame++;
			////print ("Spawn enemies.");

			if(m_Frame>m_Patterns[m_CurrentPattern].m_Frames.Count-1)
			{
				m_CurrentPattern = -1;

				//return yield WaitForSeconds(3);
				return;
			}			

			////print ("Frame: "+m_Frame+" Total: "+(m_Patterns[m_CurrentPattern].m_Frames.Count-1));
			while(m_Patterns[m_CurrentPattern].m_Frames[m_Frame].m_Speed.Count==0)
			{
				m_Frame++;
				if(m_Frame>m_Patterns[m_CurrentPattern].m_Frames.Count-1)
				{
					m_CurrentPattern = -1;

					////print (m_PatternsPassed%m_SwapSidePatternCount);
					
					m_PatternsPassed++;

					if(m_PatternsPassed%m_SwapSidePatternCount==0)
					{
						int t_Dice = Random.Range(0,100);
						int t_OldDirection = (int)m_Direction;
						////print ("Direction "+m_Direction+" Roll "+t_Dice);
						if(m_Direction==-1) // left
						{
							if(t_Dice<10){}
							else if(t_Dice<60)
							{
								m_Direction = 1;
							}
							else
							{
								m_Direction = 0;
							}
						}
						else if(m_Direction==1) // right
						{
							if(t_Dice<10){}
							else if(t_Dice<60)
							{
								m_Direction = -1;
							}
							else
							{
								m_Direction = 0;
							}
						}
						else
						{
							if(t_Dice<40)
							{
								m_Direction = -1;
							}
							else if(t_Dice<80)
							{
								m_Direction = 1;
							}
						}

						if(t_OldDirection!=m_Direction)
						{
							int t_Dir = (int)m_Direction;
							switch(t_Dir)
							{
							case 1:
								m_Floor.FlashColour(GroundFeedback.FlashSide.FlashOnLeft, m_Floor.GetBlue());
								m_RightSound.PlayOneShot(m_RightSound.clip);
								break;
							case -1:
								m_Floor.FlashColour(GroundFeedback.FlashSide.FlashOnRight, m_Floor.GetBlue());
								m_LeftSound.PlayOneShot(m_LeftSound.clip);
								break;
							case 0:
								m_Floor.FlashColour(GroundFeedback.FlashSide.FlashOnBoth, m_Floor.GetBlue());
								m_BothSound.PlayOneShot(m_BothSound.clip);
								break;
							}
						}

						if(m_ForceDirection)
						{
							switch(m_ForcedDirection)
							{
							case ForcedDir.Both:
								m_Direction = 0;
								break;
							case ForcedDir.Left:
								m_Direction = -1;
								break;
							case ForcedDir.Right:
								m_Direction = 1;
								break;
							}
						}
						//print ("New direction "+m_Direction);

					}

					if(m_PatternsPassed%m_IncreaseEveryAmountOfWaves==0)
					{
						m_Speed += Mathf.Abs(m_DifficultySpeedIncrement);
					}
					return;
				}
			}

			{
				Frame t_Frame = m_Patterns[m_CurrentPattern].m_Frames[m_Frame];

				////print ("Creating "+t_Frame.m_Speed.Count+" enemies.");
				/// 
				/// 

				int t_Roll = Random.Range (0,10);
				//print (t_Roll);
				if(t_Roll<5)
					m_CannonSound.PlayOneShot(m_CannonSound.clip);
				else m_Cannon2Sound.PlayOneShot(m_Cannon2Sound.clip);

				for(int i = 0; i<t_Frame.m_Speed.Count; i++)
				{
					GameObject t_Object;
					/*if(t_Frame.m_Type[i]==0)
						t_Object = (GameObject)Instantiate(m_BlueBullet, new Vector3(t_Frame.m_Direction[i], 0, t_Frame.m_Y[i]), Quaternion.identity);
					else t_Object = (GameObject)Instantiate(m_OrangeBullet, new Vector3(t_Frame.m_Direction[i], 4, t_Frame.m_Y[i]), Quaternion.identity); */


					if(m_Direction!=0)
					{
					
						if(t_Frame.m_Type[i]==0)
						{
							if (m_Direction == 1)
								m_CannonBlueParticlesRight[t_Frame.m_Index[i]].Play ();
							else
								m_CannonBlueParticlesLeft[t_Frame.m_Index[i]].Play ();
							t_Object = (GameObject)Instantiate(m_BlueBullet, transform.position + new Vector3(m_Direction * 0.5f, 0, t_Frame.m_Y[i]), Quaternion.identity);							
						}
						else
						{
							if (m_Direction == 1)
								m_CannonOrangeParticlesRight[t_Frame.m_Index[i]].Play ();
							else
								m_CannonOrangeParticlesLeft[t_Frame.m_Index[i]].Play ();
							t_Object = (GameObject)Instantiate(m_OrangeBullet, transform.position + new Vector3(m_Direction * 0.5f, 0, t_Frame.m_Y[i]), Quaternion.identity); 
						}
					}
					else
					{
						if(t_Frame.m_Type[i]==0)
						{
							m_CannonBlueParticlesRight[t_Frame.m_Index[i]].Play ();
							t_Object = (GameObject)Instantiate(m_BlueBullet, transform.position + new Vector3(1 * 0.5f, 0, t_Frame.m_Y[i]), Quaternion.identity);
						}
						else
						{
							m_CannonOrangeParticlesLeft[t_Frame.m_Index[i]].Play ();
							t_Object = (GameObject)Instantiate(m_OrangeBullet, transform.position + new Vector3(-1 * 0.5f, 0, t_Frame.m_Y[i]), Quaternion.identity); 
						}
					}

					t_Object.GetComponent<ColorBullet>().SetSpeed(m_Speed);
					//m_ActiveBullets.Add(t_Object);
				}
			}
		}
	}
	
	public BoardSide GetDirection()
	{
		switch ((int)m_Direction)
		{
			case -1:	
				return BoardSide.Left;
			break;
			case 0:	
				return BoardSide.None;
			break;
			case 1:
				return BoardSide.Right;
			break;
		}
		return BoardSide.None;
	}
}
