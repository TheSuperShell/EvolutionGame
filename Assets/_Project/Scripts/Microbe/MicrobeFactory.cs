using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;
using TheSuperShell.Eggs;
using TheSuperShell.UI;
using CodeMonkey.Utils;
using UnityEngine.Events;
using TheSuperShell.Microbes.Params;

namespace TheSuperShell.Microbes
{
    public class MicrobeFactory : MonoBehaviour
    {
        public UnityEvent<List<float>, List<float>, List<MicrobeGenome>> DistributionChangeEvent { get; private set; } = new();

        [SerializeField] private GameObject microbe;
        [SerializeField] private GameObject egg;
        [Range(100, 2000)]
        [SerializeField] private int maxMicrobeAmount;
        [Header("UI")]
        [SerializeField] private MainMicrobeUIWindow microbeUI;
        [Header("Auto Spawner")]
        [SerializeField] private bool spawnRandom;
        [Range(0f, 100f)]
        [SerializeField] private float randomRadius;
        [Range(1, 100)]
        [SerializeField] private int randomAmount;

        private List<Microbe> microbeList = new();
        private List<float> timeList = new();
        private List<float> microbeAmountList = new();
        private float timer;

        public List<float> TimeList { get => timeList; }
        public List<float> MicrobeAmountList { get => microbeAmountList; }

        private void Awake()
        {
            timer = 0;
            foreach (Microbe microbe in GetComponentsInChildren<Microbe>())
            {
                AddNewMicrobe(microbe);
            }
        }

        private void Update()
        {
            if (spawnRandom && microbeList.Count < randomAmount)
                RandomMicrobes();
            timer += Time.deltaTime;
        }

        private void RandomMicrobes()
        {
            for (int i = 0; i < randomAmount - microbeList.Count; i++)
            {
                float r = Range(0, randomRadius);
                float phi = Range(0, 360);
                Vector3 position = new Vector3(Mathf.Cos(phi * Mathf.Deg2Rad), Mathf.Sin(phi * Mathf.Deg2Rad), 0) * r;
                GameObject go = Instantiate(microbe, position, Quaternion.Euler(new Vector3(0, 0, value * 360)), transform);
                Microbe newMicrobe = go.GetComponent<Microbe>();
                newMicrobe.GenerateNew();
                AddNewMicrobe(newMicrobe);
            }
        }

        private void AddNewMicrobe(Microbe microbe)
        {
            if (microbeList.Count == maxMicrobeAmount)
                throw new InvalidOperationException("Too many microbes!");
            microbeList.Add(microbe);
            microbe.GetComponent<Button_Sprite>().ClickFunc = () => {
                microbeUI.FollowMicrobe(microbe);
            };
            microbe.DeathEvent += OnDeath;
            microbe.BirthEvent += OnBirthEvent;
            UpdateData();
        }

        private void AddNewEgg(Egg egg)
        {
            if (microbeList.Count == maxMicrobeAmount)
                throw new InvalidOperationException("Too many microbes!");
            egg.HatchEvent += OnHatchEvent;
        }

        private void OnHatchEvent(object sender, Egg.HatchEventArgs e)
        {
            if (microbeList.Count < maxMicrobeAmount)
            {
                GameObject microbeObj = Instantiate(microbe, e.position, Quaternion.Euler(new Vector3(0, 0, value * 360)), transform);
                Microbe newMicrobe = microbeObj.GetComponent<Microbe>();
                newMicrobe.Inherit(e.brain, e.genes, e.energy);
                AddNewMicrobe(newMicrobe);
            }
        }

        private void OnBirthEvent(object sender, Microbe.BirthEventArgs e)
        {
            if (microbeList.Count < maxMicrobeAmount)
            {
                GameObject eggObj = Instantiate(egg, e.position, Quaternion.Euler(new Vector3(0, 0, value * 360)), transform);
                Egg newEgg = eggObj.GetComponent<Egg>();
                newEgg.SetUp(e.brainGenome, e.genome, e.energyToPass, e.eggHatchTime);
                AddNewEgg(newEgg);
            }
        }

        private void OnDeath(object sender, EventArgs e)
        {
            microbeList.Remove((Microbe)sender);
            UpdateData();
        }

        public void InvokeDistributionChangeEvent()
        {
            List<MicrobeGenome> genomeList = new();
            foreach (Microbe microbe in microbeList)
                genomeList.Add(microbe.Genome);
            DistributionChangeEvent.Invoke(timeList, microbeAmountList, genomeList);
        }

        private void UpdateData()
        {
            timeList.Add(timer);
            microbeAmountList.Add(microbeList.Count);
            InvokeDistributionChangeEvent();
        }
    }
}
