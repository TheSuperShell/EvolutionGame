using UnityEngine;
using TheSuperShell.Microbes;
using TSSGraph;
using System.Collections.Generic;
using TheSuperShell.Microbes.Params;
using System.Linq;
using TMPro;
using UnityEngine.Events;

namespace TheSuperShell.UI
{
    public class DistributionsWindow : MonoBehaviour
    {
        [SerializeField] private MicrobeFactory microbeFactory;

        private TextMeshProUGUI amountText;
        private WindowGraph plt;
        private float timer;
        private UnityAction<List<float>, List<float>, List<MicrobeGenome>> currentGraphAction;

        private void Awake()
        {
            plt = transform.Find("graph").GetComponent<WindowGraph>();
            amountText = transform.Find("value").GetComponent<TextMeshProUGUI>();
            plt.SetXTicks(4);
            plt.SetYTicks(5);
            plt.Grid(true);
            plt.SetYFormat("F0");
            plt.SetXFormat("F2");
            currentGraphAction = PlotAmount;
            microbeFactory.DistributionChangeEvent.AddListener(currentGraphAction);
        }

        private void Update()
        {
            if (timer > 0)
                timer -= Time.deltaTime;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            timer = 0;
            microbeFactory.DistributionChangeEvent.AddListener(currentGraphAction);
            microbeFactory.InvokeDistributionChangeEvent();
            LeanTween.moveLocalY(gameObject, 290, 0.1f);
        }

        public void Hide()
        {
            if (currentGraphAction != null)
                microbeFactory.DistributionChangeEvent.RemoveListener(currentGraphAction);
            LeanTween.moveLocalY(gameObject, 800, 0.1f).setOnComplete(() => gameObject.SetActive(false));
        }

        public void SwitchGraph(int i)
        {
            if (currentGraphAction != null)
                microbeFactory.DistributionChangeEvent.RemoveListener(currentGraphAction);
            timer = 0;
            plt.SetXFormat("F2");
            switch (i)
            {
                case 0:
                    currentGraphAction = PlotAmount;
                    break;
                case 1:
                    currentGraphAction = PlotGeneration;
                    break;
                case 2:
                    currentGraphAction = PlotSize;
                    break;
                case 3:
                    currentGraphAction = PlotForce;
                    break;
            }
            microbeFactory.DistributionChangeEvent.AddListener(currentGraphAction);
            microbeFactory.InvokeDistributionChangeEvent();
        }

        private void PlotGeneration(List<float> times, List<float> amounts, List<MicrobeGenome> genomes)
        {
            plt.SetXFormat("F0");
            List<float> gens = new();
            foreach (MicrobeGenome genome in genomes)
                gens.Add(genome.generation);
            int bins = Mathf.RoundToInt(gens.Max() - gens.Min()) + 1;
            PlotHist(gens, bins);
            amountText.text = amounts[^1].ToString("F0");
        }

        private void PlotSize(List<float> times, List<float> amounts, List<MicrobeGenome> genomes)
        {
            List<float> sizes = new();
            foreach (MicrobeGenome genome in genomes)
                sizes.Add(genome.linearSize);
            PlotHist(sizes);
            amountText.text = amounts[^1].ToString("F0");
        }

        private void PlotForce(List<float> times, List<float> amounts, List<MicrobeGenome> genomes)
        {
            List<float> forces = new();
            foreach (MicrobeGenome genome in genomes)
                forces.Add(genome.movementForcePercent);
            PlotHist(forces);
            amountText.text = amounts[^1].ToString("F0");
        }

        private void PlotHist(List<float> data, int bins = 8)
        {
            plt.Refresh();
            plt.SetXLimits();
            plt.Hist(data, bins);
        }

        private void PlotAmount(List<float> times, List<float> amounts, List<MicrobeGenome> genomes)
        {
            if (timer > 0) return;
            plt.SetXFormat("F0");
            timer = 5;
            plt.Refresh();
            plt.SetXLimits(0, times.Max() * 1.1f);
            plt.SetYLimits(0, amounts.Max() * 1.1f);
            plt.Plot(times, amounts, false);
            amountText.text = amounts[^1].ToString("F0");
        }
    }
}
