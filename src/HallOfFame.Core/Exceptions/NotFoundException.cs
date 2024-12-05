namespace HallOfFame.Core.Exceptions;

/// <summary>
/// Исключение, выбрасываемое, когда запрашиваемый ресурс не найден.
/// </summary>
/// <param name="message">Сообщение, описывающее причину исключения.</param>
public class NotFoundException : Exception
{
    /// <summary>
    /// Создает новый экземпляр класса <see cref="NotFoundException"/> с указанным сообщением.
    /// </summary>
    /// <param name="message"> Сообщение, которое будет передано в базовый класс <see cref="Exception"/>. </param>
    public NotFoundException(string message) : base(message) { }
}
