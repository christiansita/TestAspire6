namespace ContentBrain.Contracts.Dto;

public record StyleReviewDto(
    System.Guid Id,
    System.Guid ScriptId,
    int ScriptVersion,
    string Status,
    int? AudienceFit,
    int? PlatformFormat,
    int? HookClarity,
    int? Tone,
    int? Pacing,
    int? Vocabulary,
    int? Structure,
    int? ProductionReadiness,
    string EvaluationSource,
    System.DateTimeOffset EvaluatedAt
);
