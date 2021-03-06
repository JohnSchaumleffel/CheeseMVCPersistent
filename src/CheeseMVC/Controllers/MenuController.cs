﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            return View(context.Menus.ToList());
        }

        public IActionResult Add()
        {
            return View(new AddMenuViewModel());
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };

                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu?id=" + newMenu.ID);
            }

            return Redirect("/Menu/Add");
        }

        public IActionResult ViewMenu(int id)
        {
            Menu menu = context.Menus.SingleOrDefault(m => m.ID == id);

            if (menu != null)
            {
                List<CheeseMenu> items = context.CheeseMenus.Include(item => item.Cheese).Where(cm => cm.MenuID == id).ToList();

                ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel(menu, items);

                return View(viewMenuViewModel);
            }

            return Redirect("/Menu");
        }

        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus.SingleOrDefault(m => m.ID == id);

            // create new AddMenuItemViewModel
            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu, context.Cheeses);

            return View(addMenuItemViewModel);
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(cm => cm.CheeseID == addMenuItemViewModel.CheeseID)
                    .Where(cm => cm.MenuID == addMenuItemViewModel.MenuID).ToList();

                if (existingItems.Count == 0)
                {
                    CheeseMenu newCheeseMenu = new CheeseMenu
                    {
                        CheeseID = addMenuItemViewModel.CheeseID,
                        MenuID = addMenuItemViewModel.MenuID
                    };

                    context.CheeseMenus.Add(newCheeseMenu);
                    context.SaveChanges();
                }
            }

            return Redirect("/Menu/ViewMenu?id=" + addMenuItemViewModel.MenuID);
        }
    }
}
