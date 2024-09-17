using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;

/// <summary>
/// This attribute can only be applied to fields because its
/// associated PropertyDrawer only operates on fields (either
/// public or tagged with the [SerializeField] attribute) in
/// the target MonoBehaviour.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field)]
public class InspectorButtonAttribute : PropertyAttribute
{
    public static float kDefaultButtonWidth = 80;

    public readonly string MethodName;

    private float _buttonWidth = kDefaultButtonWidth;
    public float ButtonWidth
    {
        get { return _buttonWidth; }
        set { _buttonWidth = value; }
    }

    public InspectorButtonAttribute(string MethodName)
    {
        this.MethodName = MethodName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
public class InspectorButtonPropertyDrawer : PropertyDrawer
{
    private MethodInfo _eventMethodInfo = null;

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        InspectorButtonAttribute inspectorButtonAttribute = (InspectorButtonAttribute)attribute;
        Rect buttonRect = new Rect(position.x + (position.width - inspectorButtonAttribute.ButtonWidth) * 0.5f, position.y, inspectorButtonAttribute.ButtonWidth, position.height);
        if (GUI.Button(buttonRect, label.text))
        {
            System.Type eventOwnerType = prop.serializedObject.targetObject.GetType();
            string eventName = inspectorButtonAttribute.MethodName;

            if (_eventMethodInfo == null)
                _eventMethodInfo = eventOwnerType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            if (_eventMethodInfo != null)
                _eventMethodInfo.Invoke(prop.serializedObject.targetObject, null);
            else
                Debug.LogWarning(string.Format("InspectorButton: Unable to find method {0} in {1}", eventName, eventOwnerType));
        }
    }
}
#endif

#if UNITY_EDITOR
[ExecuteInEditMode]
public class Proj : MonoBehaviour
{
    public bool DrawDebug = false;

    public Vector3Int SizeOfLevel;

    public bool nobgr = true;
    public LayerMask TargetLayer;
    public int backgr;
    public int targetLayer;

    [InspectorButton("GENERATE")]
    public bool Generate = false;

    [InspectorButton("CLEAR")]
    public bool Clear = false;

    public GameObject empt;
    public GameObject ResultF;
    public GameObject ResultB;
    public GameObject ResultL;
    public GameObject ResultR;

    public Tile3DManager t3dm;

    private int CurID;
    private int IDForMover;

    private void GENERATE()
    {
        ClearAllTransforms();
        Gen();
    }

    private void CLEAR()
    {
        print(TargetLayer.value);
        ClearAllTransforms();
    }

    void Gen()
    {
        PlayerSynchronization ps = Object.FindObjectOfType<PlayerSynchronization>();
        if (!t3dm)
            t3dm = Object.FindObjectOfType<Tile3DManager>();

        Vector3 StartPoint;
        Vector3Int size = SizeOfLevel + Vector3Int.one;
        //For front projection
        StartPoint = transform.position + Vector3.back;
        for(int i = 0; i < size.x; i++)
        {
            for(int j = 0; j < size.y; j++)
            {
                Vector3 pos = GetBlock(StartPoint + new Vector3(i, j, 0), Vector3.forward);
                if (pos != new Vector3(126.323f, 23, 23))
                {
                    GameObject g = (GameObject)PrefabUtility.InstantiatePrefab(empt, ResultF.transform);
                    g.transform.position = pos;
                    g.GetComponent<BlockParams>().depth = g.transform.position.z + 0.5f;
                    g.GetComponent<BlockParams>().ID = CurID;
                    g.GetComponent<BlockParams>().IDForMover = IDForMover;
                    g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, 0);
                    g.name = i.ToString() + " " + j.ToString() + " " + g.GetComponent<BlockParams>().depth.ToString();
                    g.layer = targetLayer;
                    g.GetComponent<BlockParams>().ps = ps;
                    g.GetComponent<BlockParams>().Invoke("InitInMover", .1f);
                }
            }
        }

