using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSynchronization : MonoBehaviour
{
    public bool IsControllItself = true;

    PlayerMovement pmove;
    public Transform p;
    public SpriteRenderer psr;
    public GameObject psm;
    public float depthZ = -0.5f;
    public float depthX = 0f;
    public float curDepth;
    public float tarrot;
    private float t = -1;
    public float lrot;
    public float rotspeed = 1;

    public GameObject Front;
    public GameObject Back;
    public GameObject Right;
    public GameObject Left;

    public bool Hidden = false;
    public GameObject HiddenFront;
    public GameObject HiddenBack;
    public GameObject HiddenRight;
    public GameObject HiddenLeft;

    public ContactFilter2D cf;
    public LayerMask LayerMaskFor3DPlayer;

    public bool isOnBackground = false;
    public bool hasmoved = true;

    public bool changing = false;

    private Vector2 poldvel;

    public float tforplatforms;

    private void Awake()
    {
        pmove = GetComponent<PlayerMovement>();
        lrot = p.rotation.y;

        tarrot = 0;
        Front.SetActive(true);
        Back.SetActive(false);
        Right.SetActive(false);
        Left.SetActive(false);
    }

    private void Update()
    {
        if (t == -1)
        {
            changing = false;
            if (isOnBackground)
            {
                if (Physics.SphereCast(new Ray(p.position, -p.forward), 0.2f, out RaycastHit hit, LayerMaskFor3DPlayer))
                {
                    psm.SetActive(true);
                    if (tarrot % 360 == 0)
                    {
                        transform.parent = null;
                        Set3dPos(pmove.transform.position.y);
                        Front.SetActive(false);
                        Back.SetActive(true);
                        if(Hidden)
                            HiddenFront.SetActive(true);
                        Back.transform.localScale = new Vector3(-1, 1, 1);
                        Front.transform.localScale = new Vector3(1, 1, 1);
                    }
                    if (tarrot == 90 || tarrot == -270)
                    {
                        transform.parent = null;
                        Set3dPos(pmove.transform.position.y);
                        Right.SetActive(true);
                        Left.SetActive(false);
                        if (Hidden)
                            HiddenLeft.SetActive(true);
                        Right.transform.localScale = new Vector3(-1, 1, 1);
                        Left.transform.localScale = new Vector3(1, 1, 1);
                    }
                    if (tarrot == 180 || tarrot == -180)
                    {
                        transform.parent = null;
                        Set3dPos(pmove.transform.position.y);
                        Front.SetActive(true);
                        Back.SetActive(false);
                        if (Hidden)
                            HiddenBack.SetActive(true);
                        Front.transform.localScale = new Vector3(-1, 1, 1);
                        Back.transform.localScale = new Vector3(1, 1, 1);
                    }
                    if (tarrot == -90 || tarrot == 270)
                    {
                        transform.parent = null;
                        Set3dPos(pmove.transform.position.y);
                        Right.SetActive(false);
                        Left.SetActive(true);
                        if (Hidden)
                            HiddenRight.SetActive(true);
                        Right.transform.localScale = new Vector3(1, 1, 1);
                        Left.transform.localScale = new Vector3(-1, 1, 1);
                    }
                }
                else
                { 
                    isOnBackground = false; checkrot(pmove.transform.position.y); 
                    psr.sortingLayerName = "Sprite"; 
                    psm.SetActive(false);
                }
            }

            SetDepthBy2D(pmove.transform.position.x);

            if (!isOnBackground)
            {
                if (Hidden)
                {
                    HiddenFront.SetActive(false);
                    HiddenBack.SetActive(false);
                    HiddenLeft.SetActive(false);
                    HiddenRight.SetActive(false);
                }
                if (hasmoved)
                {
                    List<RaycastHit2D> result = new List<RaycastHit2D>();
                    if (Physics2D.Raycast(pmove.transform.position, Vector2.down, cf, result, 0.63f) != 0)
                    {
                        if (result.ToArray()[0].collider.transform.position.y <= pmove.transform.position.y - 1f)
                        {
                            SetDepth(pmove.transform.position.x, result.ToArray()[0].collider.transform.gameObject.GetComponent<BlockParams>().depth);
                            curDepth = result.ToArray()[0].collider.transform.gameObject.GetComponent<BlockParams>().depth;
                        }
                    }
                }
            }

            if (pmove.inp != new Vector2())
                hasmoved = true;

            tforplatforms += Time.deltaTime;

            if (IsControllItself)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Rotate(tarrot - 90, pmove.transform.position.y);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Rotate(tarrot + 90, pmove.transform.position.y);
                }
            }
        }
        else if(t < Mathf.PI * 0.5f)
        {
            if (Physics.SphereCast(new Ray(p.position, -p.forward), 0.2f, out RaycastHit hit, LayerMaskFor3DPlayer))
            {
                psm.SetActive(true);
            }
            else
                psm.SetActive(false);
            p.rotation = Quaternion.Euler(0, Mathf.Lerp(lrot, tarrot, Mathf.Sin(t)), 0);

            t += Time.deltaTime * rotspeed;
        }
        else if(t >= Mathf.PI * 0.5f)
        {
            changing = true;
            t = -1;
            tarrot = tarrot % 360;
            p.rotation = Quaternion.Euler(0, tarrot, 0);
            lrot = tarrot;
            isOnBackground = true;
            poldvel.x = 0;
            if (pmove.OnGround())
                hasmoved = false;
            else
                hasmoved = true;
            pmove.enabled = true;
            pmove.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void LateUpdate()
    {
        Set3dPos(pmove.transform.position.y);
        Camera.main.GetComponent<CameraMovement>().MoveToPlayer();
        psm.GetComponent<SpriteMask>().sprite = psr.sprite;
        psm.transform.localScale = psr.transform.localScale;
        psm.transform.localPosition = psr.transform.localPosition;
    }

    public void Rotate(float angle, float h)
    {
        transform.parent = null;
        tforplatforms -= Time.deltaTime;
        tarrot = angle;
        poldvel = pmove.GetComponent<Rigidbody2D>().velocity;
        pmove.transform.position = pmove.rb.position + Vector2.up * 0.015f;
        pmove.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        psr.sortingLayerName = "Default";
        changing = true;
        checkrot(h);
        pmove.enabled = false;
        t = 0;
    }

    public void checkrot(float h)
    {
        if (tarrot % 360 == 0)
        {
            pmove.transform.position = new Vector3(depthX, h);
            Front.SetActive(true);
            Back.SetActive(false);
            Right.SetActive(false);
            Left.SetActive(false);
            if (Hidden)
                HiddenFront.SetActive(true);
        }
        if (tarrot == 90 || tarrot == -270)
        {
            pmove.transform.position = new Vector3(-depthZ, h);
            Front.SetActive(false);
            Back.SetActive(false);
            Right.SetActive(false);
            Left.SetActive(true);
            if (Hidden)
                HiddenLeft.SetActive(true);
        }
        if (tarrot == 180 || tarrot == -180)
        {
            pmove.transform.position = new Vector3(-depthX, h);
            Front.SetActive(false);
            Back.SetActive(true);
            Right.SetActive(false);
            Left.SetActive(false);
            if (Hidden)
                HiddenBack.SetActive(true);
        }
        if (tarrot == -90 || tarrot == 270)
        {
            pmove.transform.position = new Vector3(depthZ, h);
            Front.SetActive(false);
            Back.SetActive(false);
            Right.SetActive(true);
            Left.SetActive(false);
            if (Hidden)
                HiddenRight.SetActive(true);
        }
        Front.transform.localScale = new Vector3(1, 1, 1);
        Back.transform.localScale = new Vector3(1, 1, 1);
        Left.transform.localScale = new Vector3(1, 1, 1);
        Right.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Set2dPos(float height)
    {
        pmove.enabled = false;
        pmove.rb.isKinematic = true;
        if (tarrot % 360 == 0)
        {
            transform.position = new Vector3(depthX, height);
        }
        if (tarrot == 90 || tarrot == -270)
        {
            pmove.transform.position = new Vector3(-depthZ, height);
        }
        if (tarrot == 180 || tarrot == -180)
        {
            pmove.transform.position = new Vector3(-depthX, height);
        }
        if (tarrot == -90 || tarrot == 270)
        {
            pmove.transform.position = new Vector3(depthZ, height);
        }
        pmove.rb.isKinematic = false;
        pmove.enabled = true;
    }

    public void Set3dPos(float height)
    {
        p.position = new Vector3(depthX, height, depthZ);
    }

    public void SetDepth(float X, float Z)
    {
        if (tarrot % 360 == 0)
        {
            depthX = X;
            depthZ = Z;
        }
        if (tarrot == 90 || tarrot == -270)
        {
            depthZ = -X;
            depthX = Z;
        }
        if (tarrot == 180 || tarrot == -180)
        {
            depthX = -X;
            depthZ = Z;
        }
        if (tarrot == -90 || tarrot == 270)
        {
            depthZ = X;
            depthX = Z;
        }
    }

    public void SetDepthBy2D(float X)
    {
        if (tarrot % 360 == 0)
        {
            depthX = X;
        }
        if (tarrot == 90 || tarrot == -270)
        {
            depthZ = -X;
        }
        if (tarrot == 180 || tarrot == -180)
        {
            depthX = -X;
        }
        if (tarrot == -90 || tarrot == 270)
        {
            depthZ = X;
        }
    }

    public void RotateByExternalControll(float angle) 
    {
        Rotate(tarrot - angle, pmove.transform.position.y);
    }
}
