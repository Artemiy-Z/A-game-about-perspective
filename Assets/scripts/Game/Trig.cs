using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trig : MonoBehaviour
{
    private Transform p;
    private PlayerSynchronization psynch;
    public Vector2 size;
    public bool isOnOneSide = false;
    public float side;
    private Vector2 psize = new Vector2(0.4852092f, 0.7349902f);
    public bool DEBUGr = false;

    private void Awake()
    {
        p = GameObject.FindGameObjectWithTag("Player").transform;
        psynch = GameObject.FindGameObjectWithTag("Player2D").GetComponent<PlayerSynchronization>();
    }

    public bool Triger()
    {
        if ((isOnOneSide && psynch.tarrot != side) || psynch.changing)
        {
            if(DEBUGr)
                print("ABOBA");
            return false;
        }

        float lrot = psynch.lrot;

        if (lrot % 360 == 0)
        {
            return InBounds(p.position.x, p.position.y, transform.position.x);
        }
        if (lrot == 90 || lrot == -270)
        {
            return InBounds(p.position.z, p.position.y, transform.position.z);
        }
        if (lrot == 180 || lrot == -180)
        {
            return InBounds(p.position.x, p.position.y, transform.position.x);
        }
        if (lrot == -90 || lrot == 270)
        {
            return InBounds(p.position.z, p.position.y, transform.position.z);
        }

        return false;
    }

    bool InBounds(float x, float y, float z)
    {
        if(    x + psize.x * 0.5f >= z - 0.5f * size.x 
            && x - psize.x * 0.5f <= z + 0.5f * size.x 
            && y + psize.y * 0.5f >= transform.position.y - 0.5f * size.y 
            && y - psize.y * 0.5f <= transform.position.y + 0.5f * size.y)
        {
            return true;
        }

        return false;
    }
}
