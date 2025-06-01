using System;

[Serializable]
public class Row<T>
{
    public T[] items;
    public T this[int index]
    {
        get => items[index];
        set => items[index] = value;
    }
    public Row(int length)
    {
        items = new T[length];
    }
}