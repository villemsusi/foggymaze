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

    public static Action<int> OnSetLevelProgress;
    public static void SetLevelProgress(int amount) => OnSetLevelProgress?.Invoke(amount);
    
    public static Func<int> OnGetLevelProgress;
    public static int GetLevelProgress() => OnGetLevelProgress?.Invoke() ?? 0;

    public static event Action OnRestartGame;
    public static void RestartGame() => OnRestartGame?.Invoke();

    public static event Action OnNextStage;
    public static void NextStage() => OnNextStage?.Invoke();



    // Stage controller section

    public static event Action<int> OnSetTimer;
    public static void SetTimer(int amount) => OnSetTimer?.Invoke(amount);

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

    public static event Action<TurretData> OnTurretSelected;
    public static void SelectTurret(TurretData data) => OnTurretSelected?.Invoke(data);

    public static event Func<int> OnGetLootboxCount;
    public static int GetLootboxCount() => OnGetLootboxCount?.Invoke() ?? 0;

    public static event Func<int> OnGetTurretDropCount;
    public static int GetTurretDropCount() => OnGetTurretDropCount?.Invoke() ?? 0;

    public static event Func<bool> OnGetIsItemSelected;
    public static bool GetIsItemSelected() => OnGetIsItemSelected?.Invoke() ?? false;

    public static event Action<GameObject> OnAddInteractable;
    public static void AddInteractable(GameObject obj) => OnAddInteractable?.Invoke(obj);

    public static event Action<GameObject> OnRemoveInteractable;
    public static void RemoveInteractable(GameObject obj) => OnRemoveInteractable?.Invoke(obj);



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



    // Cosmetics section

    public static event Action<Color> OnSetAuraColor;
    public static void SetAuraColor(Color color) => OnSetAuraColor?.Invoke(color);

    public static event Func<Color> OnGetAuraColor;
    public static Color GetAuraColor() => OnGetAuraColor?.Invoke() ?? new Color();

    public static event Action<Color> OnSetProjectileColor;
    public static void SetProjectileColor(Color color) => OnSetProjectileColor?.Invoke(color);

    public static event Func<Color> OnGetProjectileColor;
    public static Color GetProjectileColor() => OnGetProjectileColor?.Invoke() ?? new Color();
}
