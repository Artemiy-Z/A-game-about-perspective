using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOptimizer : MonoBehaviour
{
    public GameObject FrontSide;
    public GameObject BackSide;
    public GameObject RightSide;
    public GameObject LeftSide;
    private PlayerSynchronization ps;

    private void Awake()
    {
        ps = GameObject.FindObjectOfType<PlayerSynchronization>();
        float rot = ps.tarrot;
        float prevrot = ps.lrot;

        if (rot != prevrot)
        {
            if (rot % 360 == 0)
            {
                if (prevrot == 90 || prevrot == -270)
                {
                    SetACTIVES(1, 0, 0, 1);
                }
                if (prevrot == -90 || prevrot == 270)
                {
                    SetACTIVES(1, 0, 1, 0);
                }
            }
            if (rot == 90 || rot == -270)
            {
                if (prevrot % 360 == 0)
                {
                    SetACTIVES(1, 0, 0, 1);
                }
                if (prevrot == 180 || prevrot == -180)
                {
                    SetACTIVES(0, 1, 0, 1);
                }
            }
            if (rot == 180 || rot == -180)
            {
                if (prevrot == 90 || prevrot == -270)
                {
                    SetACTIVES(0, 1, 0, 1);
                }
                if (prevrot == -90 || prevrot == 270)
                {
                    SetACTIVES(0, 1, 1, 0);
                }
            }
            if (rot == -90 || rot == 270)
            {
                if (prevrot % 360 == 0)
                {
                    SetACTIVES(1, 0, 1, 0);
                }
                if (prevrot == 180 || prevrot == -180)
                {
                    SetACTIVES(0, 1, 1, 0);
                }
            }
        }
        else if (rot == prevrot)
        {
            if (rot % 360 == 0)
            {
                SetACTIVES(1, 0, 0, 0);
            }
            if (rot == 90 || rot == -270)
            {
                SetACTIVES(0, 0, 0, 1);
            }
            if (rot == 180 || rot == -180)
            {
                SetACTIVES(0, 1, 0, 0);
            }
            if (rot == -90 || rot == 270)
            {
                SetACTIVES(0, 0, 1, 0);
            }
        }
    }

    private void LateUpdate()
    {
        if (ps.changing)
        {
            float rot = ps.tarrot;
            float prevrot = ps.lrot;

            if (rot != prevrot)
            {
                if (rot % 360 == 0)
                {
                    if (prevrot == 90 || prevrot == -270)
                    {
                        SetACTIVES(1, 0, 0, 1);
                    }
                    if (prevrot == -90 || prevrot == 270)
                    {
                        SetACTIVES(1, 0, 1, 0);
                    }
                    if(prevrot == 180 || prevrot == -180)
                    {
                        SetACTIVES(1, 1, 1, 1);
                    }
                }
                if (rot == 90 || rot == -270)
                {
                    if (prevrot % 360 == 0)
                    {
                        SetACTIVES(1, 0, 0, 1);
                    }
                    if (prevrot == 180 || prevrot == -180)
                    {
                        SetACTIVES(0, 1, 0, 1);
                    }
                    if (prevrot == -90 || prevrot == 270)
                    {
                        SetACTIVES(1, 1, 1, 1);
                    }
                }
                if (rot == 180 || rot == -180)
                {
                    if (prevrot == 90 || prevrot == -270)
                    {
                        SetACTIVES(0, 1, 0, 1);
                    }
                    if (prevrot == -90 || prevrot == 270)
                    {
                        SetACTIVES(0, 1, 1, 0);
                    }
                    if (prevrot % 360 == 0)
                    {
                        SetACTIVES(1, 1, 1, 1);
                    }
                }
                if (rot == -90 || rot == 270)
                {
                    if (prevrot % 360 == 0)
                    {
                        SetACTIVES(1, 0, 1, 0);
                    }
                    if (prevrot == 180 || prevrot == -180)
                    {
                        SetACTIVES(0, 1, 1, 0);
                    }
                    if (prevrot == 90 || prevrot == -270)
                    {
                        SetACTIVES(1, 1, 1, 1);
                    }
                }
            }
            else if (rot == prevrot)
            {
                if (rot % 360 == 0)
                {
                    SetACTIVES(1, 0, 0, 0);
                }
                if (rot == 90 || rot == -270)
                {
                    SetACTIVES(0, 0, 0, 1);
                }
                if (rot == 180 || rot == -180)
                {
                    SetACTIVES(0, 1, 0, 0);
                }
                if (rot == -90 || rot == 270)
                {
                    SetACTIVES(0, 0, 1, 0);
                }
            }
        }
    }

    void SetACTIVES(int f, int b, int r, int l)
    {
        if(FrontSide)
            FrontSide.SetActive(inttobol(f));
        if(BackSide)
            BackSide.SetActive(inttobol(b));
        if(RightSide)
            RightSide.SetActive(inttobol(r));
        if(LeftSide)
            LeftSide.SetActive(inttobol(l));
    }

    bool inttobol(int a)
    {
        if (a == 1)
            return true;
        else return false;
    }
}
