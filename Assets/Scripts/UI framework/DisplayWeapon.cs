using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWeapon : MonoBehaviour
{
    [SerializeField]
    private PlayerPrefabControll playerPrefabControll;
    [SerializeField]
    private Image[] imgs;
    private int curr;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Image img in imgs)
        {
            img.gameObject.SetActive(false);
        }

        if (playerPrefabControll.getDisplayOnOff())
        {
            curr = 0;
            imgs[playerPrefabControll.getWeaponOnIndex()].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPrefabControll.getDisplayOnOff())
        {
            if (curr != playerPrefabControll.getWeaponOnIndex())
            {
                foreach (Image img in imgs)
                {
                    img.gameObject.SetActive(false);
                }

                imgs[playerPrefabControll.getWeaponOnIndex()].gameObject.SetActive(true);
                curr = playerPrefabControll.getWeaponOnIndex();
            }
        }
        else
        {
            foreach (Image img in imgs)
            {
                img.gameObject.SetActive(false);
            }
            curr = -1;
        }
    }
}
