using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject charSelect;
    public GameObject calibration;
    public ConducterCalibration conducterCalibration;

    public void OpenCharSelect()
    {
        titleScreen.SetActive(false);
        calibration.SetActive(false);
        charSelect.SetActive(true);
    }
    public void OpenTitle()
    {
        titleScreen.SetActive(true);
        calibration.SetActive(false);
        charSelect.SetActive(false);
    }
    public void OpenCalibration()
    {
        titleScreen.SetActive(false);
        charSelect.SetActive(false);
        calibration.SetActive(true);
        conducterCalibration.ResetCalibration();
    }
}
