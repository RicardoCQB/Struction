using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LasagnaCollector : MonoBehaviour
{
    private int lasagnaCounter = 0;

    public TextMeshProUGUI textLasagnaCounter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Lasagna")
        {
            lasagnaCounter++;
            // add sound
            textLasagnaCounter.text = lasagnaCounter.ToString();
            Destroy(other.gameObject);
        }
    }
}
