namespace testapi.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Contacts
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int contact_id { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string email { get; set; }
    public int phone_number { get; set; }
}

