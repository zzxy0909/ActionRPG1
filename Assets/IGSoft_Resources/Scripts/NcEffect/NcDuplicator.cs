// ----------------------------------------------------------------------------------
//
// FXMaker
// Created by ismoon - 2012 - ismoonto@gmail.com
//
// ----------------------------------------------------------------------------------


using UnityEngine;

using System.Collections;

public class NcDuplicator : NcEffectBehaviour
{
	// Attribute ------------------------------------------------------------------------
	public		float			m_fDuplicateTime		= 0.1f;
	public		int				m_nDuplicateCount		= 3;
	public		float			m_fDuplicateLifeTime	= 0;

	protected	int				m_nCreateCount			= 0;
	protected	float			m_fStartTime			= 0;
	protected	GameObject	m_ClonObject;
	protected	bool			m_bInvoke				= false;

    // NC_MODIFY BEGIN ~.~
    public float m_AddStartPosFromNormalMin = 0;
    public float m_AddStartPosFromNormalMax = 0;

    public Vector3 m_StartPos = Vector3.zero;
    public Vector3 m_AddStartPos = Vector3.zero;
    public Vector3 m_AccumStartRot = Vector3.zero;
    public Vector3 m_AddAccumStartRot = Vector3.zero;
    public Vector3 m_RandomRange = Vector3.zero;
    public Vector3 m_scale = Vector3.one;
    public Vector3 m_addScale = Vector3.one;

    public bool m_allAtOnce = false;
    public bool m_useSpawnRadius = false;
    public float m_spawnRadius = 1.0f;

    public bool m_useSpawnDirection = false;
    public float m_spawnDirectionAngle = 0f;
    public float m_spawnDirectionSpeed = 4f;
	protected Vector3 m_offsetAddStartPos = Vector3.zero; 
	protected Vector3 m_offsetAddStartScale = Vector3.one;

    // NC_MODIFY END

	//wo mj begin
	public enum SpwanPositionMode
	{
		Triangle = 0,
		Vertex,
	}
	public		MeshFilter			m_meshFilter		= null;
	public 		SpwanPositionMode 	m_spwanPositionMode = SpwanPositionMode.Vertex; 
	public 		bool 				m_random    		= false;
	//wo mj end

	// Property -------------------------------------------------------------------------
#if UNITY_EDITOR
	public override string CheckProperty()
	{
		if (1 < gameObject.GetComponents(GetType()).Length)
			return "SCRIPT_WARRING_DUPLICATE";

		// err check
		if (transform.parent != null && transform.parent.gameObject == FindRootEditorEffect())
			return "SCRIPT_ERROR_ROOT";
		return "";	// no error
	}
#endif

	public override int GetAnimationState()
	{
		if ((enabled && IsActive(gameObject)) && (m_nDuplicateCount == 0 || m_nDuplicateCount != 0 && m_nCreateCount < m_nDuplicateCount))
			return 1;
		return 0;
	}

	public GameObject GetCloneObject()
	{
		return m_ClonObject;
	}

	// Loop Function --------------------------------------------------------------------
	void Awake()
	{
		m_nCreateCount	= 0;
		m_fStartTime	= -m_fDuplicateTime;
		m_ClonObject	= null;
		m_bInvoke		= false;

		if (enabled == false)
			return;

// 		Debug.Log("Duration.Awake");
#if UNITY_EDITOR
		if (IsCreatingEditObject() == false)
#endif
			if (transform.parent != null && (enabled && IsActive(gameObject) && GetComponent<NcDontActive>() == null))
				InitCloneObject();
	}

	protected override void OnDestroy()
	{
// 		Debug.Log("OnDestroy");
		if (m_ClonObject != null)
			Destroy(m_ClonObject);
		base.OnDestroy();
	}

	void Start()
	{
 //		Debug.Log("Duration.Start");
		if (m_bInvoke)
		{
			m_fStartTime = GetEngineTime();

			//wo mj begin
			if(this.m_meshFilter != null)
			{
				this.CreateCloneObjectMeshPosition();
			}
			else
			{
				CreateCloneObject();
			}//wo mj end

			InvokeRepeating("CreateCloneObject", m_fDuplicateTime, m_fDuplicateTime);
		}
	}