        //For back projection
        StartPoint = new Vector3(0, 0, size.z + 1) + transform.position;
        for (int i = size.x - 1; i >= 0; i--)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3 pos = GetBlock(StartPoint + new Vector3(i, j, 0), Vector3.back);
                if (pos != new Vector3(126.323f, 23, 23))
                {
                    GameObject g = (GameObject)PrefabUtility.InstantiatePrefab(empt, ResultB.transform);
                    g.transform.position = pos;
                    g.GetComponent<BlockParams>().depth = g.transform.position.z - 0.5f;
                    g.GetComponent<BlockParams>().ID = CurID;
                    g.GetComponent<BlockParams>().IDForMover = IDForMover;
                    g.transform.position = new Vector3(-g.transform.position.x, g.transform.position.y, 0);
                    g.name = i.ToString() + " " + j.ToString() + " " + g.GetComponent<BlockParams>().depth.ToString();
                    g.layer = targetLayer;
                    g.GetComponent<BlockParams>().ps = ps;
                    g.GetComponent<BlockParams>().Invoke("InitInMover", .1f);
                }
            }
        }
        
        //For left projection
        StartPoint = transform.position + Vector3.left;
        for (int i = size.z; i >= 0; i--)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3 pos = GetBlock(StartPoint + new Vector3(0, j, i), Vector3.right);
                if (pos != new Vector3(126.323f, 23, 23))
                {
                    GameObject g = (GameObject)PrefabUtility.InstantiatePrefab(empt, ResultL.transform);
                    g.transform.position = pos;
                    g.GetComponent<BlockParams>().depth = g.transform.position.x + 0.5f;
                    g.GetComponent<BlockParams>().ID = CurID;
                    g.GetComponent<BlockParams>().IDForMover = IDForMover;
                    g.transform.position = new Vector3(-g.transform.position.z, g.transform.position.y, 0);
                    g.name = i.ToString() + " " + j.ToString() + " " + g.GetComponent<BlockParams>().depth.ToString();
                    g.layer = targetLayer;
                    g.GetComponent<BlockParams>().ps = ps;
                    g.GetComponent<BlockParams>().Invoke("InitInMover", .1f);
                }
            }
        }

        //For right projection
        StartPoint = new Vector3(size.x, 0, 0) + new Vector3(transform.position.x, transform.position.y, transform.position.z) + Vector3.right;
        for (int i = size.z; i >= 0; i--)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3 pos = GetBlock(StartPoint + new Vector3(0, j, i), Vector3.left);
                if (pos != new Vector3(126.323f, 23, 23))
                {
                    GameObject g = (GameObject)PrefabUtility.InstantiatePrefab(empt, ResultR.transform);
                    g.transform.position = pos;
                    g.GetComponent<BlockParams>().depth = g.transform.position.x - 0.5f;
                    g.GetComponent<BlockParams>().ID = CurID;
                    g.GetComponent<BlockParams>().IDForMover = IDForMover;
                    g.transform.position = new Vector3(g.transform.position.z, g.transform.position.y, 0);
                    g.name = i.ToString() + " " + j.ToString() + " " + g.GetComponent<BlockParams>().depth.ToString();
                    g.layer = targetLayer;
                    g.GetComponent<BlockParams>().ps = ps;
                    g.GetComponent<BlockParams>().Invoke("InitInMover", .1f);
                }
            }
        }
    }

    Vector3 GetBlock(Vector3 start, Vector3 dirrection)
    {
        Ray r = new Ray(start, dirrection);
        if (Physics.Raycast(r, out RaycastHit hit, (SizeOfLevel + Vector3Int.one).magnitude, TargetLayer))
        {
            CurID = t3dm.CheckisPrefab(hit.collider.gameObject);
            if (CurID == -1)
            {
                print("start");
                print(hit.collider.gameObject.name);
                CurID = t3dm.CheckisPrefab(hit.collider.transform.parent.gameObject);
                print(hit.collider.transform.parent.gameObject.name + " Parent");
                print("end");

                if(CurID == -1)
                {
                    CurID = t3dm.CheckisPrefab(hit.collider.transform.parent.parent.gameObject);
                    IDForMover = hit.collider.transform.parent.parent.gameObject.GetComponent<Block3DParams>().IDForMover;
                }
                else
                    IDForMover = hit.collider.transform.parent.gameObject.GetComponent<Block3DParams>().IDForMover;
            }
            else
                IDForMover = hit.collider.gameObject.GetComponent<Block3DParams>().IDForMover;

            if (hit.collider.gameObject.layer != backgr)
            {

                return hit.point;
            }
            else if(!nobgr)
            {
                return hit.point;
            }
        }

        return new Vector3(126.323f, 23, 23);
    }

    void ClearAllTransforms()
    {
        GameObject[] chs = new GameObject[ResultF.transform.childCount];

        int i = 0;

        foreach(Transform t in ResultF.transform)
        {
            chs[i] = t.gameObject;
            i++;
        }

        foreach(GameObject g in chs)
        {
            DestroyImmediate(g);
        }


        chs = new GameObject[ResultB.transform.childCount];

        i = 0;

        foreach (Transform t in ResultB.transform)
        {
            chs[i] = t.gameObject;
            i++;
        }

        foreach (GameObject g in chs)
        {
            DestroyImmediate(g);
        }

        chs = new GameObject[ResultL.transform.childCount];

        i = 0;

        foreach (Transform t in ResultL.transform)
        {
            chs[i] = t.gameObject;
            i++;
        }

        foreach (GameObject g in chs)
        {
            DestroyImmediate(g);
        }

        chs = new GameObject[ResultR.transform.childCount];

        i = 0;

        foreach (Transform t in ResultR.transform)
        {
            chs[i] = t.gameObject;
            i++;
        }

        foreach (GameObject g in chs)
        {
            DestroyImmediate(g);
        }
    }

    private void OnDrawGizmos()
    {
        if(DrawDebug)
            Gizmos.DrawWireCube(transform.position + SizeOfLevel / 2 + new Vector3(0.5f, 0, 0.5f), SizeOfLevel);
    }
}
#endif