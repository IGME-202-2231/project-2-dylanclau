using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] List<Obstacle> obstacles = new List<Obstacle>();

    public List<Obstacle> Obstacles
    {
        get { return obstacles; }
        set { obstacles = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
