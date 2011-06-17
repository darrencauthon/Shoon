using System;
using SimpleCqrs.Eventing;

namespace Specs
{
    public class ProductNameSetEvent : DomainEvent
    {
        public string Name { get; set; }
    }
}