using UnityEngine;
using TMPro;
using TSSGraph;
using System.Collections.Generic;
using TheSuperShell.Microbes;

namespace TheSuperShell.UI
{
    public sealed class AdditionalMaturityWindow : AdditionalInfoWindow
    {
        [SerializeField] private Color yesColor;
        [SerializeField] private Color noColor;
        [SerializeField] private TextMeshProUGUI canLayText;
        [SerializeField] private WindowGraph plt;
        [SerializeField] private Color lineColor;
        [SerializeField] private Gradient markerGradient;

        protected override void UpdateStats()
        {
            bool canLay = currentMicrobe.Maturity >= 1;
            canLayText.text = (canLay) ? "Yes" : "No";
            canLayText.color = (canLay) ? yesColor : noColor;

            Color markerColor = markerGradient.Evaluate(currentMicrobe.Maturity);
            plt.DrawMarker(new Vector2(currentMicrobe.Maturity, currentMicrobe.GrowthFunction(currentMicrobe.Maturity)), markerColor, 20);
        }

        public void DrawGrowthCurve(Microbe microbe)
        {
            plt.Refresh();
            float dx = 0.1f;
            int N = Mathf.FloorToInt(4 / dx);
            List<float> x = new();
            List<float> y = new();
            for (int i = 0; i < N; i++)
            {
                x.Add(i * dx);
                y.Add(microbe.GrowthFunction(i * dx));
            }
            plt.SetXLimits(0, 4f);
            plt.SetXTicks(4);
            plt.SetYLimits(0, 3);
            plt.SetYTicks(3);
            plt.SetLineColor(lineColor);
            plt.Grid(true);
            plt.Plot(x, y, false);
        }
    }
}
