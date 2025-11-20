using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpriteService : MonoBehaviour
{
    private struct SpriteRuntimeData
    {
        public AsyncOperationHandle<Sprite> Handle;
        public Sprite Sprite;
    }

    private readonly Dictionary<MatchlingType, SpriteRuntimeData> _loadedMatchlings = new();

    private readonly Dictionary<BackgroundType, SpriteRuntimeData> _loadedBackgrounds = new();


    public IEnumerator LoadMatchlingSprites(HashSet<MatchlingType> typesToLoad)
    {
        foreach (var type in typesToLoad)
        {
            if (type == MatchlingType.None)
            {
                continue;
            }

            if (_loadedMatchlings.ContainsKey(type))
            {
                continue;
            }

            string key = type.ToString();

            var handle = Addressables.LoadAssetAsync<Sprite>(key);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _loadedMatchlings[type] = new SpriteRuntimeData
                {
                    Handle = handle,
                    Sprite = handle.Result
                };
            }
            else
            {
                Debug.LogError($"Failed to load sprite for MatchlingType '{type}' with key '{key}'");
            }
        }
    }

    public IEnumerator LoadBackgroundSprite(BackgroundType backgroundType)
    {
        if (backgroundType == BackgroundType.None)
        {
            yield break;
        }

        if (_loadedBackgrounds.ContainsKey(backgroundType))
        {
            yield break;
        }

        string key = backgroundType.ToString(); // Addressables key == enum name

        var handle = Addressables.LoadAssetAsync<Sprite>(key);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _loadedBackgrounds[backgroundType] = new SpriteRuntimeData
            {
                Handle = handle,
                Sprite = handle.Result
            };
        }
        else
        {
            Debug.LogError($"Failed to load background sprite for BackgroundType '{backgroundType}' with key '{key}'");
        }
    }

    public Sprite GetMatchlingSprite(MatchlingType type)
    {
        if (_loadedMatchlings.TryGetValue(type, out var data))
        {
            return data.Sprite;
        }

        return null;
    }

    public Sprite GetBackgroundSprite(BackgroundType type)
    {
        if (type == BackgroundType.None)
        {
            return null;
        }

        if (_loadedBackgrounds.TryGetValue(type, out var data))
        {
            return data.Sprite;
        }

        return null;
    }

    public void UnloadAll()
    {
        foreach (var kvp in _loadedMatchlings)
        {
            Addressables.Release(kvp.Value.Handle);
        }

        foreach (var kvp in _loadedBackgrounds)
        {
            Addressables.Release(kvp.Value.Handle);
        }

        _loadedMatchlings.Clear();
        _loadedBackgrounds.Clear();
    }
}