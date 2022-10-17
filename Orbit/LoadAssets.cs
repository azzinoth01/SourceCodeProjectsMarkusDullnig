using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;


/// <summary>
/// class to load assets while runtime
/// </summary>
public class LoadAssets
{
    private List<AsyncOperationHandle> handleList;


    /// <summary>
    /// standard constructor
    /// </summary>
    public LoadAssets() {
        handleList = new List<AsyncOperationHandle>();
    }

    /// <summary>
    /// loads a sprite out from the addressables path
    /// </summary>
    /// <param name="path"> the addressables path</param>
    /// <returns> the loaded sprite</returns>
    public Sprite loadSprite(string path) {
        if (path == "" || path == null) {
            //   Debug.LogError(" empty path");
            return null;
        }
        // Debug.LogError(path);
        Sprite sprite;

        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(path);
        handleList.Add(handle);
        handle.WaitForCompletion();


        if (handle.Status == AsyncOperationStatus.Succeeded) {
            sprite = handle.Result;
        }
        else {
            sprite = null;
        }

        //Addressables.Release(handle);
        return sprite;
    }


    /// <summary>
    /// loads a gameobject out from the addressables path
    /// </summary>
    /// <param name="path"> the addressables path</param>
    /// <returns> the loaded gameobject</returns>
    public GameObject loadGameObject(string path) {
        if (path == "" || path == null) {
            //  Debug.LogError(" empty path");
            return null;
        }
        //    Debug.LogError(path);
        GameObject game;
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(path);
        handleList.Add(handle);
        handle.WaitForCompletion();

        if (handle.Status == AsyncOperationStatus.Succeeded) {
            game = handle.Result;
        }
        else {
            game = null;
        }
        // Addressables.Release(handle);
        return game;
    }

    /// <summary>
    /// loads a textAsset out from the addressables path
    /// </summary>
    /// <param name="path"> the addressables path</param>
    /// <returns> the loaded textAsset</returns>
    public TextAsset loadText(string path) {
        if (path == "" || path == null) {
            //  Debug.LogError(" empty path");
            return null;
        }
        //path = path.Replace("Assets/", "");

        // Debug.LogError(path);
        TextAsset text;

        AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(path);
        handleList.Add(handle);
        handle.WaitForCompletion();

        // Debug.LogError("asst loaded");

        if (handle.Status == AsyncOperationStatus.Succeeded) {
            //   Debug.LogError("funktioniert");
            text = handle.Result;

            // Debug.LogError(text.text);
        }
        else {
            //Debug.LogError("fehler");
            text = null;
        }
        // Addressables.Release(handle);

        return text;
    }

    /// <summary>
    /// releases the last loaded handel to make space in ram
    /// </summary>
    public void releaseLastHandle() {
        Addressables.Release(handleList[(handleList.Count - 1)]);
        handleList.RemoveAt((handleList.Count - 1));
    }

    /// <summary>
    /// releases all loaded handels to make space in ram
    /// </summary>
    public void releaseAllHandle() {
        foreach (AsyncOperationHandle handle in handleList) {
            Addressables.Release(handle);
        }
    }

}
