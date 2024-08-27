using System.Collections;
using Logic;
using Normal.Realtime;
using UnityEngine;

[RealtimeModel]
public partial class NormcoreTimer
{
    [RealtimeProperty(1, true, true)] private float _time;
}
