namespace WebApi.Interfaces
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        IAnimalRepository AnimalRepository { get; }
        IAnimalTypeRepository AnimalTypeRepository { get; }
        IAnimalVisitedLocationRepository AnimalVisitedLocationRepository { get; }
        ILocationPointRepository LocationPointRepository { get; }
        Task<bool> Complete();
    }
}
