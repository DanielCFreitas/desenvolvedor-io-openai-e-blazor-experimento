﻿namespace AssistenteDeEnsino.Services.OpenAI;

public static class GptModelExtension
{
    public static string ToApiModel(this GptModel model)
    {
        return model switch
        {
            GptModel.Gpt3_5Turbo => "gpt-3.5-turbo",
            GptModel.Gpt4 => "gpt-4",
            GptModel.Gpt4o => "gpt-4o",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
