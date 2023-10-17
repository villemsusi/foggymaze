using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Events
{
    public static event Action OnRestartGame;
    public static void RestartGame() => OnRestartGame?.Invoke();
    public static event Action OnExpandFog;
    public static void ExpandFog() => OnExpandFog?.Invoke();


    public static event Action<int> OnSetMoney;
    public static void SetMoney(int amount) => OnSetMoney?.Invoke(amount);

    public static event Func<int> OnGetMoney;
    public static int GetMoney() => OnGetMoney?.Invoke() ?? 0;

    public static event Action<int> OnSetHealth;
    public static void SetHealth(int amount) => OnSetHealth?.Invoke(amount);

    public static event Func<int> OnGetHealth;
    public static int GetHealth() => OnGetHealth?.Invoke() ?? 0;
    public static event Func<Vector3> OnGetPlayerPosition;
    public static Vector3 GetPlayerPosition() => OnGetPlayerPosition?.Invoke() ?? Vector3.zero;




    public static event Action<TowerData> OnTowerSelected;
    public static void SelectTower(TowerData data) => OnTowerSelected?.Invoke(data);

}
