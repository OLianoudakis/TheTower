using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    private List<ItemOfInterest> m_items = new List<ItemOfInterest>();
    private List<int> m_quantities = new List<int>();

    public void AddItem(ItemOfInterest item, int quantity)
    {        
        bool sameItemFound = false;

        for (int i = 0; i < m_items.Count; i++)
        {
            if (m_items[i].GetTag().Equals(item.GetTag()))
            {
                m_quantities[i] += quantity;
                sameItemFound = true;
                break;
            }
        }

        if (!sameItemFound)
        {
            ItemOfInterest newItem = new ItemOfInterest();
            newItem.SetTag(item.GetTag());
            m_items.Add(newItem);
            m_quantities.Add(quantity);
        }
    }

    public int FindItemOfType(ItemType requestedItemType, int requestedQuantity)
    {
        int requestedQuantityRemaining = requestedQuantity;

        for (int i = 0; i < m_items.Count; i++)
        {
            if (m_items[i].m_itemType == requestedItemType && m_quantities[i] > 0)
            {
                --m_quantities[i];
                --requestedQuantityRemaining;
                if (m_quantities[i] <= 0)
                {
                    RemoveItem(i);
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

    private void RemoveItem(int index)
    {
        m_items.RemoveAt(index);
        m_quantities.RemoveAt(index);
    }
}
