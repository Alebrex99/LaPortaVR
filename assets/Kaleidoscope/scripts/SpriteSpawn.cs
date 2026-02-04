using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSpawn : MonoBehaviour
{
    public Sprite[] sprites;
    public float lowBound = -0.5f;
    public float highBound = 0.5f;
    public float spawnDelay;

    public GameObject spritePrefab;
    private GameObject _gameObject;
    private float lastSpawnTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > (lastSpawnTime + spawnDelay))
        {
            int randomIndex = Random.Range(0, sprites.Length);
            Vector3 randomSpawnPosition = new Vector3(Random.Range(transform.position.x+lowBound, transform.position.x+highBound), transform.position.y-0.5f, Random.Range(transform.position.z+lowBound, transform.position.z+highBound));
            spritePrefab.GetComponent<SpriteRenderer>().sprite = sprites[randomIndex];
            _gameObject = Instantiate(spritePrefab, randomSpawnPosition, Quaternion.identity);
            _gameObject.GetComponent<SpriteMove>().enabled = true;
            lastSpawnTime = Time.time;
        }
    }
}
