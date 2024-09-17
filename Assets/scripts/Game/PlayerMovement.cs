using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 inp;
    public bool isControlItself = true;

    public Rigidbody2D rb;
    public float speed = 5f;
    public float acceleration = 5f;
    public float jumpspeed = 5f;
    public float OffsetX;
    public float OffsetZ;
    public AnimationClip a;
    public int CollectablesCount = 0;
    public Animator anim;
    public AudioSource jump;
    public AudioSource step;
    private float groundtime = 0.5f;
    public Transform resp;
    public Transform p3d;
    public Restarter rest;
    public bool forceground = false;
    public bool dead = false;
    private float fallingtime = 0;

    private void Awake()
    {
        OffsetX = transform.position.x;
        OffsetZ = transform.position.z;
        transform.parent = null;
    }

    private void Update()
    {
        bool ground = true;
        if(!forceground)
            ground = OnGround();

        step.loop = false;

        if (isControlItself)
        {
            inp = new Vector2();
            if (Input.GetKey(KeyCode.A))
            {
                inp.x = -1;
                anim.transform.parent.localScale = new Vector2(-1, 1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                inp.x = 1;
                anim.transform.parent.localScale = new Vector2(1, 1);
            }

            if (Input.GetKeyDown(KeyCode.W) && ground)
            {
                inp.y = 1;
            }
        }
        else
        {
            if (inp.x == -1)
            {
                anim.transform.parent.localScale = new Vector2(-1, 1);
            }
            else if (inp.x == 1)
            {
                anim.transform.parent.localScale = new Vector2(1, 1);
            }

            if (!ground)
                inp.y = 0;
        }

        if (ground)
        {
            fallingtime = 0;

            if (dead)
            {
                rb.Sleep();
                rest.Invoke("Rel", 0.6f);
                this.enabled = false;
                return;
            }

            if (resp) {
                if (groundtime > 0)
                    groundtime -= Time.deltaTime;
                else
                {
                    Transform mt = OnMovingGround();
                    if (!mt && !forceground)
                    {
                        rest.itsrotation = GetComponent<PlayerSynchronization>().tarrot;
                        Transform nmt = GetGround();
                        if (GetComponent<PlayerSynchronization>().tarrot % 360 == 0)
                        {
                            resp.position = new Vector3(nmt.position.x, nmt.position.y + 1, nmt.GetComponent<BlockParams>().depth);
                        }
                        if (GetComponent<PlayerSynchronization>().tarrot == 90 || GetComponent<PlayerSynchronization>().tarrot == -270)
                        {
                            resp.position = new Vector3(nmt.GetComponent<BlockParams>().depth, nmt.position.y + 1, -nmt.position.x);
                        }
                        if (GetComponent<PlayerSynchronization>().tarrot == 180 || GetComponent<PlayerSynchronization>().tarrot == -180)
                        {
                            resp.position = new Vector3(-nmt.position.x, nmt.position.y + 1, nmt.GetComponent<BlockParams>().depth);
                        }
                        if (GetComponent<PlayerSynchronization>().tarrot == -90 || GetComponent<PlayerSynchronization>().tarrot == 270)
                        {
                            resp.position = new Vector3(nmt.GetComponent<BlockParams>().depth, nmt.position.y + 1, nmt.position.x);
                        }
                    }
                }
            }
            if (inp.x != 0)
            {
                if (rb.velocity.x > 0 && inp.x < 0)
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x - 2 * acceleration * Time.deltaTime, -speed, speed), rb.velocity.y);
                else if (rb.velocity.x < 0 && inp.x > 0)
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + 2 * acceleration * Time.deltaTime, -speed, speed), rb.velocity.y);
                else
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + inp.x * acceleration * Time.deltaTime, -speed, speed), rb.velocity.y);
                if (!step.isPlaying) step.Play();
                step.loop = true;
            }
            else
            {
                if (rb.velocity.x > 0)
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x - 2 * acceleration * Time.deltaTime, 0, speed), rb.velocity.y);
                else if (rb.velocity.x < 0)
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + 2 * acceleration * Time.deltaTime, -speed, 0), rb.velocity.y);
            }
        }
        else
        {
            if (fallingtime > 1.5f)
                dead = true;
            groundtime = 0.5f;
            //fallingtime += Time.deltaTime;
            if (inp.x != 0)
            {
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + 0.3f * acceleration * inp.x * Time.deltaTime, -speed, speed), rb.velocity.y);
            }
            else
            {
                if (rb.velocity.x > 0)
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x - 0.1f * acceleration * Time.deltaTime, 0, speed), rb.velocity.y);
                else if (rb.velocity.x < 0)
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + 0.1f * acceleration * Time.deltaTime, -speed, 0), rb.velocity.y);
            }
        }

        if (inp.y == 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpspeed);
            jump.Play();
        }
        else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        Transform mtt = OnMovingGround();
        if (mtt)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + mtt.GetComponent<MoverSelf>().speed);

        Vector2 vel = rb.velocity;
        if (ground)
        {
            anim.SetBool("jump", false);
            anim.SetBool("fall", false);
            if (Mathf.Abs(vel.x) >= 0.03f)
            {
                anim.SetBool("run", true);
                anim.SetFloat("runspeed", Mathf.Abs(vel.x));
            }
            else
            {
                anim.SetBool("run", false);
            }
        }
        else
        {
            if (vel.y > 0)
            {
                anim.SetBool("jump", true);
            }
            else if (vel.y <= 0)
            {
                anim.SetBool("fall", true);
            }
        }
    }

    public bool OnGround()
    {
        RaycastHit2D[] hit = new RaycastHit2D[4];
        int k = rb.Cast(Vector2.down, GetComponent<PlayerSynchronization>().cf, hit, 0.03f);
        if (k != 0)
            for(int i = 0; i < k; i++)
            {
                if(hit[i].collider)
                    if (hit[i].collider.transform.position.y <= transform.position.y - 1f && hit[i].collider.transform.position.y > transform.position.y - 2f)
                        return true;
            }
        return false;
    }

    public Transform OnMovingGround()
    {
        RaycastHit2D[] hit = new RaycastHit2D[4];
        int k = rb.Cast(Vector2.down, GetComponent<PlayerSynchronization>().cf, hit, 0.03f);
        if (k != 0)
            for (int i = 0; i < k; i++)
            {
                if (hit[i].collider)
                    if (hit[i].collider.transform.position.y <= transform.position.y - 1f && hit[i].collider.transform.position.y > transform.position.y - 2f && hit[i].collider.GetComponent<MoverSelf>())
                        return hit[i].collider.transform;
            }
        return null;
    }

    public Transform GetGround()
    {
        RaycastHit2D[] hit = new RaycastHit2D[4];
        int k = rb.Cast(Vector2.down, GetComponent<PlayerSynchronization>().cf, hit, 0.03f);
        if (k != 0)
            for (int i = 0; i < k; i++)
            {
                if (hit[i].collider)
                    if (hit[i].collider.transform.position.y <= transform.position.y - 1f && hit[i].collider.transform.position.y > transform.position.y - 2f)
                        return hit[i].collider.transform;
            }
        return null;
    }
}
