
using System.ComponentModel;

namespace NatureBox.Service
{
    public enum NatureBoxRoles
    {
        Admin,
        Parnter
    }

    public enum NatureBoxForms
    {
        [Description("Customer")]
        Customer,
        [Description("Partner")]
        Partner,
        [Description("Product")]
        Product,
        [Description("Health Record")]
        HealthRecord,
        [Description("Customer Payment")]
        CustomerPayment,
        [Description("Parnter Settlement")]
        ParterSettlement,
        [Description("Invoice")]
        Invoice,
        [Description("Customer Invoice")]
        CustomerInvoiceReport,
        [Description("Partner Invoice")]
        PartnerInvoiceReport,
        [Description("Customer Payment Report")]
        CustomerPaymentReport,
        [Description("Parnter Settlement Report")]
        ParnterSettlementReport,
        [Description("BackUp")]
        BackUp
    }
}
