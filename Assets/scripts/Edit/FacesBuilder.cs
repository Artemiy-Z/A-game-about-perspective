using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class FacesBuilder : MonoBehaviour
{
    public int BackGroundLayer;
    public int LevelLayer;

    public GameObject F;
    public GameObject B;
    public GameObject L;
    public GameObject R;

    public GameObject tarF;
    public GameObject tarB;
    public GameObject tarL;
    public GameObject tarR;

    public GameObject face;

    [InspectorButton("GENERATE")]
    public bool Generate = false;

    [InspectorButton("CLEAR")]
    public bool Clear = false;

    private void GENERATE()
    {
        PlayerSynchronization ps = Object.FindObjectOfType<PlayerSynchronization>();
        ClearAllTransforms();

        Tile3DManager tm = Object.FindObjectOfType<Tile3DManager>();

        print(tm);

        foreach (Transform t in F.transform)
        {
            if (t.GetComponent<BlockParams>().ID < 6)
            {
                GameObject f = Instantiate(face, tarF.transform);
                f.transform.position = new Vector3(Mathf.RoundToInt(t.position.x * 10) * 0.1f, Mathf.Round(t.position.y * 10) * 0.1f, t.GetComponent<BlockParams>().depth - 0.5f);
                f.GetComponent<SpriteRenderer>().sprite = tm.TilePrefabs[t.GetComponent<BlockParams>().ID].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite;
                f.GetComponent<SpriteRenderer>().color = tm.TilePrefabs[t.GetComponent<BlockParams>().ID].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color;
                f.transform.rotation = Quaternion.LookRotation(Vector3.back);
                if (tm.TilePrefabs[t.GetComponent<BlockParams>().ID].transform.GetChild(0).GetChild(0).gameObject.layer == GetComponent<Proj>().backgr)
                {
                    f.layer = BackGroundLayer;
                }
                else
                {
                    f.layer = LevelLayer;
                }
                f.GetComponent<BlockParams>().IDForMover = t.GetComponent<BlockParams>().IDForMover;
                f.GetComponent<BlockParams>().ps = ps;
                f.GetComponent<BlockParams>().Invoke("InitInMover", .1f);
            }
        }
        foreach (Transform t in B.transform)
        {
            if (t.GetComponent<BlockParams>().ID < 6)
            {
                GameObject f = Instantiate(face, tarB.transform);
                f.transform.position = new Vector3(Mathf.Round(-t.position.x * 10) * 0.1f, Mathf.Round(t.position.y * 10) * 0.1f, t.GetComponent<BlockParams>().depth + 0.5f);
                f.transform.rotation = Quaternion.LookRotation(Vector3.forward);
                f.GetComponent<SpriteRenderer>().sprite = tm.TilePrefabs[t.GetComponent<BlockParams>().ID].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite;
                f.GetComponent<SpriteRenderer>().color = tm.TilePrefabs[t.GetComponent<BlockParams>().ID].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color;
                if (tm.TilePrefabs[t.GetComponent<BlockParams>().ID].transform.GetChild(0).GetChild(0).gameObject.layer == GetComponent<Proj>().backgr)
                {
                    f.layer = BackGroundLayer;
                }
                else
                {
                    f.layer = LevelLayer;
                }
                f.GetComponent<BlockParams>().IDForMover = t.GetComponent<BlockParams>().IDForMover;
                f.GetComponent<BlockParams>().ps = ps;
                f.GetComponent<BlockParams>().Invoke("InitInMover", .1f);
            }
        }
        foreach (Transform t in L.transform)
        {
            if (t.GetComponent<BlockParams>().ID < 6)
            {
                GameObject f = Instantiate(face, tarL.transform);
                f.transform.position = new Vector3(t.GetComponent<BlockParams>().depth - 0.5f, Mathf.Round(t.position.y * 10) * 0.1f, Mathf.Round(-t.position.x * 10) * 0.1f);
                f.transform.rotation = Quaternion.LookRotation(Vector3.left);
                f.GetComponent<SpriteRenderer>().sprite = tm.TilePrefabs[t.GetComponent<BlockParams>().ID].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite;
                f.GetComponent<SpriteRenderer>().color = tm.TilePrefabs[t.GetComponent<BlockParams>().ID].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color;
                if (tm.TilePrefabs[t.GetComponent<BlockParams>().ID].transform.GetChild(0).GetChild(0).gameObject.layer == GetComponent<Proj>().backgr)
                {
                    f.layer = BackGroundLayer;
                }
                else
                {
                    f.layer = LevelLayer;
                }
                f.GetComponent<BlockParams>().IDForMover = t.GetComponent<BlockParams>().IDForMover;
                f.GetComponent<BlockParams>().ps = ps;
                f.GetComponent<BlockParams>().Invoke("InitInMover", .1f);
            }
        }
        foreach (Transform t in R.transform)
        {
            if (t.GetComponent<BlockParams>().ID < 6)
            {
                GameObject f = Instantiate(face, tarR.transform);
                f.transform.position = new Vector3(t.GetComponent<BlockParams>().depth + 0.5f, Mathf.Round(t.position.y * 10) * 0.1f, Mathf.Round(t.position.x * 10) * 0.1f);
                f.transform.rotation = Quaternion.LookRotation(Vector3.right);
                f.GetComponent<SpriteRenderer>().sprite = tm.TilePrefabs[t.GetComponent<BlockParams>().ID].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite;
                f.GetComponent<SpriteRenderer>().color = tm.TilePrefabs[t.GetComponent<BlockParams>().ID].transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color;
                print(tm.TilePrefabs[t.GetComponent<BlockParams>().ID].transform.GetChild(0).GetChild(0).gameObject.layer);
                print(t.GetComponent<BlockParams>().ID);
                if (tm.TilePrefabs[t.GetComponent<BlockParams>().ID].transform.GetChild(0).GetChild(0).gameObject.layer == GetComponent<Proj>().backgr)
                {
                    f.layer = BackGroundLayer;
                }
                else
                {
                    f.layer = LevelLayer;
                }
                f.GetComponent<BlockParams>().IDForMover = t.GetComponent<BlockParams>().IDForMover;
                f.GetComponent<BlockParams>().ps = ps;
                f.GetComponent<BlockParams>().Invoke("InitInMover", .1f);
            }
        }
    }

    private void CLEAR()
    {
        ClearAllTransforms();
    }

    void ClearAllTransforms()
    {
        print("Front has " + tarF.transform.childCount.ToString() + " childs");

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

        print("Back has " + tarB.transform.childCount.ToString() + " childs");

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

        print("Left has " + tarL.transform.childCount.ToString() + " childs");

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

        print("Right has " + tarR.transform.childCount.ToString() + " childs");

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

        print("Clear Done");
    }
}

#endif