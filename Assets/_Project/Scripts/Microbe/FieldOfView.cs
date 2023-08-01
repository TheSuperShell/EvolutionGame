using System.Collections;
using UnityEngine;

namespace TheSuperShell.Microbes
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] private float radius;
        [Range(0f, 360f)]
        [SerializeField] private float angle;
        [SerializeField] private LayerMask foodMask;
        [SerializeField] private LayerMask microbeMask;
        [Range(0f, 5f)]
        [SerializeField] private float delay = 1f;

        private float foodDistance;
        private float foodAngle;
        private int foodAmount;
        private float microbeDistnace;
        private float microbeAngle;
        private int microbeAmount;
        private Color microbeColor;

        public float ClosestFoodDistance { get => foodDistance; }
        public float ClosestFoodAngle { get => foodAngle; }
        public int AmountOfFoodSeen { get => foodAmount; }
        public float ClosestMicrobeDistance { get => microbeDistnace; }
        public float ClosestMicrobeAngle { get => microbeAngle; }
        public Color ClosestMicrobeColor { get => microbeColor; }
        public int AmountOfMicrobesSeen { get => microbeAmount; }

        public float Radius { get => radius; set => radius = value; }
        public float Angle { get => angle; }

        public void StartSensing()
        {
            StopCoroutine(FindTargetsWithDelay());
            StartCoroutine(FindTargetsWithDelay());
        }

        public void StopSensing()
        {
            StopCoroutine(FindTargetsWithDelay());
        }

        private IEnumerator FindTargetsWithDelay()
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindClosest(foodMask, out foodAmount, out foodDistance, out foodAngle);
                Collider2D microbeCollider;
                FindClosest(microbeMask, out microbeAmount, out microbeDistnace, out microbeAngle, out microbeCollider);
                if (microbeCollider != null)
                    microbeColor = microbeCollider.GetComponent<Microbe>().MicrobeColor;
                else
                    microbeColor = Color.black;
            }
        }

        private void FindClosest(LayerMask mask, out int amount, out float distance, out float angle)
        {
            FindClosest(mask, out amount, out distance, out angle, out _);
        }

        private void FindClosest(LayerMask mask, out int amount, out float distance, out float angle, out Collider2D collider)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, mask);
            amount = 0;
            distance = radius;
            angle = 180;
            collider = null;
            for (int i = 0; i < colliders.Length; i++)
            {
                Transform target = colliders[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                float AngleToTarget = Vector3.SignedAngle(transform.right, dirToTarget, new Vector3(0, 0, 1));
                if (Mathf.Abs(AngleToTarget) < this.angle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);
                    if (dstToTarget != 0)
                    {
                        amount++;
                        if (dstToTarget < distance)
                        {
                            angle = AngleToTarget;
                            distance = dstToTarget;
                            collider = colliders[i];
                        }
                    }
                }
            }
        }

        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
                angleInDegrees += transform.eulerAngles.z;
            return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
        }
    }
}
