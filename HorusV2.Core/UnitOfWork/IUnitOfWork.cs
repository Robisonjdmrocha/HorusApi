namespace HorusV2.Core.UnitOfWork;

public interface IUnitOfWork
{
    void Begin();
    void Commit();
    void Rollback();
}