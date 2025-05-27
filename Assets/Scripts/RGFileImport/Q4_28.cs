// AI-generated, and i had no time to go through it yet
// might be broken somewhere
using System;
namespace RGFileImport
{
    public struct Q4_28
    {
        private int value;

        public Q4_28(int value)
        {
            this.value = value;
        }

        public Q4_28(float value)
        {
            this.value = (int)(value * 268435456);
        }

        public static Q4_28 FromInt(int value)
        {
            return new Q4_28(value << 28);
        }

        public static Q4_28 FromFloat(float value)
        {
            return new Q4_28(value);
        }

        public float ToFloat()
        {
            return (float)value / 268435456.0f;
        }

        public int ToInt()
        {
            return value >> 28;
        }

        public static Q4_28 operator +(Q4_28 a, Q4_28 b)
        {
            return new Q4_28(a.value + b.value);
        }

        public static Q4_28 operator -(Q4_28 a, Q4_28 b)
        {
            return new Q4_28(a.value - b.value);
        }

        public static Q4_28 operator *(Q4_28 a, Q4_28 b)
        {
            // Multiply the two Q4.28 numbers, then shift right by 28 to maintain the Q4.28 format
            return new Q4_28((int)(((long)a.value * b.value) >> 28));
        }

        public static Q4_28 operator /(Q4_28 a, Q4_28 b)
        {
            // Divide the two Q4.28 numbers, then shift left by 28 to maintain the Q4.28 format
            return new Q4_28((int)(((long)a.value << 28) / b.value));
        }

        public override string ToString()
        {
            return ToFloat().ToString();
        }
    }
}
