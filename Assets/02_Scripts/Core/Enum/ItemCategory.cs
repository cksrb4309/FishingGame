[System.Flags]
public enum ItemCategory
{
    All = 0,         // 모두
    Material = 1 << 0,  
    Functional = 1 << 1,  
    Event = 1 << 2,  
    Tool = 1 << 3   
}
