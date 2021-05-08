using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField] AudioClip[] stoneClips;
    [SerializeField] AudioClip[] mudClips;
    [SerializeField] AudioClip[] grassClips;
    [SerializeField] AudioClip[] sandClips;

    [SerializeField] AudioSource audioSource = null;
    TerrainDetector terrainDetector;

    void Awake()
    {
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }        
        terrainDetector = new TerrainDetector();
    }

    void Step()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    AudioClip GetRandomClip()
    {
        int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

        switch (terrainTextureIndex)
        {
            case 0:
                return grassClips[UnityEngine.Random.Range(0, grassClips.Length)];
            case 1:
                return mudClips[UnityEngine.Random.Range(0, mudClips.Length)];
            case 2:
                return stoneClips[UnityEngine.Random.Range(0, stoneClips.Length)];
            default:
                return sandClips[UnityEngine.Random.Range(0, sandClips.Length)];
        }

    }
}