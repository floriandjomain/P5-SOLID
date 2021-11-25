using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

[CreateAssetMenu(menuName ="Cooldown")]
public class Cooldown : ScriptableObject
{
    [SerializeField]
    [Tooltip("Durée en ms")]
    private float _cooldownDuration;

    public void StartCooldown()
    {
        _timer.Restart();
    }

    public void StopCooldown()
    {
        _timer.Stop();
    }

    public bool IsCooldownDone()
    {
        return _timer.ElapsedMilliseconds > _cooldownDuration;
    }

    private Stopwatch _timer = new Stopwatch();
}
