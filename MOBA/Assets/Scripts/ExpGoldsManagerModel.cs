using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class ExpGoldsManagerModel
{
    [RealtimeProperty(1, true, true)] private RealtimeSet<ExpGoldsModel> _gains;
}