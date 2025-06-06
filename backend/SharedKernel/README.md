# Shared Kernel

This project contains the shared domain model, base classes, and interfaces used across the LastLink backend.

## Overview

The Shared Kernel contains the fundamental building blocks for implementing Domain-Driven Design (DDD) in the LastLink application. It includes base classes for entities, value objects, aggregate roots, and repositories, as well as common patterns like the Unit of Work and Repository patterns.

## Key Components

### Domain Primitives

- **EntityBase**: Base class for all domain entities.
- **AggregateRoot**: Base class for aggregate roots in the domain model.
- **ValueObject**: Base class for value objects in the domain model.
- **IAuditableEntity**: Interface for entities that track creation and modification information.
- **IDomainEvent**: Interface for domain events.

### Repository Pattern

- **IRepository<T>**: Generic repository interface for data access.
- **IUnitOfWork**: Interface for the unit of work pattern to manage transactions.

### Result Pattern

- **Result**: Represents the result of an operation that does not return a value.
- **Result<T>**: Represents the result of an operation that returns a value.
- **Error**: Represents an error in the domain.

## Usage

### Defining an Entity

```csharp
public class Product : AggregateRoot, IAuditableEntity
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }

    private Product() { }

    public Product(Guid id, string name, decimal price)
        : base(id)
    {
        Name = name;
        Price = price;
    }
}
```

### Defining a Value Object

```csharp
public class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string PostalCode { get; }

    public Address(string street, string city, string postalCode)
    {
        Street = street;
        City = city;
        PostalCode = postalCode;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return PostalCode;
    }
}
```

### Using the Result Pattern

```csharp
public Result<Order> PlaceOrder(Customer customer, Product product, int quantity)
{
    if (quantity <= 0)
    {
        return Result.Failure<Order>("Order.InvalidQuantity", "Quantity must be greater than zero.");
    }

    var order = new Order(Guid.NewGuid(), customer.Id, product.Id, quantity);
    return Result.Success(order);
}
```

## Dependencies

- .NET 9.0
- Microsoft.EntityFrameworkCore
- System.ComponentModel.Annotations

## License

This project is licensed under the MIT License.
