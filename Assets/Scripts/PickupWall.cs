using UnityEngine;
using System.Collections;

public class PickupWall : Pickup {
	public float m_WallValue;
	public override void PickupAction(ColorType a_ColorType)
	{
		switch (a_ColorType)
		{
			case ColorType.Orange:
				m_GameManager.GetBulletWall (Direction.West).ReduceTargetPosition(m_WallValue);	
			break;
			case ColorType.Blue:
				m_GameManager.GetBulletWall (Direction.East).ReduceTargetPosition(m_WallValue);
			break;
		}
		
		
		
		Destroy(this.gameObject);
	}
}
