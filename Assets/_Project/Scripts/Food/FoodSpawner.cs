using System.Collections.Generic;
using UnityEngine;
using TheSuperShell.Utils;

namespace TheSuperShell.food
{
    public class FoodSpawner : MonoBehaviour
    {
        [SerializeField] private LayerMask microbeMask;
        [SerializeField] private GameObject foodObject;
        [Range(0f, 500f)]
        [SerializeField] private float AverageFoodEnergy;
        [Range(0f, 500f)]
        [SerializeField] private float energyDensity;
        [Range(0f, 100f)]
        [SerializeField] private float radius;
        [Range(10, 2000)]
        [SerializeField] private int maxFood;
        [Range(0, 50)]
        [SerializeField] private int foodPerSecond;

        private List<GenericFood> foodList = new();
        private float spawnQueue = 0;

        private void Start()
        {
            for (int i = 0; i < Mathf.RoundToInt(maxFood * 0.75f); i++)
                Spawn();
        }

        private int Spawn()
        {
            Vector3 pos = RandomPointInCircle() + transform.position;
            float energy = Mathf.Clamp(Functions.NormalDistribution(AverageFoodEnergy, AverageFoodEnergy * 0.25f), 10, 1000);
            float size = Mathf.Pow(energy / energyDensity, 0.5f);
            if (Physics2D.OverlapCircleAll(pos, size, microbeMask).Length != 0)
                return 0;
            GameObject go = Instantiate(foodObject, pos, Quaternion.identity, transform);
            GenericFood food = go.GetComponent<GenericFood>();
            food.Spawn(energy, energyDensity);
            food.Destroyed += OnDestroyed;
            foodList.Add(food);
            return 1;
        }

        private Vector3 RandomPointInCircle()
        {
            float x = 0;
            float y = 0;
            for (int i = 0; i < 3; i++)
            {
                float randomRadius = radius * (1 + Random.value * 0.1f);
                x = (2 * Random.value - 1) * randomRadius;
                y = (2 * Random.value - 1) * randomRadius;
                if (x * x + y * y <= randomRadius * randomRadius)
                    return new Vector3(x, y);
            }
            return new Vector3(x, y);
        }

        private void OnDestroyed(object sender, System.EventArgs e)
        {
            GenericFood food = (GenericFood)sender;
            foodList.Remove(food);
            food.Destroyed -= OnDestroyed;
        }

        private void Update()
        {
            if (foodList.Count < maxFood)
            {
                spawnQueue += Time.deltaTime * foodPerSecond;
                int amountToSpawn = Mathf.FloorToInt(spawnQueue);
                int spawned = 0;
                for (int i = 0; i < amountToSpawn; i++)
                    spawned += Spawn();
                spawnQueue -= spawned;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 0.8f, 0, 0.3f);
            Gizmos.DrawSphere(Vector3.zero + transform.position, radius);
        }
    }
}
