using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChangeBehavior : MonoBehaviour
{
    public ParticleSystem[] weaponVFX;
    public AudioClip[] weaponSFX;
    public GameObject[] weapons;
    public GameObject weaponUI;
    public static int selectedWeaponIndex = 0;
    private Button[] buttons;
    private int previousWeaponIndex;

    void Awake() {
        PlantingGroundController.currentVFX = weaponVFX[selectedWeaponIndex];
        PlantingGroundController.currentSFX = weaponSFX[selectedWeaponIndex];
    }

    // Start is called before the first frame update
    void Start()
    {
        buttons = weaponUI.GetComponentsInChildren<Button>();
        UpdateWeapon();
        UpdateWeaponUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) {
            weaponVFX[selectedWeaponIndex].Stop();
            selectedWeaponIndex = 0;
        }

        if (Input.GetKeyDown("2")) {
            weaponVFX[selectedWeaponIndex].Stop();
            selectedWeaponIndex = 1;
        }

        if (previousWeaponIndex != selectedWeaponIndex) {
            previousWeaponIndex = selectedWeaponIndex;
            weaponVFX[previousWeaponIndex].Stop();
            UpdateWeaponUI();
            UpdateWeapon();
            UpdatePlantingGround();
        }
    }

    void UpdatePlantingGround() {
        PlantingGroundController.currentVFX = weaponVFX[selectedWeaponIndex];
        PlantingGroundController.currentSFX = weaponSFX[selectedWeaponIndex];
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
                selectedButton.transform.localScale *= 1.15f;
            } else {
                selectedButton.transform.localScale = new Vector3(1, 1, 1);
            }
            i++;
        }
    }
}
