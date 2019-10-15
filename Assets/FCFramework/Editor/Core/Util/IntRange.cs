using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace XCFramework.Editor
{
    [Serializable]
  public class IntRange : IEnumerable<int>, IEnumerable
  {
    private static Regex parser = new Regex("\\((?<min>\\d+)\\,(?<max>\\d+)\\)");
    private static string format = "({0},{1})";
    public int min;
    public int max;

    public int interval
    {
      get
      {
        return Mathf.Abs(this.max - this.min);
      }
    }

    public int Max
    {
      get
      {
        return Mathf.Max(this.min, this.max);
      }
    }

    public int Min
    {
      get
      {
        return Mathf.Min(this.min, this.max);
      }
    }

    public int count
    {
      get
      {
        return Mathf.Abs(this.max - this.min) + 1;
      }
    }

    public IntRange(int min, int max)
    {
      this.min = min;
      this.max = max;
    }

    public bool IsInRange(int value)
    {
      if (value >= this.Min)
        return value <= this.Max;
      return false;
    }

    public int Lerp(float t)
    {
      return Mathf.RoundToInt(Mathf.Lerp((float) this.min, (float) this.max, t));
    }

    internal IntRange GetClone()
    {
      return (IntRange) this.MemberwiseClone();
    }

    IEnumerator<int> IEnumerable<int>.GetEnumerator()
    {
      for (int value = this.Min; value <= this.Max; ++value)
        yield return value;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) ((IEnumerable<int>) this).GetEnumerator();
    }

    public static IntRange Parse(string raw)
    {
      Match match = IntRange.parser.Match(raw);
      if (match.Success)
        return new IntRange(int.Parse(match.Groups["min"].Value), int.Parse(match.Groups["max"].Value));
      throw new FormatException("Can't to parse \"" + raw + "\" to IntRange format. It must have next format: (int,int)");
    }

    public override string ToString()
    {
      return string.Format(IntRange.format, (object) this.min, (object) this.max);
    }

    public static implicit operator IntRange(int number)
    {
      return new IntRange(number, number);
    }
  }
}