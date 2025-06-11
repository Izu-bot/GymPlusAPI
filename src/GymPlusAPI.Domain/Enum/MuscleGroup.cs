namespace GymPlusAPI.Domain.Enum;

[Flags]
public enum MuscleGroup
{
    None = 0,
    Peito = 1 << 0,
    Perna = 2 << 1,
    Costas = 3 << 2,
}
