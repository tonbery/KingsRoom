using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListShuffleBag<T>
{
    private List<T> _bag;
    private readonly List<T> _elements;

    public ListShuffleBag(List<T> PossibleElements)
    {
        _elements = new List<T>(PossibleElements);
        CreateBag();
    }
    public ListShuffleBag(T[] PossibleElements)
    {
        _elements = new List<T>(PossibleElements);
        CreateBag();
    }

    public T Pick()
    {
        int randomIndex = Random.Range(0, _bag.Count - 1);
        var pick = _bag[randomIndex];
        _bag.RemoveAt(randomIndex);
        
        if(_bag.Count == 0) CreateBag();

        return pick;
    }

    void CreateBag()
    {
        _bag = new List<T>(_elements);
    }

    public void ResetBag()
    {
        CreateBag();
    }

    public void AddToBag(T element)
    {
        _bag.Add(element);
    }

    public void AddToElements(T element)
    {
        _elements.Add(element);
    }
    
    public void RemoveFromBag(T element)
    {
        if(_bag.Contains(element)) _bag.Remove(element);
    }

    public void RemoveFromElements(T element)
    {
        if(_elements.Contains(element)) _elements.Remove(element);
    }}
