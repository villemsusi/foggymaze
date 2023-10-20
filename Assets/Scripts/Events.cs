using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Events
{
    // Permanent stats section

    public static event Action<int> OnSetHealthPerm;
    public static void SetHealthPerm(int amount) => OnSetHealthPerm?.Invoke(amount);

    public static event Func<int> OnGetHealthPerm;
    public static int GetHealthPerm() => OnGetHealthPerm?.Invoke() ?? 0;

    public static event Action<float> OnSetMovespeedPerm;
    public static void SetMovespeedPerm(float amount) => OnSetMovespeedPerm?.Invoke(amount);

    public static event Func<float> OnGetMovespeedPerm;
    public static float GetMovespeedPerm() => OnGetMovespeedPerm?.Invoke() ?? 0;



    // Game controller section

    public static event Action OnRestartGame;
    public static void RestartGame() => OnRestartGame?.Invoke();

    public static event Action OnNextStage;
    public static void NextStage() => OnNextStage?.Invoke();



    // Stage controller section

    public static event Action OnAugmentsEnable;
    public static void EnableAugments() => OnAugmentsEnable?.Invoke();

    public static event Action OnEnableStairs;
    public static void EnableStairs() => OnEnableStairs?.Invoke();

    public static event Func<bool> OnGetStairsOpen;
    public static bool GetStairsOpen() => OnGetStairsOpen?.Invoke() ?? false;



    // Player items section

    public static event Action<int> OnSetAmmoCount;
    public static void SetAmmoCount(int amount) => OnSetAmmoCount?.Invoke(amount);

    public static event Func<int> OnGetAmmoCount;
    public static int GetAmmoCount() => OnGetAmmoCount?.Invoke() ?? 0;

    public static event Action<int> OnSetUpgradeCount;
    public static void SetUpgradeCount(int amount) => OnSetUpgradeCount?.Invoke(amount);

    public static event Func<int> OnGetUpgradeCount;
    public static int GetUpgradeCount() => OnGetUpgradeCount?.Invoke() ?? 0;

    public static event Action<int> OnSetTurretCount;
    public static void SetTurretCount(int amount) => OnSetTurretCount?.Invoke(amount);

    public static event Func<int> OnGetTurretCount;
    public static int GetTurretCount() => OnGetTurretCount?.Invoke() ?? 0;

    public static event Action<TowerData> OnTowerSelected;
    public static void SelectTower(TowerData data) => OnTowerSelected?.Invoke(data);



    // Player stats section

    public static event Action<int> OnSetHealth;
    public static void SetHealth(int amount) => OnSetHealth?.Invoke(amount);

    public static event Func<int> OnGetHealth;
    public static int GetHealth() => OnGetHealth?.Invoke() ?? 0;

    public static event Action<float> OnSetMovespeed;
    public static void SetMovespeed(float amount) => OnSetMovespeed?.Invoke(amount);

    public static event Func<float> OnGetMovespeed;
    public static float GetMovespeed() => OnGetMovespeed?.Invoke() ?? 0;

    public static event Func<Vector3> OnGetPlayerPosition;
    public static Vector3 GetPlayerPosition() => OnGetPlayerPosition?.Invoke() ?? Vector3.zero;




}
