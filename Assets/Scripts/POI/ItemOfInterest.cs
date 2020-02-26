using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOfInterest : MonoBehaviour
{
    public ItemType m_itemType;
    public int m_quantity;
    public string m_itemName;

    public void SetTag(ItemType tag)
    {
        m_itemType = tag;
    }

    public ItemType GetTag()
    {
        return m_itemType;
    }

    public void SetQuantity(int quantity)
    {
        m_quantity = quantity;
    }

    public int GetQuantity()
    {
        return m_quantity;
    }

    public void SetItemName(string name)
    {
        m_itemName = name;
    }

    public string GetItemName()
    {
        return m_itemName;
    }
}

public enum ItemType {CommonKey, SilverKey, StoneKey, JadeKey};
