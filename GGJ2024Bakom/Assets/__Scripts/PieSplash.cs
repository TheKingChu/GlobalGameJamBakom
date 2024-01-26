using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieSplash : MonoBehaviour
{
    public GameObject pieSplashPrefab;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PieOnHit()
    {
        Instantiate(pieSplashPrefab);
    }
}
