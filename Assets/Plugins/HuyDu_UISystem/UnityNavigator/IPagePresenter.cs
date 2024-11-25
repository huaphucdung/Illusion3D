using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Core.Page;
using UnityScreenNavigator.Runtime.Core.Sheet;

namespace HuyDu_UISystem{
    internal interface IPagePresenter : IPresenter, IPageLifecycleEvent{}
    internal interface IModalPresenter : IPresenter, IModalLifecycleEvent{}
    internal interface ISheetPresenter : IPresenter, ISheetLifecycleEvent{}
}