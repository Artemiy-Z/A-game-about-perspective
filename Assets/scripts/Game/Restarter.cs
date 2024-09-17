using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restarter : MonoBehaviour
{
    public enum types { Collider, Height };
    public types Type = types.Height;
    public bool UseRespawnPoints;
    public bool SetPlayerRotation;
    public float itsrotation;
    public AudioSource death;
    public GameObject p;

    bool trigered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player2D" && Type == types.Collider)
        {
            Rel();
        }
    }

    private void LateUpdate()
    {
        if (Type == types.Height && p.transform.position.y < transform.position.y && !trigered)
        {
            trigered = true;
            Camera.main.GetComponent<CameraMovement>().enabled = false;
            Invoke("Rel", 0.6f);
        }
    }

    void Rel()
    {
        if (UseRespawnPoints)
        {
            GameObject[] rpoints = GameObject.FindGameObjectsWithTag("Respawn");
            Vector3 ppos = p.transform.position;
            float min = -1f;
            Vector3 rr = rpoints[0].transform.position;
            foreach (GameObject g in rpoints)
            {
                Vector3 rpos = g.transform.position;
                float dist = Vector2.Distance(new Vector2(ppos.x, ppos.z), new Vector2(rpos.x, rpos.z));
                if(min == -1)
                {
                    min = dist;
                    rr = rpos;
                }
                else if(dist < min)
                {
                    min = dist;
                    rr = rpos;
                }
            }

            print(rr.y);

            PlayerSynchronization ps = p.GetComponent<PlayerSynchronization>();

            p.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ps.depthX = rr.x;
            ps.depthZ = rr.z;
            ps.Set2dPos(rr.y);
            if (SetPlayerRotation)
                ps.Rotate(itsrotation, rr.y);
            ps.Set3dPos(p.transform.position.y);
            Camera.main.GetComponent<CameraMovement>().enabled = true;
            trigered = false;
            death.Play();
            p.GetComponent<PlayerMovement>().enabled = true;
            p.GetComponent<PlayerMovement>().dead = false;
            p.GetComponent<PlayerMovement>().rb.WakeUp();
        }
        else
        {
            GameObject.FindGameObjectWithTag("LoadGFX").GetComponent<Animation>().Play("FADEIN");
            Invoke("RLOAD", GameObject.FindGameObjectWithTag("LoadGFX").GetComponent<Animation>().clip.length);
        }
    }

    void RLOAD()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
