using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public GameObject fishPrefab;

    public Transform targetObject;

    //fish actively following the player
    public List<FishController> FishList;
    public List<FishController> FishList2;
    public List<FishController> FishList3;

    //past positions of the player used to guide the fish 
    public List<Vector3> targetPositions;
    public List<Vector3> targetPositions2;
    public List<Vector3> targetPositions3;

    //Recorded speeds of the player for each of their past positions
    public List<float> speeds;
    public List<float> speeds2;
    public List<float> speeds3;

    PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        FishList = new List<FishController>();
        FishList2 = new List<FishController>();
        FishList3 = new List<FishController>();

        
        player = targetObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //pastpositons gotten from player object
        targetPositions = targetObject.GetComponent<PlayerController>().pastPositionList;
        targetPositions2 = targetObject.GetComponent<PlayerController>().pastPositionList2;
        targetPositions3 = targetObject.GetComponent<PlayerController>().pastPositionList3;

        speeds = player.speedList;
        speeds2 = player.speedList2;
        speeds3 = player.speedList3;

        //iterate through all the fish in each list to move them 
        int i = 0;
        foreach (FishController fish in FishList)
        {

            if (fish.following == true)
            {
                fish.fishNumber = i;
                fish.fishSpeed = speeds[i];
                fish.Movement(targetPositions[i],targetObject.position,Time.deltaTime);
                
            }
            i++;
        }
        i = 0;
        foreach (FishController fish in FishList2)
        {

            if (fish.following == true)
            {
                fish.fishNumber = i;
                fish.fishSpeed = speeds2[i];
                fish.Movement(targetPositions2[i], targetObject.position, Time.deltaTime);

            }
            i++;
        }
        i = 0;
        foreach (FishController fish in FishList3)
        {

            if (fish.following == true)
            {
                fish.fishNumber = i;
                fish.fishSpeed = speeds3[i];
                fish.Movement(targetPositions3[i], targetObject.position, Time.deltaTime);

            }
            i++;
        }

        //calculate current number of fish the player has
        player.foundFish = FishList.Count + FishList2.Count + FishList3.Count;
    }

    //Spawn fish out of crates and assign them to 1 of three possible lists of fish
    int fishInsertTick = 0;
    public void SpawnFish(int fishToSpawn, Vector3 position)
    {
        for (int i = 0; i < fishToSpawn; i++)
        {
            GameObject InstantiatedFish = Instantiate(fishPrefab, position, Quaternion.Euler(0,0,0));

            InstantiatedFish.GetComponent<FishController>().following = true;

            targetObject.gameObject.GetComponent<PlayerController>().foundFish += 1;

            if(fishInsertTick == 0)
                FishList.Insert(FishList.Count, InstantiatedFish.GetComponent<FishController>());
            if (fishInsertTick == 1)
                FishList2.Insert(FishList2.Count, InstantiatedFish.GetComponent<FishController>());
            if (fishInsertTick == 2)
                FishList3.Insert(FishList3.Count, InstantiatedFish.GetComponent<FishController>());
            fishInsertTick++;
            if (fishInsertTick > 2) fishInsertTick = 0;
        }
    }

    //Assign fish to 1 of 3 lists when the player touches them
    public void AddToList(FishController fish)
    {
        fish.GetComponent<FishController>().fishSpeed = targetObject.gameObject.GetComponent<PlayerController>().playerSpeed;
        

        if (fishInsertTick == 0)
        {
            fish.fishTeam = 1;
            FishList.Insert(FishList.Count, fish);
        }
            
        if (fishInsertTick == 1)
        {
            fish.fishTeam = 2;
            FishList2.Insert(FishList2.Count, fish);
        }
            
        if (fishInsertTick == 2)
        {
            fish.fishTeam = 3;
            FishList3.Insert(FishList3.Count, fish);
        }
            
        fishInsertTick++;
        if (fishInsertTick > 2) fishInsertTick = 0;
    }

    //Remove fish in the front of the queue when colliding with mines. Putting them into a dead state
    int fishRemoveTick = 0;
    public void RemoveFromList()
    {
        

        if (fishRemoveTick == 0)
        {
            FishController fish = FishList[0];
            fish.Dead();
            FishList.RemoveAt(0);
        }
            
        if (fishRemoveTick == 1)
        {
            FishController fish = FishList2[0];
            fish.Dead();
            FishList2.RemoveAt(0);
        }
            
        if (fishRemoveTick == 2)
        {
            FishController fish = FishList3[0];
            fish.Dead();
            FishList3.RemoveAt(0);
        }
            
        fishRemoveTick++;
        if (fishRemoveTick > 2) fishRemoveTick = 0;
        Debug.Log("FishremoveTick" + fishRemoveTick);
    }

    // Remove fish that contact the eels mouth. Must be removed from the list before being destroyed to avoid errors
    public void EatFish(int fishNumber, int fishTeam, FishController fish   )
    {
        if (fishTeam == 1)
        {
            FishList.RemoveAt(fishNumber);
            Destroy(fish.gameObject);
        }
        if (fishTeam == 2)
        {
            FishList2.RemoveAt(fishNumber);
            Destroy(fish.gameObject);
        }
        if (fishTeam == 3)
        {
            FishList3.RemoveAt(fishNumber);
            Destroy(fish.gameObject);
        }
    }

}
