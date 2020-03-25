using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Inventory
{
    public class PlayerInventoryController : MonoBehaviour
    {
        Dictionary<ItemType, Item> m_inventory = new Dictionary<ItemType, Item>();

        public void AddItem(Item item, int quantity)
        {
            if (m_inventory.ContainsKey(item.m_itemType))
            {
                m_inventory[item.m_itemType].m_quantity += quantity;
            }
            else
            {
                Item newItem = ScriptableObject.CreateInstance(typeof(Item)) as Item;
                newItem.m_itemName = item.m_itemName;
                newItem.m_itemType = item.m_itemType;
                newItem.m_quantity = quantity;
                m_inventory[item.m_itemType] = newItem;
            }
        }

        public int RemoveItem(ItemType requestedItemType, int requestedQuantity)
        {
            if (m_inventory.ContainsKey(requestedItemType))
            {
                if (m_inventory[requestedItemType].m_quantity >= requestedQuantity)
                {
                    m_inventory[requestedItemType].m_quantity -= requestedQuantity;
                    return 0;
                }
                int requestedQuantityRemaining = requestedQuantity - m_inventory[requestedItemType].m_quantity;
                m_inventory[requestedItemType].m_quantity = 0;
                return requestedQuantityRemaining;
            }
            return requestedQuantity;
        }
    }
}
