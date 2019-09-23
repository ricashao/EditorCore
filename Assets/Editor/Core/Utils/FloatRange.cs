using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace XCFramework.Editor
{
    [Serializable]
    public class FloatRange
    {
        private static Regex parser = new Regex("\\((?<min>\\d*\\.?\\d+)\\,(?<max>\\d*\\.?\\d+)\\)");
        private static string format = "({0},{1})";
        public float min;
        public float max;

        public float interval
        {
            get
            {
                return Mathf.Abs(this.max - this.min);
            }
        }

        public FloatRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public bool IsInRange(float value)
        {
            if ((double) value >= (double) Mathf.Min(this.min, this.max))
                return (double) value <= (double) Mathf.Max(this.min, this.max);
            return false;
        }

        public float Lerp(float t)
        {
            return Mathf.Lerp(this.min, this.max, t);
        }

        internal FloatRange GetClone()
        {
            return (FloatRange) this.MemberwiseClone();
        }

        public static FloatRange Parse(string raw)
        {
            Match match = FloatRange.parser.Match(raw);
            if (match.Success)
                return new FloatRange(float.Parse(match.Groups["min"].Value), float.Parse(match.Groups["max"].Value));
            throw new FormatException("Can't to parse \"" + raw + "\" to FloatRange format. It must have next format: (float,float)");
        }

        public override string ToString()
        {
            return string.Format(FloatRange.format, (object) this.min, (object) this.max);
        }

        public static implicit operator FloatRange(float value)
        {
            return new FloatRange(value, value);
        }
    }
}