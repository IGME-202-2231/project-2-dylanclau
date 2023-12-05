using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    // ----- fields ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    [SerializeField] int numberToSpawn;
    [SerializeField] GameObject prefab;
    [SerializeField] float standDev;
    [SerializeField] Manager manager;

    // prevent non-singleton constructor use.
    protected SpawnManager() { }



    // ----- start ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        Spawn(numberToSpawn);
    }



    // ----- update ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {

    }



    // ----- methods --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Spawn(int numSpawn)
    {
        Vector2 location;

        for (int i = 0; i < numSpawn; i++)
        {
            float x = Gaussian(0, standDev);
            float y = Gaussian(0, standDev);

            while(x == 0 && y == 0)
            {
                x = Gaussian(0, standDev);
                y = Gaussian(0, standDev);
            }


            location = new Vector3(x, y, 0);
            GameObject thing = Obstacle.Instantiate(prefab, location, Quaternion.identity);
            Wanderer script = thing.GetComponent<Wanderer>();
            if(script != null)
            {
                script.Manager = manager;
            }
            manager.Obstacles.Add(thing.GetComponent<Obstacle>());
            

        }
    }

    private float Gaussian(float mean, float stdDev)
    {
        float val1 = Random.Range(0f, 1f);
        float val2 = Random.Range(0f, 1f);

        float gaussValue =
        Mathf.Sqrt(-2.0f * Mathf.Log(val1)) *
        Mathf.Sin(2.0f * Mathf.PI * val2);

        return mean + stdDev * gaussValue;
    }
}
