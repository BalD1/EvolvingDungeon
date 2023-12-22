using UnityEngine;

[System.Serializable]
public abstract class MinMax<T>
{
    [field: SerializeField] public T Min {get; private set; }
    [field: SerializeField] public T Max { get; private set; }

    public MinMax(T min, T max)
    {
        this.Min = min;
        this.Max = max;
    }

    public void SetMin(T min)
        => Min = min;

    public void SetMax(T max)
        => Max = max;

    public abstract T GetRandomInRange();
    public abstract T GetAverage();
}

[System.Serializable]
public class MinMax_Float : MinMax<float>
{
    public MinMax_Float(float min, float max) : base(min, max) { }

    public override float GetRandomInRange()
        => Random.Range(Min, Max);

    public override float GetAverage()
        => (Min + Max) / 2;
}

[System.Serializable]
public class MinMax_Int : MinMax<int>
{
    public MinMax_Int(int min, int max) : base(min, max) { }

    public override int GetRandomInRange()
        => Random.Range(Min, Max);

    public override int GetAverage()
        => (Min + Max) / 2;
}

[System.Serializable]
public class MinMax_Vector2 : MinMax<Vector2>
{
    public MinMax_Vector2(Vector2 min, Vector2 max) : base(min, max) { }

    public override Vector2 GetRandomInRange()
    {
        return new Vector2(Random.Range(Min.x, Max.x),
                           Random.Range(Min.y, Max.y));
    }

    public override Vector2 GetAverage()
        => (Min + Max) / 2;
}

[System.Serializable]
public class MinMax_Vector3 : MinMax<Vector3>
{
    public MinMax_Vector3(Vector3 min, Vector3 max) : base(min, max) { }

    public override Vector3 GetRandomInRange()
    {
        return new Vector3(Random.Range(Min.x, Max.x),
                           Random.Range(Min.y, Max.y),
                           Random.Range(Min.z, Max.z));
    }

    public override Vector3 GetAverage()
        => (Min + Max) / 2;
}