using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Inventory
{
    // TODO we probably wont need this one, but lets see
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

    //public struct Item
    //{
    //    public string m_itemName;
    //    public int m_quantity;
    //}

    
}
