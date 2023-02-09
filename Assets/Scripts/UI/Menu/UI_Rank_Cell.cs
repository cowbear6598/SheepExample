using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
namespace UI
{
    public class UI_Rank_Cell : FancyScrollRectCell<RankData, Context>
    {
        [SerializeField] private Image           bgImg;
        [SerializeField] private Color[]         bgColor;
        [SerializeField] private Color[]         colors;
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Image           levelImg;
        [SerializeField] private Sprite[]        levelSprites;

        public override void UpdateContent(RankData rankData)
        {
            bgImg.color = bgColor[rankData.uid == Context.uid ? 1 : 0];
            
            Color color = Index switch {
                0 => colors[0],
                1 => colors[1],
                2 => colors[2],
                _ => colors[3]
            };

            rankText.color = nameText.color = timeText.color = scoreText.color = color;

            rankText.text  = (Index + 1).ToString();
            nameText.text  = rankData.name;
            timeText.text  = (rankData.elapsed_time < TimeSpan.MaxValue.TotalSeconds) ? TimeSpan.FromSeconds(rankData.elapsed_time).ToString(@"mm\:ss") : "-";
            scoreText.text = rankData.score.ToString();

            levelImg.sprite = levelSprites[(Index is > 15 or < 0) ? 15 : Index];
        }
    }
}