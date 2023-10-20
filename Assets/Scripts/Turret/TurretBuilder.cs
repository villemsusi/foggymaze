using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    public TurretData CurrentTurretData;

    public List<TurretData> TurretDatas;

    private void Awake()
    {
        Events.OnTurretSelected += TurretSelected;

        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        Events.OnTurretSelected -= TurretSelected;
    }




    // Update is called once per frame
    void Update()
    {
        transform.position = Events.GetPlayerPosition();

    }

    private void TurretSelected(TurretData data)
    {
        Instantiate(data.TurretPrefab, transform.position, Quaternion.identity, null);
        Events.SetTurretCount(Events.GetTurretCount() - 1);
        gameObject.SetActive(false);
    }



}
