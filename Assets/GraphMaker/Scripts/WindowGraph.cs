using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using CodeMonkey.Utils;
using TMPro;

namespace TSSGraph
{
    public class WindowGraph : MonoBehaviour
    {
        [SerializeField] private RectTransform graphContainer;
        [Range(0, 0.5f)]
        [SerializeField] private float emptyPartX;
        [Range(0, 0.5f)]
        [SerializeField] private float emptyPartY;
        [Header("Customize look")]
        [SerializeField] private Sprite dotSprite;
        [SerializeField] private TMP_FontAsset textFont;

        private List<RectTransform> childrenRectTransforms = new();
        private float xMin=0, xMax=10, yMin=0, yMax=10;
        private bool setXRange=true, setYRange=true;
        private bool drawGrid = false;
        private int xTicks=10, yTicks=10;
        private string xFormat="F1", yFormat="F1";
        private Color dotColor = Color.white, lineColor = Color.white, histColor = Color.white;
        private Color axisColor = new Color(1, 1, 1, 0.6f);

        private float graphWidth, graphHeight;
        private bool drawDots = true;
        private bool drawLines = true;
        private RectTransform marker;

        private void Awake()
        {
            graphWidth = graphContainer.sizeDelta.x;
            graphHeight = graphContainer.sizeDelta.y;
        }

        public void SetXLimits() => setXRange = true;
        public void SetXLimits(float min, float max)
        {
            xMin = min;
            xMax = max;
            setXRange = false;
        }
        public void SetYLimits() => setYRange = true;
        public void SetYLimits(float min, float max)
        {
            yMax = max;
            yMin = min;
            setYRange = false;
        }
        public void SetXTicks(int amount) => xTicks = amount;
        public void SetYTicks(int amount) => yTicks = amount;
        public void SetLineColor(Color color) => lineColor = color;
        public void SetXFormat(string format) => xFormat = format;
        public void SetYFormat(string format) => yFormat = format;
        public void Grid(bool drawGrid) => this.drawGrid = drawGrid;

        public void Scatter(List<float> x, List<float> y)
        {
            drawDots = true;
            drawLines = false;
            BasePlot(x, y);
        }

        public void Plot(List<float> x, List<float> y, bool drawDots=true)
        {
            this.drawDots = drawDots;
            drawLines = true;
            BasePlot(x, y);
        }

