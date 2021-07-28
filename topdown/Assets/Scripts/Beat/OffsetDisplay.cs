using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OffsetDisplay : MonoBehaviour
{
    public ConducterCalibration conducterCalibration;
    public TextMeshProUGUI offsetText;
    public TextMeshProUGUI recText;
    public void UpdateRec()
    {
        recText.text = "Recommended Offset: " + (conducterCalibration.calibrationOffset * 1000).ToString("F0") + " ms";
        offsetText.text = "Current Offset: " + (conducterCalibration.calibrationOffset * 1000).ToString("F0") + " ms";
    }
     
    public void OnSet()
    {
        offsetText.text = "Current Offset: " + (conducterCalibration.calibrationOffset * 1000).ToString("F0") + " ms";
    }

    public void IncOffset(bool upInc)
    {
        if (upInc) conducterCalibration.calibrationOffset += 0.001f;
        else conducterCalibration.calibrationOffset -= 0.001f;
        OnSet();
    }

    public void OnDisable()
    {
        ES3.Save("inputOffset", conducterCalibration.calibrationOffset);
    }
}
