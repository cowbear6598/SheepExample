using System.Collections.Generic;
using System.Linq;
using Card;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts
{
    public class CardCheckLock : EditorWindow
    {
        [MenuItem("Card/SetLock")]
        private static void ShowWindow()
        {
            CardLockSetting[] cardLockSettings = FindObjectsOfType<CardLockSetting>();

            for (int i = 0; i < cardLockSettings.Length; i++)
            {
                cardLockSettings[i].unlockCount = 0;
            }

            for (int i = 0; i < cardLockSettings.Length; i++) // 幾張卡牌
            {
                List<CardView> lockCardViewList = new List<CardView>();

                Transform cardTrans = cardLockSettings[i].transform;

                float width    = 1.1f;
                float height   = 1.4f;
                float margin   = 0.2f;
                int   xCount   = 5;
                int   yCount   = 5;
                float xPadding = (width  - margin * 2) / xCount;
                float yPadding = (height - margin * 2) / yCount;
                float startX   = cardTrans.position.x - width  / 2 + margin;
                float startY   = cardTrans.position.y + height / 2 - margin;

                for (int x = 0; x < xCount + 1; x++)
                {
                    for (int y = 0; y < yCount + 1; y++)
                    {
                        Vector3 originPos = new Vector3(startX + xPadding * x, startY - yPadding * y, cardTrans.transform.position.z);

                        RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(originPos, Vector3.forward), 5, 1 << LayerMask.NameToLayer("Card"));

                        if (hit)
                        {
                            CardView cardView = hit.collider.GetComponent<CardView>();

                            if (lockCardViewList.Count == 0)
                            {
                                AddLock(lockCardViewList, cardView);
                            }
                            else
                            {
                                bool IsExist = lockCardViewList.Any(t => t == cardView);

                                if (!IsExist)
                                {
                                    AddLock(lockCardViewList, cardView);
                                }
                            }
                        }
                    }
                }

                cardLockSettings[i].lockCardViews = lockCardViewList.ToArray();
                
                EditorUtility.SetDirty(cardLockSettings[i]);
            }
        }

        private static void AddLock(List<CardView> lockCardViewList, CardView cardView)
        {
            lockCardViewList.Add(cardView);

            CardLockSetting lockCardLockSetting = cardView.GetComponent<CardLockSetting>();
            lockCardLockSetting.unlockCount++;
            EditorUtility.SetDirty(lockCardLockSetting);
        }
    }
}