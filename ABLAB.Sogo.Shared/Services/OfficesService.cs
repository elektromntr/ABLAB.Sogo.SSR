namespace ABLAB.Sogo.Shared.Services;

public class OfficesService
{
    public Office GetOffice(int investmentId)
    {
        Office office = investmentId switch
        {
            1 => new("Osiedle Piastowskie 1, 71-064 Szczecin",
                new Contact("123456789", "ema11l@dokogos.pl"),
                new Contact("123456222", "ema22l@dokogos.pl")),
            2 => new("Tadeusza Kościuszki 7, 88-666 Hel",
                new Contact("123456789", "ema1l@dokogos.pl"),
                new Contact("222333444", "ema2l@dokogos.pl"),
                new Contact("345234123", "ema3l@dokogos.pl")),
            _ => new("Mariana Krzakleszcza 3, 33-345 Dest",
                new Contact("987654322", "em2ail@dokogos.pl"),
                new Contact("987654322", "em7ail@dokogos.pl")),
        };
        return office;
    }
}

public class Office
{
    public string Address { get; set; } = string.Empty;
    public Contact[] Contacts { get; set; }

    public Office(string address, params Contact[] contacts)
    {
        Address = address;
        Contacts = contacts;
    }
}

public class Contact
{
    public string Phone { get; set; }
    public string Email { get; set; }

    public Contact(string phone, string email)
    {
        Email = email;
        Phone = phone;
    }
}
