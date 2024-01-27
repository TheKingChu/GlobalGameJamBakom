using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieSplash : MonoBehaviour
{
    public GameObject pieSplashPrefab;
    private float destroyDelay = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Audience"))
        {
            PlaySplashEffect(transform.position);
            Rigidbody pieRb = GetComponent<Rigidbody>();
            pieRb.isKinematic = true;
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = other.GetComponent<Rigidbody>();
            Destroy(gameObject, destroyDelay);
        }
        else if(other.CompareTag("ground"))
        {
            Destroy(gameObject, 0.2f);
        }
    }

    private void PlaySplashEffect(Vector3 position)
    {
        GameObject splashInstance = Instantiate(pieSplashPrefab, position, Quaternion.identity);
        GameObject pieObject = GameObject.FindGameObjectWithTag("Pie");

        if(pieObject != null )
        {
            splashInstance.transform.parent = pieObject.transform;
        }
    }
}
