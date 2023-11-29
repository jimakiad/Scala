using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinCol : MonoBehaviour
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
            // Coin collected
            GameManager.totalCoinsCollected++;
            Debug.Log("Coin collected! Total coins: " + GameManager.totalCoinsCollected);

            // Destroy the coin
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("GameManager is not assigned in CoinCol script.");
        }

    }

}
