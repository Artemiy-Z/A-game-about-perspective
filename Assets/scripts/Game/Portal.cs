using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public PlayerMovement pm;
    private Material pmat;
    public float delay;
    public bool openingstart = false;
    public GameObject psF;
    public GameObject psB;
    public GameObject psC;
    public bool psCEmitted = false;
    public float facing;
    public GameObject W;
    private Trig tr;
    public AudioSource Pop;
    public AudioSource JIP;
    public bool PlayerJumpedInPortal = false;
    private bool Dis = false;

    private void Awake()
    {
        int i = 2;
        foreach (Transform t in transform)
            if (i > 0)
            { t.gameObject.SetActive(false); i--; }
        W.SetActive(false);
        pmat = GetComponent<MeshRenderer>().material;
        pmat.SetFloat("Diameter", 0);
        tr = GetComponentInChildren<Trig>();
    }

    private void Update()
    {
        if (Dis)
        {
            if (pm.GetComponent<PlayerSynchronization>().psr.transform.localScale.x > 0)
                pm.GetComponent<PlayerSynchronization>().psr.transform.localScale -= Vector3.one * Time.deltaTime;
            if(pm.GetComponent<PlayerSynchronization>().psr.color.a > 0)
            {
                pm.GetComponent<PlayerSynchronization>().psr.color = new Color(pm.GetComponent<PlayerSynchronization>().psr.color.r, pm.GetComponent<PlayerSynchronization>().psr.color.g, pm.GetComponent<PlayerSynchronization>().psr.color.b, pm.GetComponent<PlayerSynchronization>().psr.color.a - Time.deltaTime);
            }
            pm.p3d.Rotate(pm.p3d.localToWorldMatrix * Vector3.back, 360 * Time.deltaTime);

            return;
        }

        W.SetActive(false);
        float lrot = pm.GetComponent<PlayerSynchronization>().lrot;

        if (lrot != facing && !((lrot == 90 || lrot == -270 || lrot == -90 || lrot == 270) && pm.GetComponent<PlayerSynchronization>().tarrot % 360 == 0))
        {
            gameObject.layer = 8;
            int i = 2;
            foreach (Transform t in transform)
                if (i > 0)
                { t.gameObject.layer = 8; i--; }
        }
        else
        {
            gameObject.layer = 5;
            int i = 2;
            foreach (Transform t in transform)
                if (i > 0)
                { t.gameObject.layer = 5; i--; }
        }

        if(Mathf.Abs(pm.GetComponent<PlayerSynchronization>().tarrot) == 180 + facing)
        {
            psF.layer = 8;
            psB.layer = 0;
        }
        else
        {
            psF.layer = 5;
            psB.layer = 8;
        }

        if (pm.CollectablesCount != 4)
            return;
        
        if (delay <= 0)
        {
            if (pmat.GetFloat("Diameter") != 0.8f)
            {
                if (!psCEmitted)
                {
                    psC.GetComponent<ParticleSystem>().Emit(100);
                    Pop.Play();
                    psCEmitted = true;
                }
                pmat.SetFloat("Diameter", Mathf.Clamp(pmat.GetFloat("Diameter") + Time.deltaTime * 2f, 0, 0.8f));
                foreach (Transform t in transform)
                    t.gameObject.SetActive(true);
            }
            else 
            {
                if (tr.Triger())
                {
                    W.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.W)) 
                    {
                        PlayerJumpedInPortal = true;
                        return;
                    }

                    if (PlayerJumpedInPortal && pm.GetComponent<Rigidbody2D>().velocity.y <= -9f)
                    {
                        JIP.Play();
                        pm.anim.SetBool("Fade", true);
                        pm.GetComponent<PlayerSynchronization>().enabled = false;
                        pm.GetComponent<PlayerSynchronization>().psr.GetComponent<Animator>().enabled = false;
                        pm.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                        Invoke("invlod", 0.8f);
                        psC.transform.position = pm.transform.position;
                        psC.GetComponent<ParticleSystem>().Emit(120);
                        Dis = true;
                    }
                }
            }
        }
        else
        {
            delay -= Time.deltaTime;
            if (!openingstart)
            {
                psF.GetComponent<ParticleSystem>().Play();
                psB.GetComponent<ParticleSystem>().Play();

                openingstart = true;

                GetComponent<AudioSource>().Play();
            }
        }
    }

    void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    void invlod()
    {
        try { GameObject.FindGameObjectWithTag("LoadGFX").GetComponent<Animation>().Play("FADEIN");}catch{ }
        
        Invoke("LoadMenu", GameObject.FindGameObjectWithTag("LoadGFX").GetComponent<Animation>().clip.length + 0.7f);
    }
}
