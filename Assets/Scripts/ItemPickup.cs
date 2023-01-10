using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField]
    private float pickupDistance = 2;


    Color greenColor = new Color(0, 50, 0);
    Color redColor = new Color(50, 0, 0);
    Color normalColor = new Color(255, 255, 255);

    public Holding personHeldBy = null;

    public bool poisoned = false;

    Holding[] people; // Everything that can carry an object

    void Start(){
        people = (Holding[]) FindObjectsOfType<Holding>();

    }


    // Update is called once per frame
    void Update()
    {   
        if (personHeldBy != null){
            // GetComponent<SpriteRenderer>().color = redColor;
            // I think someone is holding me
            // Checking if the person that is holding me is actually holding me
            if (personHeldBy.thing != gameObject){
                // They are not holding me!
                personHeldBy = null;
            } 
        } else {
            if (poisoned){
                GetComponent<SpriteRenderer>().color = greenColor;
            } else {
                GetComponent<SpriteRenderer>().color = normalColor;
            }
        }
        




    }
}
