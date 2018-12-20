using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Models;

namespace CheeseMVC.ViewModels
{
    public class ViewMenuViewModel
    {
        public Menu Menu { get; set; }

        public List<CheeseMenu> Items = new List<CheeseMenu>();

        public ViewMenuViewModel() { }

        public ViewMenuViewModel(Menu menu, List<CheeseMenu> cheeseMenus)
        {
            Menu = menu;

            foreach (CheeseMenu cheeseMenu in cheeseMenus)
            {
                Items.Add(cheeseMenu);
            }
        }
    }
}
