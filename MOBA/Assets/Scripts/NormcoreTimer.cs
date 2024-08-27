using System.Collections;
using Scripts;
using Normal.Realtime;
using UnityEngine;

[RealtimeModel(createMetaModel: true)]
public partial class NormcoreTimer
{
    [RealtimeProperty(1, true, true)] private double _time;
    [RealtimeProperty(2, true, true)] private ushort _minionWaves;
}
