using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;


public class NPCController : MonoBehaviour
{
    [SerializeField]
    public int yearsToLive = 100;
    [SerializeField]
    private TextMeshPro textMeshPro;

    [SerializeField]
    public GameObject path;

    [SerializeField]
    public GameObject reaper; 
    private PlayerController playercontroller;

    bool inRoute;
    int currentWaypoint; 
    GameObject[] waypoints;

    private Vector2 movement;
    private Rigidbody2D rb;

    public float speed;

    public float scare;

    private int pathDirection;

    public Vector3 scareVector;


    NPCSpriteHandler npcSpriteHandler;

    Vector3 randomVector;


    // Start is called before the first frame update
    void Start()
    {
        waypoints = path.transform.Cast<Transform>().Select(t=>t.gameObject).ToArray();
        inRoute = false;
        rb = GetComponent<Rigidbody2D>();
        
        speed = 4 * yearsToLive / 100 + 1;  // The more lively villagers move faster
        
        scare = 0;
        currentWaypoint = 0;

        playercontroller = reaper.GetComponent<PlayerController>();
        npcSpriteHandler = GetComponent<NPCSpriteHandler>();
        pathDirection = UnityEngine.Random.Range(0, 2) * 2 - 1;  // Will either be 1 or -1   
        randomVector = new Vector3(UnityEngine.Random.Range(-10, 10) * .001f, UnityEngine.Random.Range(-10, 10) * .001f, 0);
        
    }

    int ClosestWaypoint(){
        float shortestDistance = 10000000;
        int bestWaypoint = 0;
        float dist;
        for (int i = 0; i < waypoints.Count(); i++){
           
            dist = Vector3.Distance(waypoints[i].transform.position,
                                    transform.position);
            if (dist < shortestDistance){
                shortestDistance = dist;
                bestWaypoint = i;
            }
            
            
        }
        return bestWaypoint; 
    }

    void MoveTowardsWaypoint(){
        Vector2 movement = waypoints[currentWaypoint].transform.position - transform.position + randomVector;
        movement.Normalize();
        rb.velocity = movement * speed;
    }

    void TryNextWaypoint(){
        if (Vector3.Distance(waypoints[currentWaypoint].transform.position, transform.position) < .1){
            currentWaypoint += pathDirection;
            currentWaypoint = currentWaypoint % waypoints.Count();
            if (currentWaypoint < 0){
                currentWaypoint += waypoints.Count();
            }
        }
    }

    void UpdateScare(){
        if (playercontroller.inMainForm){
            float dist = Vector3.Distance(transform.position, reaper.transform.position);
            if (dist < 5){
                scare = 10 - dist;
                scareVector = new Vector3(reaper.transform.position.x, reaper.transform.position.y, reaper.transform.position.z);
            }
        }
        scare -= 1 * Time.deltaTime;
        if (scare > 10){
            scare = 10;
        } else if (scare < 0){
            scare = 0;
        }
    }

    void RunAway(){
        inRoute = false;
        Vector2 movement = scareVector - transform.position;
        movement.Normalize();
        rb.velocity = movement * speed * -1;
    }

    void Animate()
    {
        if(Math.Abs(rb.velocity.x) < 0.1 && Math.Abs(rb.velocity.y) < 0.1)
        {
            npcSpriteHandler.walking = false;
            return;
        }
        else
        {
            npcSpriteHandler.walking = true;
        }

        if(Math.Abs(rb.velocity.x) > Math.Abs(rb.velocity.y))
        {
            npcSpriteHandler.facingDirection = rb.velocity.x > 0 ? 0 : 2;
        }
        else
        {
            npcSpriteHandler.facingDirection = rb.velocity.y > 0 ? 1 : 3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = yearsToLive.ToString();

        UpdateScare();
        if (scare == 0){
            if (!inRoute){
                currentWaypoint = ClosestWaypoint();
                inRoute = true;
            } else {
                MoveTowardsWaypoint();
                TryNextWaypoint();
            }
        } else {
            RunAway();
        }

        Animate();
    }
}
