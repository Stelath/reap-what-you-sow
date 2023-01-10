using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holding : MonoBehaviour
{   
    public float hold_looseness = 100;
    private float pickupDistance = 1;
    public GameObject thing = null;

    Color greenColor = new Color(0, 50, 0);
    Color redColor = new Color(50, 0, 0);
    Color normalColor = new Color(255, 255, 255);

    [SerializeField]
    NPCController followwaypoint; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float getDistance(Vector3 p1, Vector3 p2){
        float dist = Mathf.Sqrt(Mathf.Pow(p1.x - p2.x, 2) + Mathf.Pow(p1.y - p2.y, 2));
        return dist;
    }

    // Should only be called if this person is not already holding something i.e. (thing == null) 
    public void Grab(){
        // Getting all the items that I could grab
        ItemPickup[] itemsC = (ItemPickup[]) FindObjectsOfType<ItemPickup>();
        List<GameObject> items = new List<GameObject>();

        // Converting the component list to a GameObject list
        for (int i = 0; i < itemsC.Length; i++){
            items.Add(itemsC[i].gameObject);
        }

        for (int i = 0; i < items.Count; i++){
            // Checking if this item is not already held
            if (itemsC[i].personHeldBy == null){
                // Item is not being held
                float dist = getDistance(items[i].transform.position, transform.position);
                if (dist < pickupDistance){
                    // Setting the thing that I am holding to this thing
                    thing = items[i];
                    // Telling the item that it is being held by me
                    itemsC[i].personHeldBy = this;
                }    
            } else {
                
            }
            
        }
}

    public void Drop(){
        if (thing != null){
            // Telling the item that it is no longer being held
            thing.GetComponent<ItemPickup>().personHeldBy = null;
            thing = null; // Dropping what I have
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if (followwaypoint != null){ // The reaper does not have waypoints
            if (followwaypoint.scare > 0){
                Drop();
            }
        }
        if (thing != null){ // I have something
            // GetComponent<SpriteRenderer>().color = redColor;
            if (thing.GetComponent<ItemPickup>().personHeldBy != this){
                Drop();
            }
            Vector3 thingPos = thing.transform.position;
            Vector3 pos = transform.position;

            float distance = Vector3.Distance(thingPos, pos);
            
            thingPos.x -= (thingPos.x - pos.x) * (distance / hold_looseness);
            thingPos.y -= (thingPos.y - pos.y) * (distance / hold_looseness);
            
            thing.transform.position = thingPos;
        } else {
            // GetComponent<SpriteRenderer>().color = normalColor;
            // This person does not have anything
            Grab();
        }
    }
}
