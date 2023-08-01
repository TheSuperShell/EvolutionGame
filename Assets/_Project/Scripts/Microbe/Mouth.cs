using UnityEngine;
using TheSuperShell.food;
using System.Collections.Generic;

namespace TheSuperShell.Microbes
{
    public class Mouth : MonoBehaviour
    {
        private List<GenericFood> foodNearMouth = new();
        private List<Microbe> microbesNearMouth = new();

        public GenericFood GetFoodNearMouth()
        {
            if (foodNearMouth.Count == 0)
                return null;
            return foodNearMouth[0];
        }

        public Microbe GetMicrobeNearMouth()
        {
            if (microbesNearMouth.Count == 0)
                return null;
            return microbesNearMouth[0];
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Food"))
                foodNearMouth.Add(collision.GetComponent<GenericFood>());
            if (collision.gameObject.layer == LayerMask.NameToLayer("Microbe"))
                microbesNearMouth.Add(collision.GetComponent<Microbe>());
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Food"))
                foodNearMouth.Remove(collision.GetComponent<GenericFood>());
            if (collision.gameObject.layer == LayerMask.NameToLayer("Microbe"))
                microbesNearMouth.Remove(collision.GetComponent<Microbe>());
        }


    }
}
