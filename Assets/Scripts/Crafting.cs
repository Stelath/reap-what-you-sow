using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Crafting : MonoBehaviour
{
    public Holding holding;


    public GameObject Pie;
    public GameObject Cake;
    public GameObject Salmon;
    public GameObject Knife;
    public GameObject Gun;
    public GameObject Poison;
    public GameObject LCake;
    public GameObject Vaccine;
    public GameObject TNT;
    public GameObject Flower;
    public GameObject WeddingCake;

    public List<GameObject> items;

    
    // Start is called before the first frame update
    void Start()
    {   
        items = new List<GameObject>();
        findAllItems();
    }

    void findAllItems(){
        items.Clear();
        ItemPickup[] t = (ItemPickup[]) FindObjectsOfType<ItemPickup>();
        foreach(ItemPickup item in t){
            items.Add(item.gameObject);
        }

    }

    void RemoveItem(GameObject item){
        items.Remove(item);
        Destroy(item);
    }

    // Called after poisoning something to destroy the poison
    void RemoveJustHeldItem(){
        RemoveItem(holding.thing);
    }

    void RemoveCraftingStuff(GameObject item1, GameObject item2){
        RemoveItem(item1);
        RemoveItem(item2);
    }

    string removeCloneFromEnd(string inName){
        if (inName.Length > 7){
            if (String.Equals(inName.Substring(inName.Length - 7), "(Clone)")){
                    inName = inName.Substring(0, inName.Length - 7);
                }
        }
        return inName;
    }

    void Craft(GameObject item1, GameObject item2){
        GameObject itemA;
        GameObject itemB;
        // Trying to test both orders item 1 + item 2 and item 2 + item 1
        for (int i = 0; i < 2; i++){
            if (i==0){
                itemA = item1;
                itemB = item2;
            } else {
                itemA = item2;
                itemB = item1;
            }
            string nameA = removeCloneFromEnd(itemA.name);
            string nameB = removeCloneFromEnd(itemB.name);
            

            Vector3 pos = new Vector3();
            pos = itemA.transform.position;

            if (String.Equals(nameA, "Knife")){
                if (String.Equals(nameB, "Cake")){
                    items.Add(Instantiate(LCake, pos, Quaternion.identity));
                    RemoveCraftingStuff(itemA, itemB);
                    
                } else if (String.Equals(nameB, "Medicine")){
                    items.Add(Instantiate(Vaccine, pos, Quaternion.identity));
                    RemoveCraftingStuff(itemA, itemB);
                }
            } else if (String.Equals(nameA, "Flower")){
                if (String.Equals(nameB, "Cake")){
                    items.Add(Instantiate(WeddingCake, pos, Quaternion.identity));
                    RemoveCraftingStuff(itemA, itemB);
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (holding.thing != null){
            findAllItems();
            //Debug.Log(items.Count);
            for (int i1 = 0; i1 < items.Count; i1++){
                GameObject check_item = items[i1];
                
                if (check_item != holding.thing){
                    float dist = Vector2.Distance(transform.position, check_item.transform.position);
                    if (dist < 2){
                        // Checking each crafting recipe
                        if (String.Equals(removeCloneFromEnd(holding.thing.name), "Poison")){
                            check_item.GetComponent<ItemPickup>().poisoned = true;
                            RemoveJustHeldItem();
                        } else {
                            Craft(holding.thing, check_item);
                        }
                    }
                }
            }
        }
        
    }
}
