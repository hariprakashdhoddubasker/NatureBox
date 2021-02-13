using NatureBox.Model;
using Prism.Events;
using System.Collections.Generic;

namespace NatureBox.Event
{
    public class NotifyBirthDayAlerts : PubSubEvent<List<Customer>>
    {
    }
}
