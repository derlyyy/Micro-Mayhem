using UnityEngine;

public class MapBorder : MonoBehaviour
{
    public GameObject[] mapsToLoad;
    public GameObject[] mapsToUnload;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadMaps();
            UnloadMaps();
        }
    }

    public void LoadMaps()
    {
        for (int i = 0; i < mapsToLoad.Length; i++)
        {
            if (!mapsToLoad[i].activeSelf)
            {
                mapsToLoad[i].SetActive(true);
            }
        }
    }

    public void UnloadMaps()
    {
        for (int i = 0; i < mapsToUnload.Length; i++)
        {
            if (mapsToUnload[i].activeSelf)
            {
                mapsToUnload[i].SetActive(false);
            }
        }
    }
}