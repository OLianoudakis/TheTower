using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOfInterest : MonoBehaviour
{
    public ItemType m_itemType;
    public string m_itemName;

    public void SetTag(ItemType tag)
    {
        m_itemType = tag;
    }

    public ItemType GetTag()
    {
        return m_itemType;
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

public enum ItemType 
{
    None, 
    CommonKey, 
    SilverKey, 
    StoneKey, 
    JadeKey
};
