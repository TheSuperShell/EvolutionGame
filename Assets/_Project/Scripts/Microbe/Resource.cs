using System;
using UnityEngine;

namespace TheSuperShell.Microbes
{
    public class Resource
    {
        public event EventHandler ValueChangeEvent;

        private float maxValue;
        private float value;

        public float Value { get => value; }

        public float MaxValue { get => maxValue; }

        public bool IsBelowZero { get => value < 0; }

        public Resource()
        {
            maxValue = 1;
            value = 1;
        }

        public float GetShare() => value / maxValue;

        public void SetMaxAmount(float amount)
        {
            float difference = amount - maxValue;
            maxValue = amount;
            value += difference;
            value = Mathf.Clamp(value, 0, maxValue);
        }

        public void SetMaxAmountWithoutValueChange(float amount)
        {
            maxValue = amount;
            value = Mathf.Clamp(value, 0, maxValue);
        }

        public void SetMaxAmount(float amount, float currentValue)
        {
            value = currentValue;
            SetMaxAmount(amount);
        }

        public float AddToResource(float amount)
        {
            CheckIfValueIsBelowZero(amount);
            value += amount;
            return CheckExtra();
        }

        public float ConsumeResource(float amount)
        {
            CheckIfValueIsBelowZero(amount);
            value -= amount;
            ValueChangeEvent?.Invoke(this, EventArgs.Empty);
            return (value < 0) ? Mathf.Abs(value) : 0;
        }

        public float SetResource(float value)
        {
            this.value = value;
            return (this.value >= 0) ? CheckExtra() : Mathf.Abs(this.value);
        }

        private float CheckExtra()
        {
            float extra = (value > maxValue) ? value - maxValue : 0;
            value -= extra;
            ValueChangeEvent?.Invoke(this, EventArgs.Empty);
            return extra;
        }

        private void CheckIfValueIsBelowZero(float value)
        {
            if (value < 0)
                throw new System.InvalidOperationException("Can not accept values lower than 0");
        }
    }
}