	void Update()
	{
// 		Debug.Log("Duration.Update");
		if (m_bInvoke)
			return;
		// Duration
		if (m_nDuplicateCount == 0 || m_nCreateCount < m_nDuplicateCount)
		{
			if (m_fStartTime + m_fDuplicateTime <= GetEngineTime())
			{
				m_fStartTime = GetEngineTime();

				//wo mj begin
				if (m_useSpawnRadius && m_allAtOnce)
				{
					if(m_meshFilter != null)
					{
						for (int i = 0; i < m_nDuplicateCount; ++i)
						{
							if (m_nCreateCount >= m_nDuplicateCount)
								break;
							CreateCloneObjectMeshPosition();  
						}
					}
					else
					{
						for (int i = 0; i < m_nDuplicateCount; ++i)
						{
							
							if (m_nCreateCount >= m_nDuplicateCount)
								break;
							CreateCloneObject();  
						}
					}
					
				}
				else
				{
					if(m_meshFilter != null)
					{
						CreateCloneObjectMeshPosition();
					}
					else
					{
						CreateCloneObject();
					}
				}
				//wo mj end
			}
		}
	}
	// Control Function -----------------------------------------------------------------
	void InitCloneObject()
	{
// 		Debug.Log("Duration.InitCloneObject");

		if (m_ClonObject == null)
		{
			// clone ----------------
			 m_ClonObject = (GameObject)CreateGameObject(gameObject);

			// Cancel ActiveControl
			HideNcDelayActive(m_ClonObject);

			// Remove Dup
			NcDuplicator durCom = m_ClonObject.GetComponent<NcDuplicator>();
			if (durCom != null)
//				DestroyImmediate(durCom);
				Destroy(durCom);

			// Remove NcDelayActive
			NcDelayActive delCom = m_ClonObject.GetComponent<NcDelayActive>();
			if (delCom != null)
//				DestroyImmediate(delCom);
				Destroy(delCom);

			// this ----------------
			// remove OtherComponent
			Component[] coms = transform.GetComponents<Component>();
			for (int n = 0; n < coms.Length; n++)
				if ((coms[n] is Transform) == false && (coms[n] is NcDuplicator) == false)
					Destroy(coms[n]);

			// removeChild
//#if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
			RemoveAllChildObject(gameObject, false);
//#else
////			RemoveAllChildObject(gameObject, true);		OnTrigger error - DestroyImmediate
//			RemoveAllChildObject(gameObject, false);
//#endif
		} else return;
	}

