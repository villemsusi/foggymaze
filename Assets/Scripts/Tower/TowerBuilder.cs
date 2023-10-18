using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    public TowerData CurrentTowerData;
    public Color AllowColor;
    public Color DisableColor;
    private void Awake()
    {
        Events.OnTowerSelected += TowerSelected;
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        Events.OnTowerSelected -= TowerSelected;
    }
    void Start()
    {
        
    }
   

    // Update is called once per frame
    void Update()
    {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousepos = new Vector3(Mathf.Round(mousepos.x - 0.5f) + 0.5f, Mathf.Round(mousepos.y - 0.5f) + 0.5f, 0);
        transform.position = mousepos;
        //Verify that building area is free of other towers. 
        //By using a static overlap method from Physics2D class we can make this work without collider and a 2d rigidbody.
        bool free = IsFree(transform.position);
        //Tint the sprite to green or red accordingly.
        if (free)
        {
            TintSprite(AllowColor);
        }
        else
        {
            TintSprite(DisableColor);
        }
        //Call the build method when the player presses left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            Build();
        }
    }




    void TintSprite(Color col)
    {
        //no need for an array rn, but could be useful for futureproofing
        SpriteRenderer[] renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = col;
        }
    }
    bool IsFree(Vector3 pos)
    {
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(pos, 0.45f);
        foreach (Collider2D collision in overlaps)
        {
            if (!collision.isTrigger) return false;
        }
        return true;
    }
    private void TowerSelected(TowerData data)
    {
        CurrentTowerData = data;
        gameObject.SetActive(true);
    }
    void Build()
    {
        if (!IsFree(transform.position))
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        GameObject.Instantiate(CurrentTowerData.TowerPrefab, transform.position, Quaternion.identity, null);
        gameObject.SetActive(false);
    }
}
