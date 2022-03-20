using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitmarker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waits());
    }

    private IEnumerator waits()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}