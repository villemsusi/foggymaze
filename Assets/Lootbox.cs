using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootbox : MonoBehaviour
{

    public List<Item> Items;
    private Item SelectedItem;

    // Start is called before the first frame update
    void Start()
    {
        int randomItemIndex = Random.Range(0, Items.Capacity);
        SelectedItem = Items[randomItemIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Open()
    {
        Item item = Instantiate(SelectedItem, transform.position, Quaternion.identity, null);

    }
}
