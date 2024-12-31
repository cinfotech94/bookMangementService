public class User
{
    public Guid id{get;set;}
    public string name{get;set;}
    public string email{get;set;}
    public string role{get;set;}
    public long phoneNumber{get;set;}
    public string address{get;set;}
    public string city{get;set;}
    public string state{get;set;}
    public string country{get;set;}
    public string balance{get;set;}
    public string password{get;set;}

}
public class cart{
    public string username{get;set;}
    public string bookId{get;set;}
}
public string CreditHistory{
    public Guid transactionReference{get;set;}
    public double amount{get;set;}
    public DateTime timeCredit{get;set;}

}
public string DebitHistory{
    public Guid transactionReference{get;set;}
    public double amount{get;set;}
    public DateTime timeCredit{get;set;}
    
}