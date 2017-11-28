using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

using Sealegs.Clients.Portable.ViewModel;

namespace Sealegs.Clients.Portable.Locator
{
    public class ViewModelLocator
    {
        #region Pages

        public const string LoginPage = "LoginPage";
        public const string Master = "Master";
        public const string Locker = "Locker";
        public const string LockerDetails = "LockerDetails";

        public const string AddEditEventPage = "AddEditEventPage";
        public const string AddEditNews = "AddEditNews";
        public const string AddEditNotifications = "AddEditNotifications";
        public const string AddEmployee = "AddEmployee";
        public const string AddWine = "AddWinePage";
        public const string CheckOutFinal = "CheckOutFinal";
        public const string CheckOut = "CheckOut";
        public const string Employees = "Employees";
        public const string Evaluations = "Evaluations";
        public const string EventDetails = "EventDetails";
        public const string Events = "Events";
        public const string Feedback = "Feedback";
        public const string Feed = "Feed";
        public const string NewsDetails = "NewsDetails";
        public const string News = "News";
        public const string Notifications = "Notifications";
        public const string Settings = "Settings";
        public const string Signature = "Signature";
        public const string WineCarousel = "WineCarousel";
        public const string WineCardView = "WineCardViewPage";
        public const string WineDetail = "WineDetailsPopup";
        public const string ContactUs = "ContactUs";
        public const string Profile = "Profile";

        public const string AddEditLocker = "AddEditLocker";
        public const string AboutUs = "AboutUs";
        public const string Wine = "Wine";
        public const string NewsManagment = "NewsManagment";
        public const string EventsManagment = "EventsManagment";
        public const string RemoteChillRequest = "RemoteChillRequest";

        public const string NotificationManagment = "NotificationManagment";
        public const string FingerPrintScan = "FingerPrintScan";
        public const string RatingBarPopUp = "RatingBarPopUp";

        public const string WineEvaluation = "WineEvaluation";


        #endregion

        #region CTOR

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            #region PagesViewModels

            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<MasterViewModel>();
            SimpleIoc.Default.Register<LockersViewModel>();
            SimpleIoc.Default.Register<LockerDetailsViewModel>();

            SimpleIoc.Default.Register<AddEditEventPageViewModel>();
            SimpleIoc.Default.Register<AddEditNewsViewModel>();
            SimpleIoc.Default.Register<AddEditNotificationsViewModel>();
            SimpleIoc.Default.Register<AddEmployeeViewModel>();
            SimpleIoc.Default.Register<AddWineViewModel>();

            SimpleIoc.Default.Register<CheckOutFinalViewModel>();
            SimpleIoc.Default.Register<CheckOutViewModel>();
            SimpleIoc.Default.Register<EmployeesViewModel>();
            SimpleIoc.Default.Register<EvaluationsViewModel>();
            SimpleIoc.Default.Register<EventDetailsViewModel>();
            SimpleIoc.Default.Register<EventsViewModel>();

            SimpleIoc.Default.Register<FeedbackViewModel>();
            SimpleIoc.Default.Register<FeedViewModel>();

            SimpleIoc.Default.Register<NewsDetailsViewModel>();
            SimpleIoc.Default.Register<NewsViewModel>();
            SimpleIoc.Default.Register<NotificationsViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<SignatureViewModel>();
            SimpleIoc.Default.Register<WineCarouselViewModel>();
            SimpleIoc.Default.Register<WineDetailViewModel>();
            SimpleIoc.Default.Register<ContactUsViewModel>();
            SimpleIoc.Default.Register<ProfileViewModel>();
            SimpleIoc.Default.Register<AddEditLockersViewModel>();
            SimpleIoc.Default.Register<AboutUsViewModel>();
            SimpleIoc.Default.Register<WinesViewModel>();
            SimpleIoc.Default.Register<NewsManagmentViewModel>();
            SimpleIoc.Default.Register<EventManagementViewModel>();
            SimpleIoc.Default.Register<RemoteChillRequestViewModel>();
            SimpleIoc.Default.Register<NotificationsManagmentViewModel>();

            SimpleIoc.Default.Register<RatingBarPopUpViewModel>();
            SimpleIoc.Default.Register<WinesCardVieModel>();

            SimpleIoc.Default.Register<WineEvaluationViewModel>();

            #endregion
        }

        #endregion

        #region ViewModel Instances

