using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCut : MonoBehaviour {


    private void OnCollisionEnter2D(Collision2D collision)
    {
      StartCoroutine(EdgeFall());
    }

    IEnumerator EdgeFall()
    {
        while (true)
        {
            transform.Translate(-0.2f * Time.deltaTime, -0.4f * Time.deltaTime, 0, Space.Self)  ;
            yield return new WaitForSeconds(0.01f);
        }
	}
}
