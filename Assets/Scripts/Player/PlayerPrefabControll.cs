using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private bool[] weaponStatus;
    private int gunIndexTrue = 0; //record the current gun type that is active
    private Transform placeGunHere;
    // Start is called before the first frame update
    void Start() 
    {
        GameObject playerWithGun = GameObject.Find("Player With Gun");
        
        if (playerWithGun != null)
        {
            placeGunHere = playerWithGun.transform.Find("Place Gun Here");
        }

        weaponStatus = new bool[gunPrefabs.Length];
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);

        for (int i = 0; i < gunPrefabs.Length; i++)
        {
            GameObject newGun = Instantiate(gunPrefabs[i], placeGunHere.position, Quaternion.LookRotation(transform.forward));
            newGun.transform.SetParent(placeGunHere, true);
            newGun.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shootingController.shootingOn == true)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            if (Input.GetKey(KeyCode.Alpha1))
            {
                gunIndexTrue = 0;
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                gunIndexTrue = 1;
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                gunIndexTrue = 2;
            }
            else if (Input.GetKey(KeyCode.Alpha4))
            {
                gunIndexTrue = 3;
            }

            placeGunHere.GetChild(gunIndexTrue).gameObject.SetActive(true);
            weaponStatus[gunIndexTrue] = true;
            shootingController.SetGunShooting(placeGunHere.GetChild(gunIndexTrue).GetComponent<GunShooting>());
            for (int j = 0; j < weaponStatus.Length; j++)
            {
                if (j != gunIndexTrue)
                {
                    placeGunHere.GetChild(j).gameObject.SetActive(false);
                    weaponStatus[j] = false;
                }
            }
            
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            
        }
    }

    public bool getDisplayOnOff()
    {
        return shootingController.shootingOn;
    }

    public int getWeaponOnIndex()
    {
        return gunIndexTrue;
    }

    public float getWeaponMovingDrag()
    {
        return placeGunHere.GetChild(getWeaponOnIndex()).GetComponent<GunShooting>().GetDrag();
    }
}
