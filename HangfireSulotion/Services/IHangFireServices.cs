namespace HangfireSulotion.Services
{
    public interface IHangFireServices
    {
        void FireAndForgetJob();
        void ReccuringJob();
        void DelayedJob();
        void ContinuationJob();

    }
}
