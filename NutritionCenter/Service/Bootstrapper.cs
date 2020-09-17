using NatureBox.Customers.Service;
using NatureBox.DataAccess;
using NatureBox.Partners.Service;
using NatureBox.Products.Service;
using NatureBox.Transactions.Service;
using Prism.Events;
using Unity;
using Unity.Lifetime;

namespace NatureBox.Service
{
    public static class ContainerHelper
    {
        static ContainerHelper()
        {
            Container = new UnityContainer();

            Container.RegisterType<IPartnerRepository, EmployeeRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ICustomerRepository, CustomersRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IProductRepository, ProductRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ICustomerPaymentRepository, PaymentRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IPartnerPaymentRepository, PartnerPaymentRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IInvoiceRepository, InvoiceRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IHealthRecordRepository, HealthRecordRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IBackUpAndRestoreRepository, BackUpAndRestoreRepository>(new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer Container { get; private set; }
    }
}
