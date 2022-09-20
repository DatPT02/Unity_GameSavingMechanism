using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value = 10;
    [SerializeField] float rotateSpeed = 0.5f;

    private bool destroyed = false;

    private string coin_id ;

    void Awake()
    {
        coin_id = this.gameObject.name; 
    }

    void Update()
    {
        transform.rotation *= Quaternion.Euler(0,rotateSpeed,0);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Player>().Score += value;
        IsDestroyed = true;
    }

    public bool IsDestroyed
    {
        get 
        {
            return destroyed;
        }
        set
        {
            destroyed = value;
            destroyObj();
        }
    }

    public string Coin_ID
    {
        get
        {
            return coin_id;
        }
    }

    void destroyObj()
    {
        this.gameObject.SetActive(!IsDestroyed);
    }
}

[System.Serializable]
public class CoinData
{
    public string coin_id;
    public bool coin_isDestroyed;

    public CoinData(string id, bool destroyed)
    {
        coin_id = id;
        coin_isDestroyed = destroyed;
    }
}
