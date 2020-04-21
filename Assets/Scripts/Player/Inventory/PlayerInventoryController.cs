using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUI;
using GameSounds;

namespace Player.Inventory
{
    public class PlayerInventoryController : MonoBehaviour
    {
        Dictionary<ItemType, Item> m_inventory = new Dictionary<ItemType, Item>();
        private InventoryGroup m_inventoryGroup;
        private PlayerSFXAudioController m_inventorySounds;

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
            m_inventoryGroup.KeyUI(item.m_itemType, m_inventory[item.m_itemType].m_quantity);
            m_inventorySounds.PlayKeySound();
        }

        public int RemoveItem(ItemType requestedItemType, int requestedQuantity)
        {
            if (m_inventory.ContainsKey(requestedItemType))
            {
                if (m_inventory[requestedItemType].m_quantity >= requestedQuantity)
                {
                    m_inventory[requestedItemType].m_quantity -= requestedQuantity;
                    m_inventoryGroup.KeyUI(requestedItemType, m_inventory[requestedItemType].m_quantity);
                    m_inventorySounds.PlayUnlockSound();
                    return 0;
                }
                int requestedQuantityRemaining = requestedQuantity - m_inventory[requestedItemType].m_quantity;
                m_inventory[requestedItemType].m_quantity = 0;
                m_inventoryGroup.KeyUI(requestedItemType, m_inventory[requestedItemType].m_quantity);
                return requestedQuantityRemaining;
            }
            return requestedQuantity;
        }

        public Item SearchForItem(ItemType requestedItemType, int requestedQuantity)
        {
            if (m_inventory.ContainsKey(requestedItemType) && m_inventory[requestedItemType].m_quantity >= requestedQuantity)
            {
                return m_inventory[requestedItemType];
            }
            return null;
        }

        private void Awake()
        {
            m_inventoryGroup = FindObjectOfType(typeof(InventoryGroup)) as InventoryGroup;
            m_inventorySounds= FindObjectOfType(typeof(PlayerSFXAudioController)) as PlayerSFXAudioController;
        }
    }
}
