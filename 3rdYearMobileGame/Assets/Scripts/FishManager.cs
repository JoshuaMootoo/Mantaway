using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public GameObject fishPrefab;

    public Transform targetObject;

    public List<FishController> FishList;



    // Start is called before the first frame update
    void Start()
    {
        FishList = new List<FishController>();
        foreach(Transform child in transform)
        {
            if(child.tag == "Fish")
            {
                FishList.Add(child.GetComponent<FishController>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FishController fish in FishList)
        {
            if (fish.following == true & (Time.time - fish.startTime) > 0.5f)
            {

                fish.Movement(targetObject.position,Time.deltaTime);
            }
        }
    }

    public void SpawnFish(int fishToSpawn, Vector3 position)
    {
        for (int i = 0; i < fishToSpawn; i++)
        {
            GameObject InstantiatedFish = Instantiate(fishPrefab, position, Quaternion.Euler(0,0,0));

            InstantiatedFish.GetComponent<FishController>().following = true;
            //InstantiatedFish.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)).normalized * 5, ForceMode.Impulse);
            targetObject.gameObject.GetComponent<PlayerController>().foundFish += 1;

            FishList.Add(InstantiatedFish.GetComponent<FishController>());
        }
    }
}
