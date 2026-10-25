namespace FrameRent.Domain.Entities;

public class Customer : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PhoneNumber { get; private set; } = string.Empty;

    public Customer(
        string firstName,
        string lastName,
        string email,
        string phoneNumber)
    {
        ValidateAndSet(firstName, lastName, email, phoneNumber);
    }

    public Customer(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string phoneNumber)
    {
        Id = id;
        ValidateAndSet(firstName, lastName, email, phoneNumber);
    }

    private Customer()
    {
    }

    private void ValidateAndSet(
        string firstName,
        string lastName,
        string email,
        string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("El nombre es obligatorio.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("El apellido es obligatorio.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El correo electrónico es obligatorio.", nameof(email));

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = email.Trim().ToLowerInvariant();
        PhoneNumber = phoneNumber?.Trim() ?? string.Empty;
    }

    public void UpdatePersonalInformation(
        string firstName,
        string lastName,
        string email,
        string phoneNumber)
    {
        ValidateAndSet(firstName, lastName, email, phoneNumber);
    }
}