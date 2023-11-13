using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretBuilder : MonoBehaviour
{

    private void Awake()
    {
        Events.OnTurretSelected += TurretSelected;

        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        Events.OnTurretSelected -= TurretSelected;
    }


    void Update()
    {
        transform.position = Events.GetPlayerPosition();

    }

    private void TurretSelected(TurretData data)
    {
        if (Events.GetIsItemSelected())
            return;
        if (Events.GetTurretCount() == 0)
        {
            DataManager.Instance.DenyAudio.Play();
            return;
        }


        DataManager.Instance.BuildAudio.Play();

        Vector3 pos = transform.position;
        pos.y -= 0.2f;
        Turret turret = Instantiate(data.TurretPrefab, pos, Quaternion.identity, null);
        Events.AddInteractable(turret.gameObject);
        Events.SetTurretCount(Events.GetTurretCount() - 1);
        gameObject.SetActive(false);
    }

}
