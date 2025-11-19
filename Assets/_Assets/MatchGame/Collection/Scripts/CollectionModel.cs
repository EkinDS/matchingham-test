using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class CollectionModel
{
    public List<MatchlingPresenter> MatchlingPresenters;

    private int _collectionCapacity = 6;

    public CollectionModel()
    {
        MatchlingPresenters = new List<MatchlingPresenter>();
    }

    public bool TryToAddMatchlingToCollection(MatchlingPresenter matchlingPresenter)
    {
        if (MatchlingPresenters.Count >= _collectionCapacity)
        {
            return false;
        }

        int indexToAddNewMatchlingPresenter = MatchlingPresenters.Count;

        for (int i = MatchlingPresenters.Count - 1; i >= 0; i--)
        {
            if (MatchlingPresenters[i].GetMatchlingType() == matchlingPresenter.GetMatchlingType())
            {
                indexToAddNewMatchlingPresenter = i + 1;
                break;
            }
        }

        MatchlingPresenters.Insert(indexToAddNewMatchlingPresenter, matchlingPresenter);
        
        return true;
    }

    public void RemoveMatchlingPresenter(MatchlingPresenter matchlingPresenter)
    {
        MatchlingPresenters.Remove(matchlingPresenter);
    }

    public int GetMatchCenterIndex()
    {
        int index = -1;
        
        for (int i = 1; i < MatchlingPresenters.Count - 1; i++)
        {
            if (MatchlingPresenters[i - 1].GetMatchlingType() == MatchlingPresenters[i + 1].GetMatchlingType())
            {
                index = i;
            }
        }

        return index;
    }

    public List<MatchlingPresenter> Match(int index)
    {
        List<MatchlingPresenter> matchlingPresentersToMatch = new List<MatchlingPresenter>
        {
            MatchlingPresenters[index - 1],
            MatchlingPresenters[index],
            MatchlingPresenters[index + 1]
        };

        foreach (var matchlingPresenter in matchlingPresentersToMatch)
        {
            MatchlingPresenters.Remove(matchlingPresenter);
        }

        return matchlingPresentersToMatch;
    }
}