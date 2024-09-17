using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverSelf : MonoBehaviour
{
    public float miny;
    public float maxy;
    public float startspeed;
    public float cury = 0;
    public float speed;
    private Vector3 lpos;

    public PlayerSynchronization ps;

    private Vector3 startpos;

    private void Awake()
    {
        startpos = transform.position;
    }

    private void OnEnable()
    {
        lpos = transform.position;
        cury = ps.tforplatforms;
        if (GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().MovePosition(startpos + Vector3.up * (Mathf.Sin(startspeed * cury) * (maxy / 2) + (maxy / 2)));
        }
        else if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().MovePosition(startpos + Vector3.up * (Mathf.Sin(startspeed * cury) * (maxy / 2) + (maxy / 2)));
        }
        else
        {
            transform.position = startpos + Vector3.up * (Mathf.Sin(cury) * (maxy / 2) + (maxy / 2));
        }
        speed = transform.position.y - lpos.y;
    }

    private void Update()
    {
        lpos = transform.position;
        cury = ps.tforplatforms;
        if (GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().MovePosition(startpos + Vector3.up * (Mathf.Sin(startspeed * cury) * (maxy / 2) + (maxy / 2)));
        }
        else if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().MovePosition(startpos + Vector3.up * (Mathf.Sin(startspeed * cury) * (maxy / 2) + (maxy / 2)));
        }
        else
        {
            transform.position = startpos + Vector3.up * (Mathf.Sin(cury) * (maxy / 2) + (maxy / 2));
        }
        speed = transform.position.y - lpos.y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player2D")) return;
        if (collision.transform.parent == null)
        {
            collision.transform.parent = transform;
            collision.transform.GetComponent<PlayerMovement>().forceground = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player2D")) return;
        if (collision.transform.parent == null)
        {
            collision.transform.parent = transform;
            collision.transform.GetComponent<PlayerMovement>().forceground = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player2D")) return;
        if(collision.transform.parent != null)
            collision.transform.parent = null;
        collision.transform.GetComponent<PlayerMovement>().forceground = false;
    }
}