        private void DrawDot(Vector2 anchoredPosition, Color color, float size = 11f)
        {
            GameObject gameObject = new GameObject("circle", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            Image image = gameObject.GetComponent<Image>();
            image.sprite = dotSprite;
            image.color = color;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = Vector2.one * size;
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector3(0, 0);
            childrenRectTransforms.Add(rectTransform);
        }

        private void DrawLine(Vector2 aPos, Vector2 bPos, Color color, float thickness = 3f)
        {
            GameObject gameObject = new GameObject("line", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = color;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 dir = (bPos - aPos).normalized;
            float dist = Vector2.Distance(aPos, bPos);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(dist, thickness);
            rectTransform.anchoredPosition = aPos + dir * dist * 0.5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
            childrenRectTransforms.Add(rectTransform);
        }

        private void DrawText(Vector2 pos, Vector2 size, string text)
        {
            GameObject gameObject = new GameObject("text", typeof(TextMeshProUGUI));
            gameObject.transform.SetParent(graphContainer, false);
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.sizeDelta = size;
            rectTransform.anchoredPosition = pos;
            TextMeshProUGUI textMesh = gameObject.GetComponent<TextMeshProUGUI>();
            textMesh.enableAutoSizing = true;
            textMesh.fontSizeMax = 100;
            textMesh.fontSizeMin = 5;
            textMesh.font = textFont;
            textMesh.text = text;
            textMesh.alignment = TextAlignmentOptions.Midline;
            childrenRectTransforms.Add(rectTransform);
        }

        private Vector2 DataToGraphCoordinates(Vector2 pos)
        {
            float xPos = emptyPartX * graphWidth + (pos.x - xMin) / (xMax - xMin) * graphWidth * (0.95f - emptyPartX);
            float yPos = emptyPartY * graphHeight + (pos.y - yMin) / (yMax - yMin) * graphHeight * (0.95f - emptyPartY);
            return new Vector2(xPos, yPos);
        }

        private void DrawAxies()
        {
            DrawLine(new Vector2(graphWidth * emptyPartX, graphHeight * emptyPartY), new Vector2(graphWidth * emptyPartX, graphHeight * 0.95f), Color.white);
            DrawLine(new Vector2(graphWidth * emptyPartX, graphHeight * emptyPartY), new Vector2(graphWidth * 0.95f, graphHeight * emptyPartY), Color.white);

            float xTick = (xMax - xMin) / xTicks;
            float xTickPos = graphWidth * (0.95f - emptyPartX) / xTicks;
            float yTick = (yMax - yMin) / yTicks;
            float yTickPos = graphHeight * (0.95f - emptyPartY) / yTicks;
            float xTickLength = (drawGrid) ? graphWidth * (0.95f - emptyPartX) : 8;
            float yTickLength = (drawGrid) ? graphHeight * (0.95f - emptyPartY) : 8;

            for (int i = 0; i <= xTicks; i++)
            {
                float xPos = graphWidth * emptyPartX + i * xTickPos;
                DrawText(new Vector2(xPos, emptyPartY * 0.5f * graphHeight), new Vector2(xTickPos * 0.9f, graphHeight * emptyPartY), (xMin + xTick * i).ToString(xFormat));
                if (i == 0) continue;
                DrawLine(new Vector2(xPos, emptyPartY * graphHeight), new Vector2(xPos, emptyPartY * graphHeight + yTickLength), axisColor, 2);
            }
            for (int j = 0; j <= yTicks; j++)
            {
                float yPos = graphHeight * emptyPartY + j * yTickPos;
                DrawText(new Vector2(emptyPartX * 0.5f * graphWidth, yPos), new Vector2(graphWidth * emptyPartX, graphHeight * emptyPartY), (yMin + yTick * j).ToString(yFormat));
                if (j == 0) continue;
                DrawLine(new Vector2(emptyPartX * graphWidth, yPos), new Vector2(emptyPartX * graphWidth + xTickLength, yPos), axisColor, 2);
            }
        }

        private void ChangeChildrenOrder()
        {
            int index = 1;
            for (int i = 0; i < childrenRectTransforms.Count; i++)
            {
                if (childrenRectTransforms[i].name == "line")
                {
                    childrenRectTransforms[i].SetSiblingIndex(index);
                    index++;
                }
            }
        }

        public void Hist(List<float> data, int n_bins = 11)
        {
            graphWidth = graphContainer.sizeDelta.x;
            graphHeight = graphContainer.sizeDelta.y;
            int[] bins = new int[n_bins];
            float maxValue = data.Max();
            float minValue = data.Min();
            float delta = (maxValue - minValue) / (n_bins-1);
            for (int i = 0; i < data.Count; i++)
            {
                for (int n = 0; n < n_bins; n++)
                {
                    if (data[i] <= minValue + (n + 0.5f) * delta)
                    {
                        bins[n]++;
                        break;
                    }
                }
            }
            yMin = 0;
            yMax = Mathf.Ceil(bins.Max() * 1.2f / xTicks) * xTicks;
            if (setXRange)
            {
                xMin = minValue - delta;
                xMax = maxValue + delta;
            }
            xTicks = n_bins + 1;
            DrawAxies();

            for (int i = 0; i < n_bins; i++)
            {
                Vector2 pos = DataToGraphCoordinates(new Vector2(minValue + i * delta, bins[i]));
                if (pos.x <= emptyPartX * graphWidth || pos.y >= graphWidth * (0.95f - emptyPartX)) continue;
                DrawLine(new Vector2(pos.x, emptyPartY * graphHeight), pos, histColor, delta * graphWidth * (0.95f - emptyPartX) / (xMax - xMin));
            }
            ChangeChildrenOrder();
        }

        private void BasePlot(List<float> x, List<float> y)
        {
            if (x.Count != y.Count)
                throw new System.InvalidOperationException("Size of x != size of y!");
            graphWidth = graphContainer.sizeDelta.x;
            graphHeight = graphContainer.sizeDelta.y;

            if (setXRange)
            {
                xMax = x.Max();
                xMin = x.Min();
                if (xMax - xMin == 0)
                {
                    xMax += 1e-5f;
                    xMin -= 1e-5f;
                }
            }

            if (setYRange)
            {
                yMax = y.Max();
                yMin = y.Min();
                if (yMax - yMin == 0)
                {
                    yMax += 1e-5f;
                    yMin -= 1e-5f;
                }
            }

            DrawAxies();
            Vector2 previousPointPos = Vector2.zero;
            
            for (int i = 0; i < y.Count; i++)
            {
                Vector2 pos = DataToGraphCoordinates(new Vector2(x[i], y[i]));
                if (pos.x < 0 || pos.x > graphWidth || pos.y < 0 || pos.y > graphHeight)
                    continue;
                if (drawDots) DrawDot(pos, dotColor);
                if (drawLines && previousPointPos != Vector2.zero)
                    DrawLine(previousPointPos, pos, lineColor);
                
                previousPointPos = pos;
            }
            ChangeChildrenOrder();
        }

        public void DrawMarker(Vector2 pos, Color color, float size)
        {
            if (marker == null)
            {
                GameObject gameObject = new("marker", typeof(Image));
                gameObject.transform.SetParent(graphContainer, false);
                gameObject.GetComponent<Image>().sprite = dotSprite;
                RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector3(0, 0);
                marker = rectTransform;
            }
            marker.anchoredPosition = DataToGraphCoordinates(pos);
            marker.sizeDelta = Vector2.one * size;
            marker.GetComponent<Image>().color = color;
        }

        public void HideMarker()
        {
            Destroy(marker);
            marker = null;
        }

        public void Refresh()
        {
            foreach (RectTransform rectTransform in childrenRectTransforms)
            {
                Destroy(rectTransform.gameObject);
            }
            childrenRectTransforms = new();
        }
    }
}
