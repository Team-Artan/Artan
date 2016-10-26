using UnityEngine;
using System.Collections;

[System.Serializable]
public class toothedWheel
{
	public GameObject wheel;
	public float      radius;
	public float      circumference
	{
		get { return 2.0f * Mathf.PI * radius; }
	}
	public int		  toothCount;
}

[System.Serializable]
public class smoothWheel
{
	public GameObject wheel;
	public float      radius;
	public float      circumference
	{
		get { return 2.0f * Mathf.PI * radius; }
	}
}

public enum trackDirectionAdjust
{
	positiveDir,
	negativeDir
}

[System.Serializable]
public class TrackController //: MonoBehaviour 
{
	public trackDirectionAdjust trackDir;
	public trackDirectionAdjust wheelDir;
	public GameObject track;
	public int        trackToothCount;
	public int		  trackTextureWrapCount;
	public float	  trackThickness;

	//public float      trackLength;
	public toothedWheel driveWheel;
	public smoothWheel[] nonDriveWheels;
	public Color debugColour = Color.green;
	public Color debugColour2 = Color.red;
	public float debugCylinderWidth = 1.0f;
	
	float trackDirAdjust = 1.0f;
	float wheelDirAdjust = 1.0f;
	Renderer trackRenderer;
	bool trackActive = false;

	Quaternion driveWheelInitialRotation;
	float driveWheelRotationAngle;
	Quaternion[] nonDriveWheelInitialRotations;
	float[] nonDriveWheelCurrentRotationAngles;

	Vector3 prevTrackPos = Vector3.zero;
	Vector3 currTrackPos = Vector3.zero;

	// Use this for initialization
	public void Init() 
	{


		if ( trackDir == trackDirectionAdjust.negativeDir )
		{
			trackDirAdjust = -1.0f;
		}

		if ( wheelDir == trackDirectionAdjust.negativeDir )
		{
			wheelDirAdjust = -1.0f;
		}

		if ( track != null && track.GetComponent<Renderer>() != null )
		{
			//Debug.Log("Found the track renderer");
			trackRenderer = track.GetComponent<Renderer>();
			if ( trackRenderer.material != null )
			{
				//Debug.Log("Found the track material");
				trackActive = true;
			}

			prevTrackPos = track.transform.position;
			currTrackPos = prevTrackPos;
		}

		if ( driveWheel.wheel != null )
		{
			driveWheelInitialRotation = driveWheel.wheel.transform.localRotation;
			driveWheelRotationAngle = 0.0f;
		}

		if ( nonDriveWheels != null && nonDriveWheels.Length > 0 )
		{
			nonDriveWheelInitialRotations = new Quaternion[nonDriveWheels.Length];
			nonDriveWheelCurrentRotationAngles = new float[nonDriveWheels.Length];

			for (int i = 0 ; i < nonDriveWheels.Length ; i++)
			{
				if ( nonDriveWheels[i].wheel != null )
				{
					nonDriveWheelInitialRotations[i] = nonDriveWheels[i].wheel.transform.localRotation;
				}
				nonDriveWheelCurrentRotationAngles[i] = 0.0f;
			}
		}
	}
	
