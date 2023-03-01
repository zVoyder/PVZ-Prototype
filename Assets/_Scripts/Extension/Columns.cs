using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Custom data structure for managing a List as if it were a column of each T object.
/// </summary>
/// <typeparam name="T">Generic Type T</typeparam>
[System.Serializable]
public class Column<T>
{
    public List<T> objects;

    public int Lenght { get => objects.Count; }

    public Column()
    {
        objects = new List<T>();
    }

    public T GetByIndex(int index)
    {
        return objects[index];
    }

    public void Add(T obj)
    {
        objects.Add(obj);
    }

    public void Remove(T obj)
    {
        objects.Remove(obj);
    }

    public bool isEmpty()
    {
        return objects.Count == 0;
    }
}


/// <summary>
/// Custom data structure for managing a List as if it were divided into columns.
/// </summary>
/// <typeparam name="T">Generic Type</typeparam>
[System.Serializable]
public class Columns<T>
{
    public Column<T>[] columns;

    public int Lenght { get => columns.Length; }
    
    public Columns(int size = 10)
    {
        columns = new Column<T>[size];

        for(int i = 0; i < size; i++)
        {
            columns[i] = new Column<T>();
        }
    }

    public Column<T> GetColumnByIndex(int index)
    {
        return columns[index];
    }

    public void AddToColumn(T obj, int columnIndex)
    {
        columns[columnIndex].Add(obj);
    }

    public void RemoveFromColumn(T obj, int columnIndex)
    {
        columns[columnIndex].Remove(obj);
    }
}


