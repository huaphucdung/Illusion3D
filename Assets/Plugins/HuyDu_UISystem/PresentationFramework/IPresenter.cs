namespace HuyDu_UISystem
{
    internal interface IPresenter{
        bool IsDisposed { get; }
        bool IsInitialized { get; }
        void Initialize();
    }
}