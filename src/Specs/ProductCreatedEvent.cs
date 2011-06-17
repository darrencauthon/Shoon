using System;
using SimpleCqrs.Eventing;

namespace Specs
{
    public class ProductCreatedEvent : DomainEvent
    {
        public string Sku { get; set; }
    }
}