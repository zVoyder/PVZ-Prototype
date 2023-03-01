using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Custom Data Structure to manage a List like a loop. If the list reach the end, it will start over from the head.
/// </summary>
/// <typeparam name="T">Generic Type</typeparam>
[System.Serializable]
public class LoopList<T>
{
    public List<T> objects;
    private int _index;

    public LoopList(List<T> objects)
    {
        this.objects = objects;
        _index = 0;
    }

    /// <summary>
    /// Reset the index
    /// </summary>
    public void Reset()
    {
        _index = 0;
    }

    /// <summary>
    /// Get the Current object and move the index to the next object.
    /// </summary>
    /// <returns>Current Object</returns>
    public T Next()
    {
        if (_index >= objects.Count)
            _index = 0;

        return objects[_index++];
    }

    /// <summary>
    /// Get the Current object and move the index to the previous object.
    /// </summary>
    /// <returns>Current Object</returns>
    public T Previous()
    {
        if(_index < 0)
        {
            _index = objects.Count - 1;
        }

        return objects[_index--];
    }

}
