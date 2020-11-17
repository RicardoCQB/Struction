using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LasagnaCollector : MonoBehaviour
{
    private int lasagnaCounter = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Lasagna")
        {
            Destroy(other.gameObject);
        }
    }
}
