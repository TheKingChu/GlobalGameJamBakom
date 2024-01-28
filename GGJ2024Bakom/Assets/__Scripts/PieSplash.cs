//Created by Charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieSplash : MonoBehaviour
{
    public GameObject pieSplashPrefab;
    private float destroyDelay = 2.0f;
    private LaughMeter laughMeter;

    private bool hasHitAudience = false;
    private bool hasTriggeredNegativeEvent = false;

    private Rigidbody pieRb;
    private Collider sphereCollider;

    //SFX
    public AudioSource splashSound, booSound, laughterSound;

    public void Start()
    {
        laughMeter = GameObject.FindGameObjectWithTag("King").GetComponent<LaughMeter>();
        pieRb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Audience") || other.CompareTag("Jester") && !hasHitAudience)
        {
            PlaySplashEffect(transform.position);
            splashSound.Play();

            pieRb.isKinematic = true;
            sphereCollider.enabled = false;

            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = other.GetComponent<Rigidbody>();

            laughMeter.PositiveEvent();
            laughterSound.Play();
            hasHitAudience = true;

            Destroy(gameObject, destroyDelay);
        }
        else if(other.CompareTag("ground") || other.CompareTag("Wall") && !hasHitAudience && !hasTriggeredNegativeEvent)
        {
            PlaySplashEffect(transform.position);
            splashSound.Play();

            pieRb.isKinematic = true;
            sphereCollider.enabled = false;

            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = other.GetComponent<Rigidbody>();

            laughMeter.NegativeEvent();
            booSound.Play();
            hasTriggeredNegativeEvent = true;

            Destroy(gameObject, 0.5f);
        }
        else if (other.CompareTag("King") && !hasHitAudience && !hasTriggeredNegativeEvent)
        {
            PlaySplashEffect(transform.position);
            splashSound.Play();

            pieRb.isKinematic = true;
            sphereCollider.enabled = false;

            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = other.GetComponent<Rigidbody>();

            laughMeter.NegativeEvent();
            booSound.Play();
            hasTriggeredNegativeEvent = true;
            Destroy(gameObject, destroyDelay);
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