    /// <summary>
    /// NC_MODIFY BEGIN ~.~
    /// </summary>
    void CreateCloneObject()
    {
        if (m_ClonObject == null)
            return;

        GameObject createObj;
        if (transform.parent == null)
            createObj = (GameObject)CreateGameObject(gameObject);
        else createObj = (GameObject)CreateGameObject(transform.parent.gameObject, m_ClonObject);

//#if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
        SetActiveRecursively(createObj, true);
//#endif

        // m_fDuplicateLifeTime
        if (0 < m_fDuplicateLifeTime)
        {
            NcAutoDestruct ncAd = createObj.GetComponent<NcAutoDestruct>();
            if (ncAd == null)
                ncAd = createObj.AddComponent<NcAutoDestruct>();
            ncAd.m_fLifeTime = m_fDuplicateLifeTime;
        }

        if (m_useSpawnRadius)
        {
            // Random pos
            Vector3 newPos = createObj.transform.position;

            // newPos = Vector3.zero;
            Quaternion l_tmpQuat = Quaternion.AngleAxis((float)m_nCreateCount / (float)m_nDuplicateCount * 360.0f, Vector3.up);

            Vector3 l_tmpVec = new Vector3(m_spawnRadius, 0, 0);
            l_tmpVec = l_tmpQuat * l_tmpVec;
            createObj.transform.position = l_tmpVec + newPos;// new Vector3(Random.Range(-m_RandomRange.x, m_RandomRange.x) + newPos.x, Random.Range(-m_RandomRange.y, m_RandomRange.y) + newPos.y, Random.Range(-m_RandomRange.z, m_RandomRange.z) + newPos.z);

            // AddStartPos  
            m_offsetAddStartPos += m_AddStartPos;
            createObj.transform.localPosition += m_StartPos + m_offsetAddStartPos;
        }
        else if (m_useSpawnDirection)
        {
            // // Random pos
            // Vector3 newPos = createObj.transform.position;

            //// newPos = Vector3.zero;
            // Quaternion l_tmpQuat = Quaternion.AngleAxis(m_spawnDirectionAngle, Vector3.up);

            // Vector3 l_tmpVec = new Vector3((float)m_nCreateCount * m_fDuplicateTime * m_spawnDirectionSpeed, 0, 0);
            // l_tmpVec = l_tmpQuat * l_tmpVec;
            // createObj.transform.position = l_tmpVec + newPos;// new Vector3(Random.Range(-m_RandomRange.x, m_RandomRange.x) + newPos.x, Random.Range(-m_RandomRange.y, m_RandomRange.y) + newPos.y, Random.Range(-m_RandomRange.z, m_RandomRange.z) + newPos.z);

            // // AddStartPos  
            // createObj.transform.position += m_AddStartPos;

            // Random pos
            Vector3 newPos = createObj.transform.position;

            // newPos = Vector3.zero;
            Quaternion l_tmpQuat;

            if (transform.parent != null)
            {
                Vector3 l_go = transform.parent.transform.rotation.eulerAngles;
                l_tmpQuat = Quaternion.AngleAxis(m_spawnDirectionAngle + l_go.y, Vector3.up);
            }
            else
            {
                l_tmpQuat = Quaternion.AngleAxis(m_spawnDirectionAngle, Vector3.up);
            }
            Vector3 l_tmpVec = new Vector3((float)m_nCreateCount * m_fDuplicateTime * m_spawnDirectionSpeed, 0, 0);
            l_tmpVec = l_tmpQuat * l_tmpVec;
            createObj.transform.position = l_tmpVec + newPos;// new Vector3(Random.Range(-m_RandomRange.x, m_RandomRange.x) + newPos.x, Random.Range(-m_RandomRange.y, m_RandomRange.y) + newPos.y, Random.Range(-m_RandomRange.z, m_RandomRange.z) + newPos.z);

            // AddStartPos  
            m_offsetAddStartPos += m_AddStartPos;
            createObj.transform.localPosition += m_StartPos + m_offsetAddStartPos;
        }
        else
        {
            // Random pos
            Vector3 newPos = createObj.transform.position;
            createObj.transform.position = new Vector3(Random.Range(-m_RandomRange.x, m_RandomRange.x) + newPos.x, Random.Range(-m_RandomRange.y, m_RandomRange.y) + newPos.y, Random.Range(-m_RandomRange.z, m_RandomRange.z) + newPos.z);

            // AddStartPos
            m_offsetAddStartPos += m_AddStartPos;
            createObj.transform.localPosition += m_StartPos + m_offsetAddStartPos;
        }
        // m_AccumStartRot
        createObj.transform.localRotation *= Quaternion.Euler
                                            (m_AddAccumStartRot.x * m_nCreateCount + m_AccumStartRot.x,
                                             m_AddAccumStartRot.y * m_nCreateCount + m_AccumStartRot.y,
                                             m_AddAccumStartRot.z * m_nCreateCount + m_AccumStartRot.z);

        //m_offsetAddStartScale  = m_offsetAddStartScale * m_addScale;
        m_offsetAddStartScale = Vector3.Scale(m_offsetAddStartScale, m_addScale);
        Vector3 scale = Vector3.Scale(m_scale, m_offsetAddStartScale);
        createObj.transform.localScale = Vector3.Scale(createObj.transform.localScale, scale);
        createObj.name += " " + m_nCreateCount;

        //// Random pos
        //Vector3 newPos = createObj.transform.position;
        //createObj.transform.position = new Vector3(Random.Range(-m_RandomRange.x, m_RandomRange.x) + newPos.x, Random.Range(-m_RandomRange.y, m_RandomRange.y) + newPos.y, Random.Range(-m_RandomRange.z, m_RandomRange.z) + newPos.z);

        //// AddStartPos
        //createObj.transform.position += m_AddStartPos;

        //// m_AccumStartRot
        //createObj.transform.localRotation *= Quaternion.Euler(m_AccumStartRot.x * m_nCreateCount, m_AccumStartRot.y * m_nCreateCount, m_AccumStartRot.z * m_nCreateCount);
        //createObj.name += " " + m_nCreateCount;

        m_nCreateCount++;
        if (m_bInvoke)
        {
            if (m_nDuplicateCount <= m_nCreateCount)
                CancelInvoke("CreateCloneObject");
        }
    }

//  // ORIGINAL SOURCE ~.~
//    void CreateCloneObject()
//    {
//        if (m_ClonObject == null)
//            return;

//        GameObject createObj;
//        if (transform.parent == null)
//            createObj = (GameObject)CreateGameObject(gameObject);
//        else createObj = (GameObject)CreateGameObject(transform.parent.gameObject, m_ClonObject);

//#if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
//        SetActiveRecursively(createObj, true);
//#endif

//        // m_fDuplicateLifeTime
//        if (0 < m_fDuplicateLifeTime)
//        {
//            NcAutoDestruct ncAd = createObj.GetComponent<NcAutoDestruct>();
//            if (ncAd == null)
//                ncAd = createObj.AddComponent<NcAutoDestruct>();
//            ncAd.m_fLifeTime = m_fDuplicateLifeTime;
//        }

//        // Random pos
//        Vector3 newPos = createObj.transform.position;
//        createObj.transform.position = new Vector3(Random.Range(-m_RandomRange.x, m_RandomRange.x) + newPos.x, Random.Range(-m_RandomRange.y, m_RandomRange.y) + newPos.y, Random.Range(-m_RandomRange.z, m_RandomRange.z) + newPos.z);

//        // AddStartPos
//        createObj.transform.position += m_AddStartPos;

//        // m_AccumStartRot
//        createObj.transform.localRotation *= Quaternion.Euler(m_AccumStartRot.x * m_nCreateCount, m_AccumStartRot.y * m_nCreateCount, m_AccumStartRot.z * m_nCreateCount);
//        createObj.name += " " + m_nCreateCount;

//        m_nCreateCount++;
//        if (m_bInvoke)
//        {
//            if (m_nDuplicateCount <= m_nCreateCount)
//                CancelInvoke("CreateCloneObject");
//        }
//    }

