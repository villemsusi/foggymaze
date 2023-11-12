using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LootboxTutorial : MonoBehaviour
{
    private Item SelectedItem;
 
    
    public void Open()
    {
        Instantiate(SelectedItem, transform.position, Quaternion.identity, null);
        Events.RemoveInteractable(gameObject);
        Destroy(gameObject);
    }


    public void SetSelectedItem(Item item)
    {
        SelectedItem = item;
    }
}
