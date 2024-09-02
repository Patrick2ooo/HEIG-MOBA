using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class DamageManagerModel
{
    [RealtimeProperty(1, true, true)] private RealtimeSet<DamageModel> _damages;
}