    /// NC_MODIFY END ~.~
    /// 
    // Event Function -------------------------------------------------------------------
    public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
    {
        m_fDuplicateTime /= fSpeedRate;
        m_fDuplicateLifeTime /= fSpeedRate;

        if (bRuntime && m_ClonObject != null)
            NsEffectManager.AdjustSpeedRuntime(m_ClonObject, fSpeedRate);
    }
//    void CreateCloneObject()
//    {
//        // NC_MODIFY BEGIN ~.~
//        /*
//        if (m_ClonObject == null)
//            return;

//        GameObject createObj;
//        if (transform.parent == null)
//             createObj = (GameObject)CreateGameObject(gameObject);
//        else createObj = (GameObject)CreateGameObject(transform.parent.gameObject, m_ClonObject);

//#if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
//        SetActiveRecursively(createObj, true);
//#endif

//        // m_fDuplicateLifeTime
//        if (0 < m_fDuplicateLifeTime)
//        {
//            NcAutoDestruct	ncAd = createObj.GetComponent<NcAutoDestruct>();
//            if (ncAd == null)
//                ncAd = createObj.AddComponent<NcAutoDestruct>();
//            ncAd.m_fLifeTime	= m_fDuplicateLifeTime;
//        }

//        // Random pos
//        Vector3	newPos = createObj.transform.position;
//        createObj.transform.position = new Vector3(Random.Range(-m_RandomRange.x, m_RandomRange.x)+newPos.x, Random.Range(-m_RandomRange.y, m_RandomRange.y)+newPos.y, Random.Range(-m_RandomRange.z, m_RandomRange.z)+newPos.z);

//        // AddStartPos
//        createObj.transform.position += m_AddStartPos;

//        // m_AccumStartRot
//        createObj.transform.localRotation	*= Quaternion.Euler(m_AccumStartRot.x*m_nCreateCount, m_AccumStartRot.y*m_nCreateCount, m_AccumStartRot.z*m_nCreateCount);
//        createObj.name += " " + m_nCreateCount;

//        m_nCreateCount++;
//        if (m_bInvoke)
//        {
//            if (m_nDuplicateCount <= m_nCreateCount)
//                CancelInvoke("CreateCloneObject");
//        }
//         * */
        

//        if (m_ClonObject == null)
//            return;

//        GameObject createObj;
//        if (transform.parent == null)
//            createObj = (GameObject)CreateGameObject(gameObject);
//        else createObj = (GameObject)CreateGameObject(transform.parent.gameObject, m_ClonObject);

//#if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
//        SetActiveRecursively(createObj, true);
//#endif

//        // m_fDuplicateLifeTime
//        if (0 < m_fDuplicateLifeTime)
//        {
//            NcAutoDestruct ncAd = createObj.GetComponent<NcAutoDestruct>();
//            if (ncAd == null)
//                ncAd = createObj.AddComponent<NcAutoDestruct>();
//            ncAd.m_fLifeTime = m_fDuplicateLifeTime;
//        }

//        if (m_useSpawnRadius)
//        {
//            // Random pos
//            Vector3 newPos = createObj.transform.position;

//           // newPos = Vector3.zero;
//            Quaternion l_tmpQuat = Quaternion.AngleAxis((float)m_nCreateCount / (float)m_nDuplicateCount * 360.0f, Vector3.up);

//            Vector3 l_tmpVec = new Vector3(m_spawnRadius, 0, 0);
//            l_tmpVec = l_tmpQuat * l_tmpVec;
//            createObj.transform.position = l_tmpVec + newPos;// new Vector3(Random.Range(-m_RandomRange.x, m_RandomRange.x) + newPos.x, Random.Range(-m_RandomRange.y, m_RandomRange.y) + newPos.y, Random.Range(-m_RandomRange.z, m_RandomRange.z) + newPos.z);

//            // AddStartPos  
//            m_offsetAddStartPos  += m_AddStartPos;
//            createObj.transform.localPosition += m_StartPos + m_offsetAddStartPos;
//        }
//        else if (m_useSpawnDirection)
//        {
//           // // Random pos
//           // Vector3 newPos = createObj.transform.position;

//           //// newPos = Vector3.zero;
//           // Quaternion l_tmpQuat = Quaternion.AngleAxis(m_spawnDirectionAngle, Vector3.up);

//           // Vector3 l_tmpVec = new Vector3((float)m_nCreateCount * m_fDuplicateTime * m_spawnDirectionSpeed, 0, 0);
//           // l_tmpVec = l_tmpQuat * l_tmpVec;
//           // createObj.transform.position = l_tmpVec + newPos;// new Vector3(Random.Range(-m_RandomRange.x, m_RandomRange.x) + newPos.x, Random.Range(-m_RandomRange.y, m_RandomRange.y) + newPos.y, Random.Range(-m_RandomRange.z, m_RandomRange.z) + newPos.z);

//           // // AddStartPos  
//           // createObj.transform.position += m_AddStartPos;

//            // Random pos
//            Vector3 newPos = createObj.transform.position;

//            // newPos = Vector3.zero;
//            Quaternion l_tmpQuat;

//            if (transform.parent != null)
//            {
//                Vector3 l_go = transform.parent.transform.rotation.eulerAngles;
//                l_tmpQuat = Quaternion.AngleAxis(m_spawnDirectionAngle + l_go.y, Vector3.up);
//            }
//            else
//            {
//                l_tmpQuat = Quaternion.AngleAxis(m_spawnDirectionAngle, Vector3.up);
//            }
//            Vector3 l_tmpVec = new Vector3((float)m_nCreateCount * m_fDuplicateTime * m_spawnDirectionSpeed, 0, 0);
//            l_tmpVec = l_tmpQuat * l_tmpVec;
//            createObj.transform.position = l_tmpVec + newPos;// new Vector3(Random.Range(-m_RandomRange.x, m_RandomRange.x) + newPos.x, Random.Range(-m_RandomRange.y, m_RandomRange.y) + newPos.y, Random.Range(-m_RandomRange.z, m_RandomRange.z) + newPos.z);

//            // AddStartPos  
//            m_offsetAddStartPos  += m_AddStartPos;
//            createObj.transform.localPosition += m_StartPos + m_offsetAddStartPos;
//        }
//        else
//        {
//            // Random pos
//            Vector3 newPos = createObj.transform.position;
//            createObj.transform.position = new Vector3(Random.Range(-m_RandomRange.x, m_RandomRange.x) + newPos.x, Random.Range(-m_RandomRange.y, m_RandomRange.y) + newPos.y, Random.Range(-m_RandomRange.z, m_RandomRange.z) + newPos.z);

//            // AddStartPos
//            m_offsetAddStartPos  += m_AddStartPos;
//            createObj.transform.localPosition += m_StartPos + m_offsetAddStartPos;
//        }
//        // m_AccumStartRot
//        createObj.transform.localRotation *= Quaternion.Euler
//                                            (m_AddAccumStartRot.x * m_nCreateCount + m_AccumStartRot.x,
//                                             m_AddAccumStartRot.y * m_nCreateCount + m_AccumStartRot.y, 
//                                             m_AddAccumStartRot.z * m_nCreateCount + m_AccumStartRot.z);

//        //m_offsetAddStartScale  = m_offsetAddStartScale * m_addScale;
//        m_offsetAddStartScale = Vector3.Scale(m_offsetAddStartScale, m_addScale);
//        Vector3 scale = Vector3.Scale(m_scale, m_offsetAddStartScale); 
//        createObj.transform.localScale = Vector3.Scale(createObj.transform.localScale, scale);
//        createObj.name += " " + m_nCreateCount;

//        m_nCreateCount++;
//        if (m_bInvoke)
//        {
//            if (m_nDuplicateCount <= m_nCreateCount)
//                CancelInvoke("CreateCloneObject");
//        }
//        // NC_MODIFY END
//    }

