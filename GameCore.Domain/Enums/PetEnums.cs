namespace GameCore.Domain.Enums
{
    /// <summary>
    /// 冒險類型
    /// </summary>
    public enum AdventureType
    {
        Forest = 1,
        Cave = 2,
        Mountain = 3,
        Ocean = 4,
        Space = 5,
        TimeTravel = 6
    }

    /// <summary>
    /// 冒險結果
    /// </summary>
    public enum AdventureResult
    {
        Victory = 1,
        Defeat = 2,
        Draw = 3,
        Escape = 4,
        Special = 5
    }

    /// <summary>
    /// 互動類型
    /// </summary>
    public enum InteractionType
    {
        Feed = 1,
        Play = 2,
        Bath = 3,
        Rest = 4,
        Train = 5,
        Pet = 6
    }

    /// <summary>
    /// 寵物互動類型
    /// </summary>
    public enum PetInteractionType
    {
        Feed = 1,
        Play = 2,
        Bath = 3,
        Rest = 4,
        Train = 5,
        Pet = 6,
        ColorChange = 7
    }
} 
