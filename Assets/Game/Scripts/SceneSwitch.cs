using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {

        Debug.Log("Player has entered");

        SceneManager.LoadScene(3);
    }

}