	// Update is called once per frame
	public void Update(float speed, bool freeWheel) 
	{

		if ( !trackActive )
		{
			return;
		}

		prevTrackPos = currTrackPos;
		currTrackPos = track.transform.position;

		Vector3 moveDelta = currTrackPos - prevTrackPos;
		Vector3 localDelta = track.transform.InverseTransformDirection(moveDelta);

		//Debug.Log("localDelta = " + localDelta.ToString());

		float trackDelta = 0;

		if ( freeWheel == false )
		{
			trackDelta = speed*Time.deltaTime;
		}
		else
		{
			trackDelta = localDelta.z;
		}

		if ( trackDir == trackDirectionAdjust.negativeDir )
		{
			trackDirAdjust = -1.0f;
		}
		else
		{
			trackDirAdjust = 1.0f;
		}
		
		if ( wheelDir == trackDirectionAdjust.negativeDir )
		{
			wheelDirAdjust = -1.0f;
		}
		else
		{
			wheelDirAdjust = 1.0f;
		}

		if ( driveWheel.circumference <= 0.0f )
		{
			// Must not cause a divide by zero etc.
			return;
		}

		// Determine the cogwheel rotation
		driveWheelRotationAngle += wheelDirAdjust*360.0f*trackDelta/((driveWheel.radius+trackThickness)*2.0f*Mathf.PI);

		if ( driveWheelRotationAngle > (360.0f * (float)(driveWheel.toothCount*trackToothCount) ) )
		{
			driveWheelRotationAngle -= (360.0f * (float)(driveWheel.toothCount*trackToothCount) );
		}
		else if ( driveWheelRotationAngle < -(360.0f * (float)(driveWheel.toothCount*trackToothCount) ) )
		{
			driveWheelRotationAngle += (360.0f * (float)(driveWheel.toothCount*trackToothCount) );
		}

		//Debug.Log("driveWheelRotationAngle = " + driveWheelRotationAngle);
		driveWheel.wheel.transform.localRotation = driveWheelInitialRotation;
		driveWheel.wheel.transform.Rotate(Vector3.right, driveWheelRotationAngle, Space.Self);

		// now drive the track offset
		float trackOffset = ((float)driveWheel.toothCount*driveWheelRotationAngle*(float)trackTextureWrapCount)/(360.0f*(float)trackToothCount);

		Vector2 newOffset = new Vector2(0.0f, trackOffset*trackDirAdjust);
		track.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", newOffset);
		track.GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", newOffset);



		// Set the non drive wheel rotations
		if ( nonDriveWheels != null && nonDriveWheels.Length > 0 && trackTextureWrapCount > 0)
		{
			for (int i = 0 ; i < nonDriveWheels.Length ; i++ )
			{
				if ( nonDriveWheels[i].wheel != null )
				{
					nonDriveWheelCurrentRotationAngles[i] += wheelDirAdjust*360.0f*trackDelta/nonDriveWheels[i].circumference;
					if ( nonDriveWheelCurrentRotationAngles[i] > 360.0f )
					{
						nonDriveWheelCurrentRotationAngles[i] -= 360.0f;
					}
					else if ( nonDriveWheelCurrentRotationAngles[i] < 0.0f )
					{
						nonDriveWheelCurrentRotationAngles[i] += 360.0f;
					}
					nonDriveWheels[i].wheel.transform.localRotation = nonDriveWheelInitialRotations[i];
					nonDriveWheels[i].wheel.transform.Rotate(Vector3.right, nonDriveWheelCurrentRotationAngles[i], Space.Self);
				}
			}
		}

	}



	public void OnDrawGizmos()
	{
		if ( driveWheel.wheel != null )
		{
			DrawWheelCylinder(driveWheel.wheel, driveWheel.radius, 20, debugCylinderWidth, debugColour );
			DrawWheelCylinder(driveWheel.wheel, driveWheel.radius+trackThickness, 20, debugCylinderWidth, debugColour2 );
		}

		if ( nonDriveWheels != null && nonDriveWheels.Length > 0 )
		{
			for ( int i = 0 ; i < nonDriveWheels.Length ; i++ )
			{
				if ( nonDriveWheels[i] != null && nonDriveWheels[i].wheel != null)
				{
					DrawWheelCylinder(nonDriveWheels[i].wheel, nonDriveWheels[i].radius, 20, debugCylinderWidth, debugColour);
				}
			}
		}
	}

	void DrawWheelCylinder(GameObject wheel, float radius, int segmentCount, float width, Color objectCol)
	{
		if ( radius <= 0.0f || segmentCount <= 0 )
		{
			return;
		}

		Gizmos.color = objectCol;

		float rotationIncrement = 2.0f*Mathf.PI/(float)segmentCount;

		for (int i = 0 ; i < segmentCount ; i++ )
		{
			float startAngle = (float) i * rotationIncrement;
			float endAngle = (float) (i+1)*rotationIncrement;
			Vector3 startPointLocalOP = new Vector3(0.0f, Mathf.Cos(startAngle)*radius, Mathf.Sin(startAngle)*radius);
			Vector3 endPointLocalOP   = new Vector3(0.0f, Mathf.Cos(endAngle)*radius, Mathf.Sin(endAngle)*radius);
			Vector3 startPointLocalEP = new Vector3(-width, Mathf.Cos(startAngle)*radius, Mathf.Sin(startAngle)*radius);;
			Vector3 endPointLocalEP = new Vector3(-width, Mathf.Cos(endAngle)*radius, Mathf.Sin(endAngle)*radius);

			Gizmos.DrawLine(wheel.transform.TransformPoint(startPointLocalOP), wheel.transform.TransformPoint(endPointLocalOP));
			Gizmos.DrawLine(wheel.transform.TransformPoint(startPointLocalEP), wheel.transform.TransformPoint(endPointLocalEP));
			Gizmos.DrawLine(wheel.transform.TransformPoint(endPointLocalOP), wheel.transform.TransformPoint(endPointLocalEP));
		}

	}
}
