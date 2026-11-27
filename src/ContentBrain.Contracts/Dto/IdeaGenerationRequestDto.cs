namespace ContentBrain.Contracts.Dto;

public record SuccessMetricDto(string MetricType, double TargetValue, string? Unit, string? TimeWindow);

public record HistoricalMetricDto(string MetricType, double Value, System.DateTimeOffset ObservedAt);

public record HistoricalContextDto(string Availability, string? Summary, HistoricalMetricDto[]? Metrics);

public record IdeaGenerationRequestDto(
    string Niche,
    string Audience,
    string Platform,
    string Goal,
    SuccessMetricDto SuccessMetric,
    HistoricalContextDto? HistoricalContext,
    int? Count
);
