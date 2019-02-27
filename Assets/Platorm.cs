using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platorm : MonoBehaviour
{
    public GameObject currentShadow;
    [HideInInspector]
    public Rigidbody2D[] thisRigid;
    [SerializeField]
    GameObject Shadow;
    bool hasTouched;
    
    
    private void Awake()
    {
        hasTouched = false;
        thisRigid = gameObject.GetComponentsInChildren<Rigidbody2D>();
        currentShadow = Instantiate(Shadow, transform);
        foreach (var item in thisRigid)
        {
            item.simulated = false;
        }
    }

    Vector2 a;
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (!hasTouched)
        {
            GameSys.Instance.Score++;
            float z = transform.rotation.eulerAngles.z * 0.0174533f;
            collision.rigidbody.bodyType = RigidbodyType2D.Static;
            collision.rigidbody.bodyType = RigidbodyType2D.Dynamic;
            collision.rigidbody.simulated = true;
            collision.rigidbody.AddForce(a = new Vector2(-Mathf.Sin(z) * 12, Mathf.Cos(z) * 10), ForceMode2D.Impulse);
            StartCoroutine(RigidDeactivate());
            StartCoroutine(PlatformFall());
            StartCoroutine(platformSet());
            GameSys.Instance.platformList.RemoveAt(0);
            Destroy(gameObject, 0.5f);
            hasTouched = true;
        }
    }

    IEnumerator platformSet()
    {
        yield return new WaitForSeconds(0.2f);
        GameSys.Instance.PlatformSet();
        GameSys.Instance.platformList[0].GetComponent<Platorm>().Activate();
    }

    IEnumerator RigidDeactivate()
    {
        yield return new WaitForSeconds(0.05f);
        foreach (var item in thisRigid)
        {
            item.simulated = false;
        }
        
    }

    IEnumerator PlatformFall()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            transform.Translate(new Vector3(0, -5 * Time.deltaTime));
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Activate()
    {
        Destroy(currentShadow);
        foreach (var item in thisRigid)
        {
            item.simulated = true;
        }
    }
}
