using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class HiddenFinder : MonoBehaviour
{
    [InspectorButton("FIND")]
    public bool Find = false;

    public GameObject F;
    public GameObject B;
    public GameObject L;
    public GameObject R;

    public GameObject tarF;
    public GameObject tarB;
    public GameObject tarL;
    public GameObject tarR;

    public Transform HiddenStart;
    public Transform HiddenEnd;

    private void FIND()
    {
        ClearAllTransforms();

        foreach(Transform t in F.transform)
        {
            if(t.GetComponent<BlockParams>().depth >= HiddenStart.position.z && t.GetComponent<BlockParams>().depth <= HiddenEnd.position.z &&
                t.position.x >= HiddenStart.position.x && t.position.x <= HiddenEnd.position.x)
            {
                GameObject gR = Instantiate(t.gameObject, tarR.transform);
                gR.transform.position = new Vector3(t.GetComponent<BlockParams>().depth, t.position.y);
                gR.GetComponent<BlockParams>().depth = t.position.x;
                GameObject gL = Instantiate(t.gameObject, tarL.transform);
                gL.transform.position = new Vector3(-t.GetComponent<BlockParams>().depth, t.position.y);
                gL.GetComponent<BlockParams>().depth = t.position.x;
            }
        }

        foreach (Transform t in B.transform)
        {
            if (t.GetComponent<BlockParams>().depth >= HiddenStart.position.z && t.GetComponent<BlockParams>().depth <= HiddenEnd.position.z &&
                -t.position.x >= HiddenStart.position.x && -t.position.x <= HiddenEnd.position.x)
            {
                GameObject gR = Instantiate(t.gameObject, tarR.transform);
                gR.transform.position = new Vector3(t.GetComponent<BlockParams>().depth, t.position.y);
                gR.GetComponent<BlockParams>().depth = -t.position.x;
                GameObject gL = Instantiate(t.gameObject, tarL.transform);
                gL.transform.position = new Vector3(-t.GetComponent<BlockParams>().depth, t.position.y);
                gL.GetComponent<BlockParams>().depth = -t.position.x;
            }
        }

        foreach (Transform t in R.transform)
        {
            if (t.GetComponent<BlockParams>().depth >= HiddenStart.position.x && t.GetComponent<BlockParams>().depth <= HiddenEnd.position.x &&
                t.position.x >= HiddenStart.position.z && t.position.x <= HiddenEnd.position.z)
            {
                GameObject gF = Instantiate(t.gameObject, tarF.transform);
                gF.transform.position = new Vector3(t.GetComponent<BlockParams>().depth, t.position.y);
                gF.GetComponent<BlockParams>().depth = t.position.x;
                GameObject gB = Instantiate(t.gameObject, tarB.transform);
                gB.transform.position = new Vector3(-t.GetComponent<BlockParams>().depth, t.position.y);
                gB.GetComponent<BlockParams>().depth = t.position.x;
            }
            else
                print(t.name);
        }

        foreach (Transform t in L.transform)
        {
            if (t.GetComponent<BlockParams>().depth >= HiddenStart.position.x && t.GetComponent<BlockParams>().depth <= HiddenEnd.position.x &&
                -t.position.x >= HiddenStart.position.z && -t.position.x <= HiddenEnd.position.z)
            {
                GameObject gF = Instantiate(t.gameObject, tarF.transform);
                gF.transform.position = new Vector3(t.GetComponent<BlockParams>().depth, t.position.y);
                gF.GetComponent<BlockParams>().depth = -t.position.x;
                GameObject gB = Instantiate(t.gameObject, tarB.transform);
                gB.transform.position = new Vector3(-t.GetComponent<BlockParams>().depth, t.position.y);
                gB.GetComponent<BlockParams>().depth = -t.position.x;
            }
            else
                print(t.name);
        }
    }

    void ClearAllTransforms()
    {
        GameObject[] chs = new GameObject[tarF.transform.childCount];

        int i = 0;

        foreach (Transform t in tarF.transform)
        {
            chs[i] = t.gameObject;
            i++;
        }

        foreach (GameObject g in chs)
        {
            DestroyImmediate(g);
        }


        chs = new GameObject[tarB.transform.childCount];

        i = 0;

        foreach (Transform t in tarB.transform)
        {
            chs[i] = t.gameObject;
            i++;
        }

        foreach (GameObject g in chs)
        {
            DestroyImmediate(g);
        }

        chs = new GameObject[tarL.transform.childCount];

        i = 0;

        foreach (Transform t in tarL.transform)
        {
            chs[i] = t.gameObject;
            i++;
        }

        foreach (GameObject g in chs)
        {
            DestroyImmediate(g);
        }

        chs = new GameObject[tarR.transform.childCount];

        i = 0;

        foreach (Transform t in tarR.transform)
        {
            chs[i] = t.gameObject;
            i++;
        }

        foreach (GameObject g in chs)
        {
            DestroyImmediate(g);
        }
    }
}
#endif