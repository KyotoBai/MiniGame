using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefabControll : MonoBehaviour
{
    [SerializeField]
    private GameObject[] gunPrefabs;
    [SerializeField]
    private PlayerShootingController shootingController;
    [SerializeField]
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (shootingController.shootingOn == true)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
