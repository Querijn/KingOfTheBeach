using UnityEngine;
using System.Collections;

public class FoamOscillator : MonoBehaviour {
	public float m_Speed;
	public float m_Range;
	
	float m_StartZ;
	float m_RandStart;
	// Use this for initialization
	void Start () {
		m_StartZ = transform.localPosition.y;
		m_RandStart = Random.Range (0.0f, 15.0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector3(transform.localPosition.x, m_StartZ + Mathf.Cos (m_RandStart + Time.timeSinceLevelLoad * m_Speed) * m_Range, transform.localPosition.z);
	}
}
