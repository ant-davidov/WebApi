namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IAnimalRepository AnimalRepository { get; }
        IAnimalTypeRepository AnimalTypeRepository { get; }
        IAnimalVisitedLocationRepository AnimalVisitedLocationRepository { get; }
        ILocationPointRepository LocationPointRepository { get; }
        IAreaRepository AreaRepository { get; }
        IAnimalsAnalyticsRepository AnimalsAnalyticsRepository { get; }
        IAccountRepository AccountRepository { get; }
        Task<bool> Complete();
    }
}
