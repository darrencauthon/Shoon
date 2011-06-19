Shoon
===========
# What is it?
Shoon is a simple SQL Server denormalizer for SimpleCQRS.  You can use it to quickly build view models from events that happen in your system.

# How do you use it?

1.)  Add a reference to Shoon.dll in your SimpleCQRS application.  You'll also need to add references to Simple.Data (NuGet package coming with SimpleCQRS soon).

2.)  Create an implementation of IConnectionStringRetriever that returns a connection string to your database.  Register that retriever into your SimpleCQRS service locator.

3.)  Create a table that will serve as your view model table, and set its identity to be a uniqueidentifier "Id".

3.)  Create a denormalizer named "[YOUR TABLE NAME]Denormalizer".  Make it inherit from SqlDenormalizer.

4.)  For each handled event, pass the domain event to either:  Insert, Update, Delete, or Upsert.

# Example

    public class AccountDenormalizer : SqlDenormalizer,
                                       IHandleDomainEvents<AccountCreatedEvent>,
                                       IHandleDomainEvents<AccountNameSetEvent>,
                                       IHandleDomainEvents<AccountEmailSetEvent>
    {

        public void Handle(AccountCreatedEvent domainEvent)
        {
            Insert(domainEvent);
        }

        public void Handle(AccountNameSetEvent domainEvent)
        {
            Update(domainEvent);
        }

        public void Handle(AccountEmailSetEvent domainEvent)
        {
            Update(domainEvent);
        }
    }

As accounts are created and data set, it will build a table like this:

| Id     | FirstName | LastName | Email            |
| GUID 1 | Howard    | Roark    | howard@roark.com |
| GUID 2 | John      | Galt     | john@galt.com    |