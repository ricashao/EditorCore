using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCFramework.Editor
{
    public class URandom
  {
    public static URandom main = new URandom();
    public readonly int seed;
    private int m;
    private int c;
    private int a;
    private int last;

    public URandom(int seed)
    {
      this.seed = seed;
      this.last = seed;
      this.m = 325648;
      this.c = 270312;
      this.a = 123856;
    }

    public URandom()
      : this(UnityEngine.Random.Range(int.MinValue, int.MaxValue))
    {
    }

    public float Value(string key = null)
    {
      int num;
      if (string.IsNullOrEmpty(key))
      {
        this.last = (this.a * this.last + this.c) % this.m;
        num = this.last;
      }
      else
        num = this.seed + URandom.GetCode(key);
      return Math.Abs(1f * (float) (this.a * num + this.c) % (float) this.m / (float) this.m);
    }

    public T ValueRange<T>(string key, params T[] values)
    {
      if (values.Length == 0)
        return default (T);
      return values[this.Range(0, values.Length - 1, key)];
    }

    public bool Chance(float probability, string key = null)
    {
      float num = this.Value(key);
      return (double) probability > (double) num;
    }

    public float Range(float min, float max, string key = null)
    {
      if ((double) min >= (double) max)
      {
        double num = (double) min;
        min = max;
        max = (float) num;
      }
      float num1 = this.Value(key);
      return min + (max - min) * num1;
    }

    public int Range(int min, int max, string key = null)
    {
      if (min >= max)
      {
        int num = min;
        min = max;
        max = num;
      }
      return (int) Math.Floor((double) this.Range((float) min, (float) max + 1f, key));
    }

    public float Range(FloatRange range, string key = null)
    {
      return this.Range(range.min, range.max, key);
    }

    public int Range(IntRange range, string key = null)
    {
      return this.Range(range.min, range.max, key);
    }

    public int Seed(string key = null)
    {
      int num;
      if (string.IsNullOrEmpty(key))
      {
        this.last = (this.a * this.last + this.c) % this.m;
        num = this.last;
      }
      else
        num = this.seed + URandom.GetCode(key);
      return (this.a * num + this.c) % this.m;
    }

    public URandom NewRandom(string key = null)
    {
      return new URandom(this.Seed(key));
    }

    private static int GetCode(string key)
    {
      return (int) ((double) Mathf.Pow((float) (key.GetHashCode() % 9651348), 3f) % 7645289.0);
    }

    public T ValueByProbability<T>(List<URandom.Event<T>> values, string key)
    {
      if (values != null)
      {
        float num = values.Sum<URandom.Event<T>>((Func<URandom.Event<T>, float>) (x => x.probability)) * this.Value(key);
        foreach (URandom.Event<T> @event in values)
        {
          num -= @event.probability;
          if ((double) num <= 0.0)
            return @event.info;
        }
      }
      return default (T);
    }

    public class Event<T>
    {
      internal T info;
      internal float probability;

      public Event(T info, float probability)
      {
        this.probability = probability;
        this.info = info;
      }
    }
  }
}