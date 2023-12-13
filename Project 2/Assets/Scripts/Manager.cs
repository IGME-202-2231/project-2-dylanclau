using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] List<Obstacle> obstacles = new List<Obstacle>();
    [SerializeField] List<Agent> curses = new List<Agent>();
    [SerializeField] List<Agent> sorcerers = new List<Agent>();

    // spawn
    [SerializeField] float standDev;
    [SerializeField] GameObject prefab1;
    [SerializeField] int num1;
    [SerializeField] GameObject prefab2;
    [SerializeField] int num2;

    // properties
    public List<Obstacle> Obstacles { 
        get { return obstacles; }
        set { obstacles = value; }
    }

    public List<Agent> Curses
    {
        get { return curses; }
        set { curses = value; }
    }

    public List<Agent> Sorcerers
    {
        get { return sorcerers; }
        set { sorcerers = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        ObstacleSpawn();
        CurseSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Sorcerer s in Sorcerers)
        {
            s.ManageState();
        }

        foreach(Curse c in Curses)
        {
            c.ManageState();
        }
    }

    public void ObstacleSpawn()
    {
        Vector3 location;
        Vector3 rotation = new Vector3(0, 0, 0);

        for (int i = 0; i < num2; i++)
        {
            float x = Gaussian(0, standDev);
            float y = Gaussian(0, standDev);

            while (x == 0 && y == 0)
            {
                x = Gaussian(0, standDev);
                y = Gaussian(0, standDev);
            }

            location = new Vector3(x, y, 0);
            GameObject thing = Obstacle.Instantiate(prefab2, location, Quaternion.Euler(rotation));

            obstacles.Add(thing.GetComponent<Obstacle>());
        }
    }

    public void CurseSpawn()
    {
        Vector3 location;
        Vector3 rotation = new Vector3(0, 0, 0);

        for (int i = 0; i < num1; i++)
        {
            float x = Gaussian(0, standDev);
            float y = Gaussian(0, standDev);

            while (x == 0 && y == 0)
            {
                x = Gaussian(0, standDev);
                y = Gaussian(0, standDev);
            }

            location = new Vector3(x, y, 0);
            GameObject thing = Agent.Instantiate(prefab1, location, Quaternion.Euler(rotation));

            thing.GetComponent<Agent>().Manager = this;

            curses.Add(thing.GetComponent<Agent>());
        }
    }

    public void CurseSpawnUser()
    {
        Vector3 location;
        Vector3 rotation = new Vector3(0, 0, 0);

        for (int i = 0; i < 5; i++)
        {
            float x = Gaussian(0, standDev);
            float y = Gaussian(0, standDev);

            while (x == 0 && y == 0)
            {
                x = Gaussian(0, standDev);
                y = Gaussian(0, standDev);
            }

            location = new Vector3(x, y, 0);
            GameObject thing = Agent.Instantiate(prefab1, location, Quaternion.Euler(rotation));

            thing.GetComponent<Agent>().Manager = this;

            curses.Add(thing.GetComponent<Agent>());
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

    private bool CircleCollision(Bounds circle1, Bounds circle2)
    {
        float radius1 = circle1.max.y - circle1.center.y;
        float radius2 = circle2.max.x - circle2.center.x;
        float distance = Mathf.Sqrt(Mathf.Pow(circle1.center.x - circle2.center.x, 2) + Mathf.Pow(circle1.center.y - circle2.center.y, 2));

        if (distance < radius1 + radius2)
        {
            return true;
        }

        return false;
    }
}
