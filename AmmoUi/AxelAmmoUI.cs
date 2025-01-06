using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using RedLoader;
using TheForest.Utils;

namespace AmmoUi;

[RegisterTypeInIl2Cpp]
public class AxelAmmoUI : MonoBehaviour
{
    void Update()
    {
        if (LocalPlayer.Inventory.RightHandItem?.Data._weaponType == Sons.Items.Core.ItemData.WeaponType.AmmoBased && !LocalPlayer.IsInInventory)
        {
            AmmoUiUi.AmmoInfo.Set($"<size=80>{AmmoUi.RemainingAmmo}</size><size=60>/{AmmoUi.TotalAmmo}</size>");
            AmmoUiUi.AmmoIcon.Set(LocalPlayer.Inventory.RightHandItem?.Data.UiData._icon);
            AmmoUiUi.ShowPanel.Set(true);
        }
        else AmmoUiUi.ShowPanel.Set(false);
    }
}
