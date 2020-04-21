using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item")]
    public class Item : ScriptableObject
    {
        public ItemType m_itemType;
        public string m_itemName;
        public int m_quantity;
    }

    public enum ItemType
    {
        None,
        AutomaticPass,
        CommonKey,
        SilverKey,
        StoneKey,
        JadeKey,
        Potions
    };
}
