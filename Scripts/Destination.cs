using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Destination : MonoBehaviour
{
    [SerializeField]
    public GameManager GameManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {

        if (GameManager != null)
        {

            GameManager.destinationReached = true;


            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Error.");
        }

    }

}
