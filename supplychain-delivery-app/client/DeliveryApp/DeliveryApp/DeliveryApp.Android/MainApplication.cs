using Android.App;
using Android.Runtime;
using Plugin.CurrentActivity;
using System;
using GalaSoft.MvvmLight.Ioc;
using DeliveryApp.ViewModel;

namespace DeliveryApp.Droid
{
#if DEBUG
    [Application(Debuggable = true)]
#else
[Application(Debuggable = false)]
#endif
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);
            SimpleIoc.Default.Register<ISessionState, SessionState>();
            SimpleIoc.Default.Register<DeliveryDetailViewModel>();
            SimpleIoc.Default.Register<DeliveryPageMasterViewModel>();
            SimpleIoc.Default.Register<DeliveryDetailViewModel>();
        }
    }
}