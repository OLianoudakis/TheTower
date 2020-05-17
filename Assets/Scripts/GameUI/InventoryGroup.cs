using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player.Inventory;

namespace GameUI
{
    public class InventoryGroup : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_commonKeysGroup;
        [SerializeField]
        private Text m_commonKeysQuantity;
        [SerializeField]
        private CanvasGroup m_stoneKeysGroup;
        [SerializeField]
        private Text m_stoneKeysQuantity;
        [SerializeField]
        private CanvasGroup m_silverKeysGroup;
        [SerializeField]
        private Text m_silverKeysQuantity;
        [SerializeField]
        private CanvasGroup m_jadeKeysGroup;
        [SerializeField]
        private Text m_jadeKeysQuantity;
        [SerializeField]
        private CanvasGroup m_potionsGroup;
        [SerializeField]
        private Text m_potionsQuantity;
        [SerializeField]
        private float m_fadeLerpSpeed = 2.0f;

        private float m_fadeLerpInterval = 0.0f;

        public void KeyUI(ItemType itemType, int quantity)
        {
            StopAllCoroutines();
            bool fadeIn = quantity == 0 ? false : true;
            switch (itemType)
            {
                case ItemType.CommonKey:
                    m_commonKeysQuantity.text = quantity.ToString();
                    StartCoroutine(ShowKeyUICoroutine(fadeIn, m_commonKeysGroup));
                    break;
                case ItemType.StoneKey:
                    m_stoneKeysQuantity.text = quantity.ToString();
                    StartCoroutine(ShowKeyUICoroutine(fadeIn, m_stoneKeysGroup));
                    break;
                case ItemType.SilverKey:
                    m_silverKeysQuantity.text = quantity.ToString();
                    StartCoroutine(ShowKeyUICoroutine(fadeIn, m_silverKeysGroup));
                    break;
                case ItemType.JadeKey:
                    m_jadeKeysQuantity.text = quantity.ToString();
                    StartCoroutine(ShowKeyUICoroutine(fadeIn, m_jadeKeysGroup));
                    break;
                case ItemType.Potions:
                    m_potionsQuantity.text = quantity.ToString();
                    StartCoroutine(ShowKeyUICoroutine(fadeIn, m_potionsGroup));
                    break;
            }
        }

        private IEnumerator ShowKeyUICoroutine(bool fadeIn, CanvasGroup keyCanvas)
        {
            bool currentlyFading;
            if (fadeIn)
            {
                if (keyCanvas.alpha == 1.0f)
                {
                    currentlyFading = false;
                    yield return null;
                }
                else
                {
                    currentlyFading = true;
                    while (currentlyFading)
                    {
                        keyCanvas.alpha = Mathf.Lerp(0.0f, 1.0f, m_fadeLerpInterval);
                        m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                        yield return new WaitForEndOfFrame();
                        if (m_fadeLerpInterval > 1.0f)
                        {
                            keyCanvas.alpha = 1.0f;
                            m_fadeLerpInterval = 0.0f;
                            currentlyFading = false;
                            yield return null;
                        }
                    }
                }
            }
            else
            {
                currentlyFading = true;
                while (currentlyFading)
                {
                    keyCanvas.alpha = Mathf.Lerp(1.0f, 0.0f, m_fadeLerpInterval);
                    m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                    if (m_fadeLerpInterval > 1.0f)
                    {
                        keyCanvas.alpha = 0.0f;
                        m_fadeLerpInterval = 0.0f;
                        currentlyFading = false;
                        yield return null;
                    }
                }
            }
        }
    }

}
