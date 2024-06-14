using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class SpawnPlayer : MonoBehaviourPunCallbacks
{

    public GameObject playerPrefabs;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxY), Random.Range(minY, maxY));
        PhotonNetwork.Instantiate(playerPrefabs.name, randomPosition, Quaternion.identity);
    }

}
