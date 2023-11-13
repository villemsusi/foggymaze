using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootbox : MonoBehaviour
{

    public Item UpgradePrefab;
    public Item HealthOrbPrefab;
    public Item AmmoPrefab;

    private Item SelectedItem;

    // Start is called before the first frame update
    void Start()
    {
        float randomItem = Random.Range(0f, 1f);
        if (randomItem <= 0.4)
            SelectedItem = AmmoPrefab;
        else if (randomItem <= 0.8)
            SelectedItem = UpgradePrefab;
        else
            SelectedItem = HealthOrbPrefab;
    }

    
    public void Open()
    {
        DataManager.Instance.OpenBoxAudio.Play();

        Instantiate(SelectedItem, transform.position, Quaternion.identity, null);
        Events.RemoveInteractable(gameObject);
        Destroy(gameObject);
    }


    public void SetSelectedItem(Item item)
    {
        SelectedItem = item;
    }
}