        public LoginViewModel LoginViewModel => ServiceLocator.Current.GetInstance<LoginViewModel>();
        public MasterViewModel MasterViewModel => ServiceLocator.Current.GetInstance<MasterViewModel>();
        public LockersViewModel LockersViewModel => ServiceLocator.Current.GetInstance<LockersViewModel>();
        public LockerDetailsViewModel LockerDetailsViewModel => ServiceLocator.Current.GetInstance<LockerDetailsViewModel>();
        public AddEditEventPageViewModel AddEditEventPageViewModel => ServiceLocator.Current.GetInstance<AddEditEventPageViewModel>();
        public AddEditNewsViewModel AddEditNewsViewModel => ServiceLocator.Current.GetInstance<AddEditNewsViewModel>();
        public AddEditNotificationsViewModel AddEditNotificationsViewModel => ServiceLocator.Current.GetInstance<AddEditNotificationsViewModel>();
        public AddEmployeeViewModel AddEmployeeViewModel => ServiceLocator.Current.GetInstance<AddEmployeeViewModel>();
        public AddWineViewModel AddWineViewModel => ServiceLocator.Current.GetInstance<AddWineViewModel>();
        public CheckOutFinalViewModel CheckOutFinalViewModel => ServiceLocator.Current.GetInstance<CheckOutFinalViewModel>();
        public CheckOutViewModel CheckOutViewModel => ServiceLocator.Current.GetInstance<CheckOutViewModel>();
        public EmployeesViewModel EmployeesViewModel => ServiceLocator.Current.GetInstance<EmployeesViewModel>();
        public EvaluationsViewModel EvaluationsViewModel => ServiceLocator.Current.GetInstance<EvaluationsViewModel>();
        public EventDetailsViewModel EventDetailsViewModel => ServiceLocator.Current.GetInstance<EventDetailsViewModel>();
        public EventsViewModel EventsViewModel => ServiceLocator.Current.GetInstance<EventsViewModel>();
        public FeedbackViewModel FeedbackViewModel => ServiceLocator.Current.GetInstance<FeedbackViewModel>();
        public FeedViewModel FeedViewModel => ServiceLocator.Current.GetInstance<FeedViewModel>();
        public NewsDetailsViewModel NewsDetailsViewModel => ServiceLocator.Current.GetInstance<NewsDetailsViewModel>();
        public NewsViewModel NewsViewModel => ServiceLocator.Current.GetInstance<NewsViewModel>();
        public NotificationsViewModel NotificationsViewModel => ServiceLocator.Current.GetInstance<NotificationsViewModel>();
        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();
        public SignatureViewModel SignatureViewModel => ServiceLocator.Current.GetInstance<SignatureViewModel>();
        public WineCarouselViewModel WineCarouselViewModel => ServiceLocator.Current.GetInstance<WineCarouselViewModel>();
        public WineDetailViewModel WineDetailViewModel => ServiceLocator.Current.GetInstance<WineDetailViewModel>();
        public ContactUsViewModel ContactUsViewModel => ServiceLocator.Current.GetInstance<ContactUsViewModel>();
        public ProfileViewModel ProfileViewModel => ServiceLocator.Current.GetInstance<ProfileViewModel>();
        public AddEditLockersViewModel AddEditLockersViewModel => ServiceLocator.Current.GetInstance<AddEditLockersViewModel>();
        public AboutUsViewModel AboutUsViewModel => ServiceLocator.Current.GetInstance<AboutUsViewModel>();
        public WinesViewModel WinesViewModel => ServiceLocator.Current.GetInstance<WinesViewModel>();
        public NewsManagmentViewModel NewsManagmentViewModel => ServiceLocator.Current.GetInstance<NewsManagmentViewModel>();
        public EventManagementViewModel EventManagementViewModel => ServiceLocator.Current.GetInstance<EventManagementViewModel>();
        public RemoteChillRequestViewModel RemoteChillRequestViewModel => ServiceLocator.Current.GetInstance<RemoteChillRequestViewModel>();
        public NotificationsManagmentViewModel NotificationsManagmentViewModel => ServiceLocator.Current.GetInstance<NotificationsManagmentViewModel>();
        public RatingBarPopUpViewModel RatingBarPopUpViewModel => ServiceLocator.Current.GetInstance<RatingBarPopUpViewModel>();
        public WinesCardVieModel WinesCardViewPageViewModel => ServiceLocator.Current.GetInstance<WinesCardVieModel>();
        public WineEvaluationViewModel WineEvaluationViewModel => ServiceLocator.Current.GetInstance<WineEvaluationViewModel>();

        #endregion
    }
}
