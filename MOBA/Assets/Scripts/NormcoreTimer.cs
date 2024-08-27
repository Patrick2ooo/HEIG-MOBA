using System.Collections;
using Logic;
using Normal.Realtime;
using UnityEngine;

[RealtimeModel(createMetaModel: true)]
public partial class NormcoreTimer
{
    [RealtimeProperty(1, true, true)] private double _time;
    [RealtimeProperty(2, true, true)] private ushort _minionWaves;
}
