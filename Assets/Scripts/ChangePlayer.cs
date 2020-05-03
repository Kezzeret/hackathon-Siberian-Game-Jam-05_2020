using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ChangePlayer : MonoBehaviour
{
    public GameObject[] players;
    private Transform current;
    private Vector3 position;

    public void ChangePlayerToFirst()
    {
        Debug.Log("to First");
        current = GameObject.FindGameObjectWithTag("Player").transform;
        position = current.position;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Instantiate(players[0], position, Quaternion.identity);
    }

    public void ChangePlayerToSecond ()
    {
        Debug.Log("to Second");
        current = GameObject.FindGameObjectWithTag("Player").transform;
        position = current.position;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Instantiate(players[1], position, Quaternion.identity);
    }

    public void ChangePlayerToThird()
    {
        Debug.Log("to Third");
        current = GameObject.FindGameObjectWithTag("Player").transform;
        position = current.position;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Instantiate(players[2], position, Quaternion.identity);
    }
}
