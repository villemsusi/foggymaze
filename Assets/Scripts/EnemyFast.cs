using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFast : MonoBehaviour
{
    private Enemy Instance;

    float speed = 4;
    float health = 5;

    private void Awake()
    {
        Instance = GetComponent<Enemy>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Instance.Move(speed);
        Instance.HealthCheck(health);
    }
}
