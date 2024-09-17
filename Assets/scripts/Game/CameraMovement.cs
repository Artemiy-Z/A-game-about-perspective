using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public bool SmoothMovement = true;
    private Transform p;
    public float distance;
    public Vector3 min;
    public Vector3 max;
    public Vector3 center;
    public Vector3 Offset;
    private Vector3 targetOffset;
    public float maxdist = 4;
    public Animation scr;

    private void Awake()
    {
        p = GameObject.FindGameObjectWithTag("Player").transform;

        Invoke("RemoveScreen", 0.5f);
    }

    public void MoveToPlayer()
    {
        Vector3 poffset = p.position;

        if (enabled)
        {
            if (poffset.x >= targetOffset.x + maxdist)
            {
                targetOffset = new Vector3(p.position.x - maxdist, 0, targetOffset.z);
            }
            else if (poffset.x <= targetOffset.x - maxdist)
            {
                targetOffset = new Vector3(p.position.x + maxdist, 0, targetOffset.z);
            }
            if (poffset.z >= Offset.z + maxdist)
            {
                targetOffset = new Vector3(targetOffset.x, 0, p.position.z - maxdist);
            }
            else if (poffset.z <= Offset.z - maxdist)
            {
                targetOffset = new Vector3(targetOffset.x, 0, p.position.z + maxdist);
            }
            targetOffset = new Vector3(targetOffset.x, Mathf.Clamp(p.position.y, min.y, max.y), targetOffset.z);
        }

        Vector3 dir = p.localToWorldMatrix * Vector3.forward;
        
        if (Vector3.Distance(Offset, targetOffset) > 0.01f)
        {
            Vector2 v = Offset - targetOffset;
            Offset = Vector3.MoveTowards(Offset, targetOffset, 3.7f * Vector3.Distance(Offset, targetOffset) * Time.deltaTime);
        }
        else
            Offset = targetOffset;
        Vector3 targetpos = center;
        if (SmoothMovement)
            targetpos += Offset;
        else
            targetpos += targetOffset;
        targetpos += -dir * distance;

        transform.position = targetpos;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    void RemoveScreen()
    {
        scr.Play("FADEOUT");
    }
}
