using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class Tile3DManager : MonoBehaviour
{
    [InspectorButton("ADD")]
    public bool Add = false;

    [InspectorButton("ADDCOPY")]
    public bool AddCopy = false;

    [InspectorButton("DEL")]
    public bool Del = false;

    [InspectorButton("RELOAD")]
    public bool Reload = false;

    [InspectorButton("RELOADARRAY")]
    public bool ReloadArray = false;

    [InspectorButton("REPLACETILEBYTILE")]
    public bool ReplaceTileByTile = false;

    [InspectorButton("RELOADCHILDREN")]
    public bool ReloadChildren = false;

    public GameObject[] Layers;
    public GameObject EmptyTilemap;

    public GameObject[] TilePrefabs;

    public enum states { Tile1B, Tile1BG, Tile1L, Tile1LG, Tile2B, Tile2L, Tile2LR};

    public states Original = states.Tile1B;
    public states Replace;

    public GameObject Tm2;

    private void ADD()
    {
        GameObject[] newl = new GameObject[Layers.Length + 1];
        int i = 0;
        foreach(GameObject g in Layers)
        {
            newl[i] = g;
            i++;
        }
        newl[Layers.Length] = EmptyTilemap;

        Layers = newl;

        UpdateGr();
    }

    private void ADDCOPY()
    {
        GameObject[] newl = new GameObject[Layers.Length + 1];
        int i = 0;
        foreach (GameObject g in Layers)
        {
            newl[i] = g;
            i++;
        }
        newl[Layers.Length] = Layers[Layers.Length - 1];

        Layers = newl;

        UpdateGr();
    }

    private void DEL()
    {
        GameObject[] newl = new GameObject[Layers.Length - 1];
        int i = 0;
        foreach (GameObject g in Layers)
        {
            newl[i] = g;
            i++;
            if (i == Layers.Length - 1) 
                break;
        }
        Layers = newl;
        UpdateGr();
        ReplaceByPrefab();
    }

    private void RELOAD()
    {
        UpdateGr();
        ReplaceByPrefab();
    }

    private void RELOADCHILDREN()
    {
        foreach(GameObject l in Layers)
        {
            foreach(Transform t in l.transform)
            {
                t.position = new Vector3(t.position.x, l.transform.position.y, t.position.z);
            }
        }
    }

    private void UpdateGr()
    {
        GameObject[] chs = new GameObject[transform.childCount];

        int i = 0;

        foreach (Transform t in transform)
        {
            chs[i] = t.gameObject;
            i++;
        }

        float minheight = transform.GetChild(0).position.y;

        for (int j = 0; j<Layers.Length; j++)
        {
            Layers[j] = Instantiate(Layers[j], transform);
            Layers[j].name = j.ToString();
            Layers[j].transform.position = Vector3.up * (minheight + j);
        }

        foreach (GameObject g in chs)
        {
            DestroyImmediate(g);
        }
    }

    void RELOADARRAY()
    {
        Layers = new GameObject[transform.childCount];

        int i = 0;

        foreach (Transform t in transform)
        {
            t.gameObject.name = i.ToString();
            Layers[i] = t.gameObject;
            i++;
        }
    }

    void ReplaceByPrefab()
    {
        int i = 0;

        GameObject[] chs = new GameObject[Tm2.transform.childCount];

        foreach (Transform t in Tm2.transform)
        {
            chs[i] = t.gameObject;
            i++;
        }

        foreach (GameObject g in chs)
        {
            DestroyImmediate(g);
        }

        foreach (GameObject l in Layers)
        {
            GameObject[] lch = new GameObject[l.transform.childCount];

            int iterat = 0;

            foreach(Transform t in l.transform)
            {
                lch[iterat] = t.gameObject;

                iterat++;
            }

            foreach(GameObject c in lch)
            {
                int cp = CheckisPrefab(c);
                if(cp != -1)
                {
                    if (cp >= 6)
                    {
                        GameObject f = Instantiate(c, Tm2.transform);
                        f.transform.position = new Vector3(Mathf.Round(c.transform.position.x * 10)/10, Mathf.Round(c.transform.position.y * 10) / 10, Mathf.Round(c.transform.position.z * 10) / 10);
                        f.name = f.name.Replace(" (Clone)", "");
                        f.layer = 0;
                        foreach (Transform s in f.transform)
                            foreach (Transform t in s)
                                t.gameObject.layer = 0;
                    }
                    GameObject g = (GameObject)PrefabUtility.InstantiatePrefab(TilePrefabs[cp], l.transform);
                    g.transform.localPosition = c.transform.localPosition;
                    DestroyImmediate(c);
                }
            }
        }
    }

    void REPLACETILEBYTILE()
    {
        foreach (GameObject l in Layers)
        {
            GameObject[] lch = new GameObject[l.transform.childCount];

            int iterat = 0;

            foreach (Transform t in l.transform)
            {
                lch[iterat] = t.gameObject;

                iterat++;
            }

            foreach (GameObject c in lch)
            {
                int cp = CheckisPrefab(c);
                if (cp != -1 && cp == (int)Original)
                {
                    GameObject g = (GameObject)PrefabUtility.InstantiatePrefab(TilePrefabs[(int)Replace], l.transform);
                    g.transform.position = c.transform.localPosition;
                    DestroyImmediate(c);
                }
            }
        }
    }

    public int CheckisPrefab(GameObject g)
    {
        int i = 0;
        foreach (GameObject p in TilePrefabs)
        {
            if (g.name == p.name)
                return i;
            else if (g.name == "Tile1B1SB")
                return 6;
            else if (g.name == "Tile1B1SF")
                return 7;
            else if (g.name == "Tile1B1SL")
                return 8;
            else if (g.name == "Tile1B1SR")
                return 9;
            else if (g.name == "Tile2B1SB")
                return 10;
            else if (g.name == "Tile2B1SF")
                return 11;
            else if (g.name == "Tile2B1SL")
                return 12;
            else if (g.name == "Tile2B1SR")
                return 13;
            i++;
        }
        return -1;
    }
}
#endif