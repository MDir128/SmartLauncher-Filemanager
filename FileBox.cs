//объект каждого отдельного файла
public class FileBox
{
    public string? Address { get; set; } //адрес
    public string? Name { get; set; } //имя
    public string[]? Tags { get; set; } //тэги
    public Dictionary<string, string>? Meta { get; set; } //метаданные
    public string? Hash { get; set; } //хэш
    public string? Extension { get; set; } //расширение
    public string? Category { get; set; } //категория
}