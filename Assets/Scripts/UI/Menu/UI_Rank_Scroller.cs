using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI.Extensions.EasingCore;

namespace UI
{
    public class Context : FancyScrollRectContext
    {
        public int uid = -1;
    }

    public class UI_Rank_Scroller : FancyScrollRect<RankData, Context>
    {
        [SerializeField] private Scroller   scroller;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private float      cellSize;

        protected override GameObject CellPrefab => cellPrefab;
        protected override float      CellSize   => cellSize;

        private void Awake()
        {
            Relayout();

            Context.uid = 10;
        }
        
        public void UpdateData(IList<RankData> items)
        {
            UpdateContents(items);
            scroller.SetTotalCount(items.Count);

            JumpTo(0);
        }

        public void ScrollTo(int index)
        {
            ScrollTo(index, 0.25f, Ease.InOutQuint);
        }

        public void JumpTo(int index)
        {
            JumpTo(index, 0.5f);
        }
    }
}