	//wo mj begin
	int m_currentDuplicatorVertexIndex = 0;
	void CreateCloneObjectMeshPosition() 
	{ 
		if (m_ClonObject == null)
			return;
		
		Vector3 duplicatorPos 	= Vector3.zero;
		Vector3 duplicatorNormal = Vector3.up;
		if(m_spwanPositionMode == SpwanPositionMode.Triangle)
		{
			if(m_meshFilter.mesh.triangles.Length > 0)
			{
				if(m_random)
				{
					m_currentDuplicatorVertexIndex = Random.Range(0, m_meshFilter.mesh.triangles.Length-2);
				}
				
				if(m_currentDuplicatorVertexIndex < m_meshFilter.mesh.triangles.Length-2)
				{ 
					int triIndex0 = m_meshFilter.mesh.triangles[m_currentDuplicatorVertexIndex];
					int triIndex1 = m_meshFilter.mesh.triangles[m_currentDuplicatorVertexIndex+1];
					int triIndex2 = m_meshFilter.mesh.triangles[m_currentDuplicatorVertexIndex+2];
					if(m_random)
					{
						float l_random1 = Random.Range(0,1);
						float l_random2 = Random.Range(0,1);
						float l_random3 = Random.Range(0,1);
						Vector3 l_vertices01 = m_meshFilter.mesh.vertices[triIndex1] - m_meshFilter.mesh.vertices[triIndex0];
						l_vertices01 *= l_random1;
						l_vertices01 += m_meshFilter.mesh.vertices[triIndex0];
						
						Vector3 l_vertices02 = m_meshFilter.mesh.vertices[triIndex2] - m_meshFilter.mesh.vertices[triIndex0];
						l_vertices02 *= l_random2;
						l_vertices02 += m_meshFilter.mesh.vertices[triIndex0];
						
						Vector3 l_vertices0102 = l_vertices01 - l_vertices02;
						l_vertices0102 *= l_random3;
						l_vertices0102 += l_vertices02;
						
						
						duplicatorPos = l_vertices0102;
						duplicatorPos = m_meshFilter.transform.TransformPoint(duplicatorPos);
					}
					else
					{
						duplicatorPos = m_meshFilter.mesh.vertices[triIndex0]+ m_meshFilter.mesh.vertices[triIndex1]+ m_meshFilter.mesh.vertices[triIndex2];
						duplicatorPos /=3.0f;
						duplicatorPos = m_meshFilter.transform.TransformPoint(duplicatorPos);
					} 
					
					if(m_meshFilter.mesh.normals.Length == m_meshFilter.mesh.vertices.Length)
					{ 
						duplicatorNormal = m_meshFilter.mesh.normals[triIndex0]; 
						duplicatorNormal += m_meshFilter.mesh.normals[triIndex1];  
						duplicatorNormal += m_meshFilter.mesh.normals[triIndex2];  
						duplicatorNormal.Normalize();
						duplicatorNormal = m_meshFilter.transform.TransformDirection(duplicatorNormal);
						float AddStartPosFromNormal = Random.Range(m_AddStartPosFromNormalMin, m_AddStartPosFromNormalMax);
						duplicatorPos += duplicatorNormal*AddStartPosFromNormal;
					}
					
				}
				if(!m_random)
				{
					m_currentDuplicatorVertexIndex += 3;
					if(m_currentDuplicatorVertexIndex >= m_meshFilter.mesh.triangles.Length)
					{
						m_currentDuplicatorVertexIndex = 0;
					}
				}
				
			}
		}
		else
		{
			if(m_meshFilter.mesh.vertices.Length > 0)
			{
				if(m_random)
				{
					m_currentDuplicatorVertexIndex = Random.Range(0, m_meshFilter.mesh.vertices.Length);
				}
				if(m_currentDuplicatorVertexIndex < m_meshFilter.mesh.vertices.Length)
				{ 
					if(m_meshFilter.mesh.normals.Length == m_meshFilter.mesh.vertices.Length)
					{ 
						duplicatorNormal = m_meshFilter.transform.TransformDirection(m_meshFilter.mesh.normals[m_currentDuplicatorVertexIndex]);  
					}
					
					duplicatorPos = m_meshFilter.transform.TransformPoint(m_meshFilter.mesh.vertices[m_currentDuplicatorVertexIndex]);  
					float AddStartPosFromNormal = Random.Range(m_AddStartPosFromNormalMin, m_AddStartPosFromNormalMax);
					duplicatorPos += duplicatorNormal*AddStartPosFromNormal;
				}
				if(!m_random)
				{
					m_currentDuplicatorVertexIndex++;
					if(m_currentDuplicatorVertexIndex >= m_meshFilter.mesh.vertices.Length)
					{
						m_currentDuplicatorVertexIndex = 0;
					}
				}
			}
		}
		
		
		GameObject createObj;
		if (transform.parent == null)
			createObj = (GameObject)CreateGameObject(gameObject);
		else createObj = (GameObject)CreateGameObject(transform.parent.gameObject, m_ClonObject);
		
//		#if (UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
		SetActiveRecursively(createObj, true);
//		#endif
		
		// m_fDuplicateLifeTime
		if (0 < m_fDuplicateLifeTime)
		{
			NcAutoDestruct ncAd = createObj.GetComponent<NcAutoDestruct>();
			if (ncAd == null)
				ncAd = createObj.AddComponent<NcAutoDestruct>();
			ncAd.m_fLifeTime = m_fDuplicateLifeTime;
		}
		
		// Random pos
		Vector3 newPos = createObj.transform.position;
		createObj.transform.position = duplicatorPos;//new Vector3(Random.Range(-m_RandomRange.x, m_RandomRange.x) + newPos.x, Random.Range(-m_RandomRange.y, m_RandomRange.y) + newPos.y, Random.Range(-m_RandomRange.z, m_RandomRange.z) + newPos.z);
		
		// AddStartPos
		m_offsetAddStartPos  += m_AddStartPos;
		createObj.transform.localPosition += m_StartPos + m_offsetAddStartPos;
		
		// m_AccumStartRot
		createObj.transform.localRotation *= Quaternion.Euler
			(m_AddAccumStartRot.x * m_nCreateCount + m_AccumStartRot.x,
			 m_AddAccumStartRot.y * m_nCreateCount + m_AccumStartRot.y, 
			 m_AddAccumStartRot.z * m_nCreateCount + m_AccumStartRot.z);
		
		//m_offsetAddStartScale  = m_offsetAddStartScale * m_addScale;
		m_offsetAddStartScale = Vector3.Scale(m_offsetAddStartScale, m_addScale);
		Vector3 scale = Vector3.Scale(m_scale, m_offsetAddStartScale); 
		createObj.transform.localScale = Vector3.Scale(createObj.transform.localScale, scale);
		createObj.name += " " + m_nCreateCount;
		
		m_nCreateCount++;
		if (m_bInvoke)
		{
			if (m_nDuplicateCount <= m_nCreateCount)
				CancelInvoke("CreateCloneObject");
		}
		 
	}
	//wo mj end
 
}


