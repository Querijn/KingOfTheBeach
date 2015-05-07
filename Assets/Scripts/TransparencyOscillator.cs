using UnityEngine;
using System.Collections;

public class TransparencyOscillator : MonoBehaviour {
	public float m_Speed;
	Renderer m_Renderer;
	// Use this for initialization
	void Start () {
		m_Renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		m_Renderer.material.color = new Color(m_Renderer.material.color.r, m_Renderer.material.color.g,
												m_Renderer.material.color.b, Mathf.Cos (Time.timeSinceLevelLoad * m_Speed)*0.3f + 0.5f);
	}
}
