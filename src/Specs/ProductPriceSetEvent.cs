using System;
using SimpleCqrs.Eventing;

namespace Specs
{
    public class ProductPriceSetEvent : DomainEvent
    {
        public int Price { get; set; }
    }
}