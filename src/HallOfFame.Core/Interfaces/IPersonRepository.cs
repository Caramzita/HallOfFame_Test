namespace HallOfFame.Core.Interfaces;

/// <summary>
/// Интерфейс для управления операциями с данными сотрудников.
/// </summary>
public interface IPersonRepository
{
    /// <summary>
    /// Получает всех сотрудников в виде асинхронного перечисления.
    /// </summary>
    /// <returns> Асинхронное перечисление, содержащее список сотрудников. </returns>
    IAsyncEnumerable<Person> GetAllPersons();

    /// <summary>
    /// Получает сотрудника по его идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор сотрудника. </param>
    /// <returns> Возвращает сотрудника, если он найден, или <c>null</c>, если не найден. </returns>
    Task<Person?> GetPersonById(long id);

    /// <summary>
    /// Добавляет нового сотрудника в репозиторий.
    /// </summary>
    /// <param name="person"> Сотрудник, который будет добавлен. </param>
    Task AddPerson(Person person);

    /// <summary>
    /// Удаляет сотрудника из репозитория.
    /// </summary>
    /// <param name="person"> Сотрудник, который будет удален. </param>
    Task DeletePerson(Person person);

    /// <summary>
    /// Обновляет данные сотрудника в репозитории.
    /// </summary>
    /// <param name="person"> Сотрудник с обновленными данными. </param>
    Task UpdatePerson(Person person);
}
