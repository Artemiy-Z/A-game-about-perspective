using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private float t;
    public float amplitude = 0.5f;
    public float Rotationspeed = 30;
    private Vector3 startPos;
    public bool collected = false;
    public GameObject ps;
    private Trig tr;
    public bool tri = false;

    private void Awake()
    {
        startPos = transform.position;
        tr = GetComponentInChildren<Trig>();
    }

    private void Update()
    {
        tri = tr.Triger();
        if (tr.Triger() && !collected)
        {
            collected = true;
            t = 0;
        }

        if (!collected)
        {
            t += Time.deltaTime;

            transform.position = startPos + Vector3.up * Mathf.Sin(t) * amplitude;
            transform.Rotate(Vector3.up, Rotationspeed * Time.deltaTime);
            return;
        }

        if(transform.localScale.x >= 0)
        {
            transform.localScale = Vector3.one * (Mathf.Sin(t) + 0.2f);
            t += Time.deltaTime * 4f * Mathf.PI;
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player2D").GetComponent<PlayerMovement>().CollectablesCount++;
            Instantiate(ps, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
