using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HelloMVC.Models;

namespace HelloMVC.ViewModels
{
    public class FuckViewModel
    {
        public List<Fuck> fuckList { get; set; }
    }
}