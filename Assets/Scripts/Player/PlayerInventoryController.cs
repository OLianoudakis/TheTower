using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    private List<ItemOfInterest> items = new List<ItemOfInterest>();

    public void AddItem (ItemOfInterest item)
    {        
        bool sameItemFound = false;
        foreach (ItemOfInterest currentItem in items)
        {
            if (currentItem.GetTag().Equals(item.GetTag()))
            {
                currentItem.SetQuantity(currentItem.GetQuantity() + item.GetQuantity());
                sameItemFound = true;
            }
        }

        if (!sameItemFound)
        {
            ItemOfInterest newItem = new ItemOfInterest();
            newItem.SetTag(item.GetTag());
            newItem.SetQuantity(item.GetQuantity());
            items.Add(newItem);
        }
    }

    public int FindItemOfType(ItemType requestedItemType, int requestedQuantity)
    {
        int requestedQuantityRemaining = requestedQuantity;
        foreach (ItemOfInterest item in items)
        {
            if(item.m_itemType == requestedItemType && item.m_quantity > 0)
            {
                --item.m_quantity;
                --requestedQuantityRemaining;
                if (item.m_quantity <= 0)
                {
                    RemoveItem(item);
                    break;
                }
                if (requestedQuantityRemaining <= 0)
                {
                    break;
                }
            }
        }
        return requestedQuantityRemaining;
    }

    private void RemoveItem(ItemOfInterest item)
    {
        items.Remove(item);
    }
}
