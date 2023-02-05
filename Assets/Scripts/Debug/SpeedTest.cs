using System.Collections;
using System.Collections.Generic;
using Licht.Unity.Physics;
using TMPro;
using UnityEngine;

public class SpeedTest : MonoBehaviour
{
    public TMP_Text TextComponent;

    public LichtPhysicsObject PhysicsObject;
    public LichtPhysicsUpdater Updater;

    public void LateUpdate()
    {
        TextComponent.text = $"is updating: {Updater.IsUpdating}";
    }
}
