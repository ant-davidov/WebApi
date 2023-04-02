namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IAnimalRepository AnimalRepository { get; }
        IAnimalTypeRepository AnimalTypeRepository { get; }
        IAnimalVisitedLocationRepository AnimalVisitedLocationRepository { get; }
        ILocationPointRepository LocationPointRepository { get; }
        Task<bool> Complete();
    }
}
