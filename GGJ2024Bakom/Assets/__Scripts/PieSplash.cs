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

    //SFX
    private AudioSource splashSound;

    public void Start()
    {
        laughMeter = GameObject.FindGameObjectWithTag("King").GetComponent<LaughMeter>();
        splashSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Audience") && !hasHitAudience)
        {
            PlaySplashEffect(transform.position);
            Rigidbody pieRb = GetComponent<Rigidbody>();
            pieRb.isKinematic = true;
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = other.GetComponent<Rigidbody>();
            laughMeter.PositiveEvent();
            hasHitAudience = true;

            Destroy(gameObject, destroyDelay);
        }
        else if(other.CompareTag("ground") && !hasHitAudience && !hasTriggeredNegativeEvent)
        {
            laughMeter.NegativeEvent();
            hasTriggeredNegativeEvent = true;
            Destroy(gameObject, 0.5f);
        }
        else if (other.CompareTag("King") && !hasHitAudience && !hasTriggeredNegativeEvent)
        {
            laughMeter.NegativeEvent();
            hasTriggeredNegativeEvent = true;
        }

        splashSound.Play();
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
