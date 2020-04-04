using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar]
    public int health = 100;

    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            //we want to respawn
            health = 100;
            StartCoroutine(Respawn(gameObject));
        }
    }


    [Server]
    IEnumerator Respawn(GameObject go)
    {
        NetworkServer.UnSpawn(go);
        Transform newPos = NetworkManager.singleton.GetStartPosition();
        go.transform.position = newPos.position;
        go.transform.rotation = newPos.rotation;
        yield return new WaitForSeconds(1f);
        NetworkServer.Spawn(go, go);
    }
}
