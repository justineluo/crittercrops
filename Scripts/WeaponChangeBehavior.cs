using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChangeBehavior : MonoBehaviour
{
    public ParticleSystem[] weaponVFX;
    public GameObject[] weapons;
    public GameObject weaponUI;
    public static int selectedWeaponIndex = 0;
    private Button[] buttons;
    private int previousWeaponIndex;

    void Awake() {
        PlantingGroundController.currentVFX = weaponVFX[selectedWeaponIndex];
    }
    
    // Start is called before the first frame update
    void Start()
    {
        buttons = weaponUI.GetComponentsInChildren<Button>();
        weapons[selectedWeaponIndex].SetActive(true);
        UpdateWeaponUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) {
            selectedWeaponIndex = 0;
        }

        if (Input.GetKeyDown("2")) {
            selectedWeaponIndex = 1;
        }

        var currentVFX = weaponVFX[selectedWeaponIndex];
        PlantingGroundController.currentVFX = currentVFX;

        if (previousWeaponIndex != selectedWeaponIndex) {
            previousWeaponIndex = selectedWeaponIndex;
            weaponVFX[previousWeaponIndex].Stop();
            UpdateWeaponUI();
            UpdateWeapon();
        }
    }

    void UpdateWeapon() {
        int i = 0;

        foreach(GameObject selectedWeapon in weapons) {
            if (i == selectedWeaponIndex) {
                selectedWeapon.SetActive(true);
            } else {
                selectedWeapon.SetActive(false);
            }
            i++;
        }
    }

    void UpdateWeaponUI() {
        int i = 0;

        foreach(Button selectedButton in buttons) {
            if (i == selectedWeaponIndex) {
                selectedButton.transform.localScale *= 1.25f;
            } else {
                selectedButton.transform.localScale = new Vector3(1, 1, 1);
            }
            i++;
        }
    }
}
