using SonsSdk.Attributes;
using UnityEngine;

namespace TankMod;

[AssetBundle("tank")]
public class Tank
{
    [AssetReference("TankLeopard")]
    public static GameObject TankPrefab { get; set; }
}

[AssetBundle("tankexplosion")]
public class TankExplosion
{
    [AssetReference("TankExplosion")]
    public static GameObject TankExplosionPrefab { get; set; }
}
