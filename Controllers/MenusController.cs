using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Diplom.Data;
using Diplom.Models;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using System.IO;
using System.Collections.Generic;

namespace Diplom.Controllers
{
    public class MenusController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public MenusController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public record SelectOptions
        {
            public string Fn { get; set; }
            public string Ln { get; set; }
            public string value { get; set; }
        }

        // GET: Menus
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Menus.Include(m => m.ApplicationUsers);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.ApplicationUsers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menus/Create
        public async Task<IActionResult> Create()
        {
            var roles = await _userManager.GetUsersInRoleAsync("Medic");
            var users = roles
                .Select(s => new SelectOptions
                {
                    value = s.Id,
                    Fn = s.FirstName,
                    Ln = s.LastName
                })
                .ToList();
            TempData["users"] = users;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUser"] = new SelectList(_context.ApplicationUsers, "Id", "Id", menu.IdUser);
            return View(menu);
        }

        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }


            var roles = await _userManager.GetUsersInRoleAsync("Medic");
            var users = roles
                .Select(s => new SelectOptions
                {
                    value = s.Id,
                    Fn = s.FirstName,
                    Ln = s.LastName
                })
                .ToList();
            TempData["users"] = users;
            ViewData["IdUser"] = new SelectList(_context.ApplicationUsers, "Id", "Id", menu.IdUser);
            return View(menu);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Menu menu)
        {
            if (id != menu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                
                if (ModelState.IsValid)
                {
                    _context.Update(menu);

                    
                    var menuFoods = await _context.MenuFoods.Where(mf => mf.MenuId == menu.Id).ToListAsync();
                    foreach (var menuFood in menuFoods)
                    {
                        menuFood.Supply = menuFood.CountPerUnit * menu.ChildCount/1000;
                        _context.Update(menuFood);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["IdUser"] = new SelectList(_context.ApplicationUsers, "Id", "Id", menu.IdUser);
            return View(menu);
        }

        // GET: Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.ApplicationUsers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.Id == id);
        }

        public IActionResult ExportData(Menu menu)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string templatePath = @"C:\Users\Keks\Desktop\Диплом крутой\Diplom\Menu.xlsx"; ;

            using (var package = new ExcelPackage(new FileInfo(templatePath)))
            {
                
                List<MenuFood> menuFoods = GetMenuFoods();
                List<Unit> units = GetUnits();
                List<Meal> meals = GetMeals();
                List<MealTime> mealTimes = GetMealTimes();
                List<Menu> menus = GetMenus();
                List<ApplicationUser> applicationUsers = GetApplicationUsers();
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Меню"];

                //продукты
                int colB = 2;
                int colC = 3;
                int colD = 4;
                int row1 = 18;
                var groupedMenuFoods = menuFoods
                    .GroupBy(mf => new { mf.Name, mf.UnitId })
                    .ToList();

                //блюда
                int startColumn1 = 5;
                int startColumn2 = 11;
                int startColumn3 = 13;
                int startColumn4 = 29;
                int startColumn5 = 33;
                var uniqueMealNames = new Dictionary<int, string>();

                //данные продукта
                int a = 5;
                int aa = 18;
                int b = 6;
                foreach (var menuu in menus)
                {
                    if (menu.Id == menuu.Id)
                    {
                        var startColumn = 5;
                        var endColumn = 42;

                        for (int column = startColumn; column <= endColumn; column += 2)
                        {
                            var cell1 = worksheet.Cells[16, column];
                            var cell2 = worksheet.Cells[16, column + 1];
                            var range = worksheet.Cells[cell1.Address + ":" + cell2.Address];
                            range.Merge = true;
                        }
                        worksheet.Cells["E11:F11"].Merge = true;
                        worksheet.Cells["E11"].Value = menuu.ChildCount;
                        worksheet.Cells["AS17"].Value = menuu.ChildCount;
                        worksheet.Cells["AT17"].Value = menuu.ChildCount;
                        worksheet.Cells["U7:X7"].Merge = true;
                        worksheet.Cells["U7"].Value = menuu.date.Date.ToShortDateString();
                        worksheet.Cells["U9"].Value = menuu.ChildHouse;
                        worksheet.Cells["U10:Y10"].Merge = true;
                        foreach (var users in applicationUsers)
                        {
                            if (users.Id == menuu.IdUser)
                            {
                                worksheet.Cells["U10"].Value = menuu.ApplicationUsers.FirstName + " " + menuu.ApplicationUsers.LastName;
                            }

                        }

                        foreach (var group in groupedMenuFoods)
                        {
                            string menuFoodName = group.Key.Name;
                            int unitId = group.Key.UnitId;

                            MenuFood firstMenuFood = group.First();

                            worksheet.Cells[row1, colB].Value = menuFoodName;
                            worksheet.Cells[row1, colC].Value = firstMenuFood.Code;

                            Unit unit = units.FirstOrDefault(u => u.Id == unitId);
                            if (unit != null)
                            {
                                worksheet.Cells[row1, colD].Value = unit.Name;
                            }

                            row1++;
                        }

                        foreach (var menufood in menuFoods)
                        {
                            foreach (var meal in meals)
                            {
                                foreach (var mealtime in mealTimes)
                                {
                                    if (menufood.MealTimeId == mealtime.Id)
                                    {
                                        if (mealtime.Id == 1)
                                        {
                                            if (meal.Id == menufood.MealId && !uniqueMealNames.ContainsKey(meal.Id))
                                            {
                                                worksheet.Cells[16, startColumn1].Value = meal.Name;
                                                uniqueMealNames.Add(meal.Id, meal.Name);
                                                startColumn1 += 2;
                                            }
                                        }
                                        if (mealtime.Id == 2)
                                        {
                                            if (meal.Id == menufood.MealId && !uniqueMealNames.ContainsKey(meal.Id))
                                            {
                                                worksheet.Cells[16, startColumn2].Value = meal.Name;
                                                uniqueMealNames.Add(meal.Id, meal.Name);
                                            }
                                        }

                                        if (mealtime.Id == 3)
                                        {
                                            if (meal.Id == menufood.MealId && !uniqueMealNames.ContainsKey(meal.Id))
                                            {
                                                worksheet.Cells[16, startColumn3].Value = meal.Name;
                                                uniqueMealNames.Add(meal.Id, meal.Name);
                                                startColumn3 += 2;
                                            }
                                        }

                                        if (mealtime.Id == 4)
                                        {
                                            if (meal.Id == menufood.MealId && !uniqueMealNames.ContainsKey(meal.Id))
                                            {
                                                worksheet.Cells[16, startColumn4].Value = meal.Name;
                                                uniqueMealNames.Add(meal.Id, meal.Name);
                                                startColumn4 += 2;
                                            }
                                        }

                                        if (mealtime.Id == 5)
                                        {
                                            if (meal.Id == menufood.MealId && !uniqueMealNames.ContainsKey(meal.Id))
                                            {
                                                worksheet.Cells[16, startColumn5].Value = meal.Name;
                                                uniqueMealNames.Add(meal.Id, meal.Name);
                                                startColumn5 += 2;
                                            }
                                        }
                                    }
                                    if (menufood.MealId == meal.Id)
                                    {
                                        if (menufood.MealTimeId == mealtime.Id)
                                        {
                                            worksheet.Cells[aa, a].Value = menufood.CountPerUnit;
                                            worksheet.Cells[aa, b].Value = menufood.Supply;
                                        }
                                    }
                                }
                            }
                            a += 2;
                            b += 2;
                        }
                    }
                }
                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
            }
        }
        private List<Menu> GetMenus()
        {
            return _context.Menus.ToList();
        }
        private List<ApplicationUser> GetApplicationUsers()
        {
            return _context.ApplicationUsers.ToList();
        }

        private List<Unit> GetUnits()
        {
            return _context.Units.ToList();
        }
        private List<MenuFood> GetMenuFoods()
        {
            return _context.MenuFoods.ToList();
        }
        private List<Meal> GetMeals()
        {
            return _context.Meals.ToList();
        }
        private List<MealTime> GetMealTimes()
        {
            return _context.MealTimes.ToList();
        }
    }
}
