using Example1.Domain.Contexts.BotPlatform.Enums;

namespace Example1.Domain.Contexts.BotPlatform;

public class FileBox
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public byte[] Data { get; set; }

    public string Name { get; set; }

    public EFileType Type { get; set; }

    public DateTime Create { get; set; }

    public virtual User User { get; set; }
}