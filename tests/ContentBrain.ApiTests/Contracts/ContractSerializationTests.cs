using System.Text.Json;
using ContentBrain.Contracts.Dto;
using Xunit;

namespace ContentBrain.ApiTests.Contracts;

public class ContractSerializationTests
{
    [Fact]
    public void IdeaGenerationRequest_SerializesAndDeserializes()
    {
        var dto = new IdeaGenerationRequestDto(
            Niche: "fitness",
            Audience: "busy beginners",
            Platform: "YouTube",
            Goal: "growth",
            SuccessMetric: new SuccessMetricDto("average_watch_time", 120.0, "seconds", "7d"),
            HistoricalContext: new HistoricalContextDto("provided", "recent avg watch time", new[] { new HistoricalMetricDto("average_watch_time", 110.0, System.DateTimeOffset.UtcNow) }),
            Count: 3
        );

        var json = JsonSerializer.Serialize(dto);
        var roundtrip = JsonSerializer.Deserialize<IdeaGenerationRequestDto>(json);

        Assert.NotNull(roundtrip);
        Assert.Equal(dto.Niche, roundtrip!.Niche);
        Assert.Equal(dto.SuccessMetric.MetricType, roundtrip.SuccessMetric.MetricType);
    }
}
