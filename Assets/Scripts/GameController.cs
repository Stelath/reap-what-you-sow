using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Game Modifiers")]
    [SerializeField]
    private int npcCount = 10;
    [SerializeField]
    private int itemCount = 6;
    [SerializeField]
    private int quota = 1;
    [SerializeField]
    private int additionalSouls = 1;
    private int decades = 0;

    private int souls = 0;


    [Header("Game Objects")]
    [SerializeField]
    private GameObject reaper;

    [SerializeField]
    private GameObject NPC;

    [Header("Spawnables")]
    [SerializeField]
    private GameObject[] paths;

    [SerializeField]
    private GameObject[] items;

    [Header("UI")]
    [SerializeField]
    private Fade blackOutPanel;
    [SerializeField]
    private TextMeshProUGUI roundText;
    [SerializeField]
    private TextMeshProUGUI timer;
    private float timeLeft;
    private bool timerOn = true;

    List<GameObject> npcs = new List<GameObject>();
    List<GameObject> spawnedItems = new List<GameObject>();

    void Start()
    {
        for(int i = 0; i < npcCount; i++)
        {
            SpawnNPC();
        }
        for(int i = 0; i < itemCount; i++)
        {
            SpawnItem();
        }
        NewRound(true);
    }

    void SpawnNPC()
    {
        GameObject npc = Instantiate(NPC);
        GameObject path = paths[Random.Range(0, paths.Length)];
        GameObject[] pathPoints = path.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
        Transform spawn = pathPoints[Random.Range(0, pathPoints.Length)].transform;

        npc.transform.position = spawn.transform.position;

        NPCController npcController = npc.GetComponent<NPCController>();
        npcController.reaper = reaper;
        npcController.path = path;
        npcController.yearsToLive = Random.Range(0, 100);
        npcs.Add(npc);
    }

    void SpawnItem()
    {
        GameObject item = Instantiate(items[Random.Range(1, items.Length)]);

        GameObject path = paths[Random.Range(0, paths.Length)];
        GameObject[] pathPoints = path.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
        Transform spawn = pathPoints[Random.Range(0, pathPoints.Length)].transform;

        item.transform.position = spawn.transform.position;
    }
    
    void Update()
    {
        if(timeLeft <= 0 && timerOn)
        {
            reaper.GetComponent<PlayerController>().enabled = false;
            reaper.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            UpdateGameState();
            NewRound();
        }
    }

    private void FixedUpdate()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        if (timerOn)
        {

            if (timeLeft > 0)
            {

                timeLeft -= Time.deltaTime;
            }
            if (timeLeft <= 0)
            {
                timeLeft = 0;
            }
        }
        timer.text = timeLeft.ToString("00.0");
    }

    void UpdateGameState()
    {
        foreach(GameObject npc in npcs)
        {
            NPCController npcController = npc.GetComponent<NPCController>();
            GameObject item = npc.GetComponent<Holding>().thing;
            ItemPickup itemController;
            bool poisoned = false;
            string itemName = "";
            if (item != null)
            {
                itemController = item.GetComponent<ItemPickup>();
                poisoned = itemController.poisoned;
                itemName = removeCloneFromEnd(item.name);
            }
            switch (itemName)
            {
                case "Pie":
                    npcController.yearsToLive -= poisoned ? 20 : 10;
                    break;
                case "Cake":
                    npcController.yearsToLive -= poisoned ? 40 : 30;
                    break;
                case "Salmon":
                    npcController.yearsToLive += poisoned ? 0 : 10;
                    break;
                case "Knife":
                    npcController.yearsToLive -= poisoned ? 50 : 10;
                    break;
                case "Gun":
                    npcController.yearsToLive -= 50;
                    break;
                case "Poison":
                    npcController.yearsToLive -= 0;
                    break;
                case "LCake":
                    npcController.yearsToLive -= 999;
                    break;
                case "Vaccine":
                    npcController.yearsToLive += poisoned ? 0 : 20;
                    break;
                case "TNT":
                    npcController.yearsToLive -= poisoned ? 10 : 5;
                    break;
                case "Flower":
                    npcController.yearsToLive += poisoned ? -10 : 10;
                    break;
                case "WeddingCake":
                    npcController.yearsToLive -= poisoned ? 50 : 40;
                    break;
            }

            npcController.yearsToLive -= 10;

            if(npcController.yearsToLive <= 0)
            {
                npcs.Remove(npc);
                Destroy(npc);
                souls += 1;
            }
        }

        foreach(GameObject item in spawnedItems)
        {
            Destroy(item);
        }

        decades += 1;
    }

    void NewRound(bool starting = false)
    {
        bool gameOver = false;
        int newNPCs = Random.Range(0, quota + 3);
        if(starting)
        {
            roundText.text = $@"You Are The Grim Reaper
And Need to Collect {quota} Souls
This Decade, Good Luck!";
        }
        else
        {
            if (souls >= quota)
            {
                roundText.text = $@"You Have Survived {decades} Decades
Another Decade Has Passed
{souls} People Died
{npcs.Count} Are Still Alive
{newNPCs} Were Born
You Met Your Quota of {quota} Souls
Your New Quota is  {quota + 1} Souls";
            }
            else
            {
                gameOver = true;
                roundText.text = $@"You Have Failed To Collect
Enough Souls ({souls} out of {quota})
You Lasted {decades} Decades as The Grim Reaper";
            }
        }

        timerOn = false;
        if (!gameOver)
            timeLeft = 60;

        if(!starting)
            quota += additionalSouls;
        souls = 0;

        for (int i = 0; i < newNPCs; i++)
        {
            SpawnNPC();
        }
        for (int i = 0; i < itemCount; i++)
        {
            SpawnItem();
        }

        StartCoroutine(DoFadeCycle());
    }

    IEnumerator DoFadeCycle() {
        Fade textFade = roundText.GetComponent<Fade>();
        StartCoroutine(blackOutPanel.DoFadeIn());
        yield return new WaitForSeconds(1);
        StartCoroutine(textFade.DoFadeIn());
        yield return new WaitForSeconds(5);
        StartCoroutine(textFade.DoFadeOut());
        yield return new WaitForSeconds(1);
        StartCoroutine(blackOutPanel.DoFadeOut());
        yield return new WaitForSeconds(2);
        reaper.GetComponent<PlayerController>().enabled = true;
        timerOn = true;
    }

    string removeCloneFromEnd(string inName)
    {
        if (inName.Length > 7)
        {
            if (System.String.Equals(inName.Substring(inName.Length - 7), "(Clone)"))
            {
                inName = inName.Substring(0, inName.Length - 7);
            }
        }
        return inName;
    }
}
