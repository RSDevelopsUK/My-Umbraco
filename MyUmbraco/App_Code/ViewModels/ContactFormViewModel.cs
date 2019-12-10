using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MyUmbraco.DataAnnotations;

namespace MyUmbraco.ViewModels
{
  public class ContactFormViewModel
  {
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Message { get; set; }
  }
}