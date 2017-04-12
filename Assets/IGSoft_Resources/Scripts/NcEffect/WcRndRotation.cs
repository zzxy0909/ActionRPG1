// Attribute ------------------------------------------------------------------------
// Property -------------------------------------------------------------------------
// Loop Function --------------------------------------------------------------------
// Control Function -----------------------------------------------------------------
// Event Function -------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class WcRndRotation : NcEffectBehaviour
{
	// Attribute ------------------------------------------------------------------------
    public enum AXIS { X = 0, Y, Z };
    public AXIS m_RatationAxis = AXIS.Z;

    //! Check properties.
    public bool m_IsRndStart = false;
    public bool m_IsRndRotation = false;
    public bool m_IsRndRotMinus = false;
    public bool m_IsRndStartPos = false;
    public bool m_IsRndScale = false;//Random scale

    public bool m_IsRndStartPosMinusX = false;
    public bool m_IsRndStartPosMinusY = false;
    public bool m_IsRndStartPosMinusZ = false;
    
    //! Random roation speed.
    public float m_RndRotSpeedMin = 1;
    public float m_RndRotSpeedMax = 2;

    //! Random Scale
    public Vector3 m_RndScaleMin = new Vector3(0,0,0);
    public Vector3 m_RndScaleMax = new Vector3(1, 1, 1);

    //! Random Start Position.
    public Vector3 m_RndStartPosMin;
    public Vector3 m_RndStartPosMax;
    //!
    private float m_RndRotSpeed = 0;
    private float m_RndRotDir = 1;
   
    //!
    private bool m_InitRnd = false;

    protected float m_fRndValue;
    private float m_fRndRotDir = 1.0f;
    protected float m_fTotalRotationValue;
    protected Quaternion m_qOiginal;


	// Property -------------------------------------------------------------------------
#if UNITY_EDITOR
	public override string CheckProperty()
	{
	//	if (GetComponent<NcBillboard>() != null)
	//		return "SCRIPT_CLASH_ROTATEBILL";
		return "";	// no error
	}
#endif

	// --------------------------------------------------------------------------
	void Update()
	{
        if (m_InitRnd == false)
        {
            if (m_IsRndStartPos)
            {
                float l_X = Random.Range(m_RndStartPosMin.x, m_RndStartPosMax.x);
                float l_Y = Random.Range(m_RndStartPosMin.y, m_RndStartPosMax.y);
                float l_Z = Random.Range(m_RndStartPosMin.z, m_RndStartPosMax.z);

                if (m_IsRndStartPosMinusX)
                    l_X *= Random.Range(1, 10) < 5 ? -1f : 1f;

                if (m_IsRndStartPosMinusY)
                    l_Y *= Random.Range(1, 10) < 5 ? -1f : 1f;

                if (m_IsRndStartPosMinusZ)
                    l_Z *= Random.Range(1, 10) < 5 ? -1f : 1f;

                transform.position += new Vector3(l_X, l_Y, l_Z);
            }

            if (m_IsRndScale)
            {
                float l_X = Random.Range(m_RndScaleMin.x, m_RndScaleMin.x);
                float l_Y = Random.Range(m_RndScaleMin.y, m_RndScaleMin.y);
                float l_Z = Random.Range(m_RndScaleMin.z, m_RndScaleMin.z);

                transform.localScale = new Vector3(l_X, l_Y, l_Z);
            }
            m_fRndValue = Random.Range(0, 360.0f);//< Random Start Rotation var

            m_RndRotSpeed = Random.Range(m_RndRotSpeedMin, m_RndRotSpeedMax);//< Random speed

            //! Random direction for random rotation.
            if (m_IsRndRotMinus == true)
            {
                if (Random.Range(1, 10) < 5)
                   m_RndRotDir = -1.0f;
                else
                    m_RndRotDir = 1.0f;
            }

            m_InitRnd = true;

            if (m_IsRndStart)
                transform.localRotation *= Quaternion.Euler((m_RatationAxis == AXIS.X ? m_fRndValue : 0), (m_RatationAxis == AXIS.Y ? m_fRndValue : 0), (m_RatationAxis == AXIS.Z ? m_fRndValue : 0));

        }
        if (m_IsRndRotation)
        {
            float fRotValue = m_fTotalRotationValue + GetEngineDeltaTime() * m_RndRotSpeed * m_RndRotDir;
            transform.Rotate((m_RatationAxis == AXIS.X ? fRotValue : 0), (m_RatationAxis == AXIS.Y ? fRotValue : 0), (m_RatationAxis == AXIS.Z ? fRotValue : 0), Space.Self);
            m_fTotalRotationValue = fRotValue;
        }
	}

	// Event Function -------------------------------------------------------------------
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
         m_fRndValue = Random.Range(0, 360.0f);
 	}
}
