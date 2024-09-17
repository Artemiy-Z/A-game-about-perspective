using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockParams : MonoBehaviour
{
    public float depth;
    public int ID;
    public int IDForMover = -1;
    public PlayerSynchronization ps;

    public void InitInMover()
    {
        if (IDForMover != -1)
        {
            foreach (BlockMover bm in Object.FindObjectsOfType<BlockMover>())
            {
                if (bm.MoverID == IDForMover)
                {
                    gameObject.AddComponent<MoverSelf>();
                    if (!GetComponent<Rigidbody2D>())
                        if (GetComponent<MeshCollider>())
                            gameObject.AddComponent<Rigidbody>();
                        else
                        {
                            gameObject.AddComponent<Rigidbody2D>();
                            GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Extrapolate;
                        }
                    MoverSelf ms = GetComponent<MoverSelf>();
                    ms.maxy = bm.maxy;
                    ms.miny = bm.miny;
                    ms.ps = ps;
                    ms.startspeed = bm.startspeed;
                    if (GetComponent<BoxCollider2D>())
                    {
                        GetComponent<PlatformEffector2D>().enabled = false;
                        GetComponent<BoxCollider2D>().isTrigger = true;
                        GetComponent<BoxCollider2D>().usedByEffector = false;
                        GetComponent<BoxCollider2D>().size = new Vector2(1, 1.1f);
                        GetComponent<BoxCollider2D>().offset = new Vector2(0, 0.05f);
                        gameObject.AddComponent<BoxCollider2D>();
                    }
                }
            }
        }
    }
}
