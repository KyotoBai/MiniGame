using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

            if (transform.GetChild(1).GetChild(0).childCount == 0)
            {
                GameObject selectedGunPrefab = gunPrefabs[0];

                // Instantiate the tree
                GameObject newGun = Instantiate(selectedGunPrefab, transform.GetChild(1).GetChild(0).position, Quaternion.Euler(0, 0, 0));

                newGun.transform.SetParent(transform.GetChild(1).GetChild(0), true);
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
