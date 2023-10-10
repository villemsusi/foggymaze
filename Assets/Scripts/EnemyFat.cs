using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFat : MonoBehaviour
{
    private Enemy Instance;

    float speed = 2;
    float health = 20;

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
