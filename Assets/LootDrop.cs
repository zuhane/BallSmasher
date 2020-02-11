using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [SerializeField] private GameObject pickup;
    [SerializeField] [Range(1, 100)] private int quantity = 1;
    private float randomSpread = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        //If more than one inside, randomise position a little bit.
        if (pickup != null)
        {


            for (int i = 0; i < quantity; i++)
            {
                Vector3 random = Vector3.zero;
                if (quantity > 1) random = new Vector3(Random.Range(-randomSpread, randomSpread),
                    Random.Range(-randomSpread, randomSpread), 0);

                Instantiate(pickup, transform.position + random, Quaternion.identity, null);
            }


        }
            

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
