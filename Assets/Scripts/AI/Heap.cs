using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    // We don't like garbage
    int indexA;
    int indexB;
    int indexTemp;
    T itemTemp;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        sortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        itemTemp = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        sortDown(items[0]);
        return itemTemp;
    }
    
    public void UpdateItem(T item)
    {
        sortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    void sortUp(T item)
    {
        indexTemp = (item.HeapIndex - 1) / 2; // Parent index

        while (true)
        {
            itemTemp = items[indexTemp]; // Parent item
            if (item.CompareTo(itemTemp) > 0) swap(item, itemTemp);
            else break;

            indexTemp = (item.HeapIndex - 1) / 2;
        }
    }

    void sortDown(T item)
    {
        while (true)
        {
            indexA = item.HeapIndex * 2 + 1;
            indexB = item.HeapIndex * 2 + 2;
            indexTemp = 0;

            if (indexA < currentItemCount)
            {
                indexTemp = indexA;

                if (indexB < currentItemCount)
                {
                    if (items[indexA].CompareTo(items[indexB]) < 0) indexTemp = indexB;
                }

                if (item.CompareTo(items[indexTemp]) < 0) swap(item, items[indexTemp]);
                else return;
            }
            else return;
        }
    }

    void swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        indexA = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = indexA;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}