namespace DoeMais.Domain.ValueObjects;

public readonly record struct Cpf
{
    public string Value { get; }

    public Cpf(string value)
    {
        var cleaned = value?.Trim().Replace(".", "").Replace("-", "") ?? "";

        if (!BrazilianUtils.Cpf.IsValid(cleaned))
            throw new ArgumentException("Invalid CPF", nameof(value));

        Value = cleaned;        
    }

    public override string ToString()
    {
        return Convert.ToUInt64(Value).ToString(@"000\.000\.000\-00");
    }
}