using System.Collections.Generic;

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
}