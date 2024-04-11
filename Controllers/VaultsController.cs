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
    public class VaultsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public VaultsController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        
        public async Task<IActionResult> Index(int idVaults)
        {
            ViewBag.IdVaults = idVaults;
            var vaults = await _context.Vaults.Include(v => v.ApplicationUsers).ToListAsync();
            return View(vaults);
        }
        public record SelectOptions
        {
            public string Fn { get; set; }
            public string Ln { get; set; }
            public string value { get; set; }
        }

        public async Task<IActionResult> Create()
        {
            var roles = await _userManager.GetUsersInRoleAsync("Fabricator");
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
        public async Task<IActionResult> Create(Vault vault)
        {
            if (ModelState.IsValid)
            {
                var minDateRange = 10; // Минимальный отрезок времени (в днях)
                var dateRange = (vault.DateEnd.Date - vault.DateStart.Date).TotalDays;

                if (dateRange <= minDateRange)
                {
                    ModelState.AddModelError(string.Empty, $"Выбранный отрезок времени должен быть не менее {minDateRange} дней или ошибка в выбранных датах.");
                    await SetCreateViewData();
                    return View(vault);
                }

                _context.Add(vault);
                await _context.SaveChangesAsync();

                var currentDate = vault.DateStart;
                var endDate = vault.DateEnd;

                while (currentDate <= endDate)
                {
                    if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        var vaultNote = new VaultNote
                        {
                            Date = currentDate,
                            IdVault = vault.Id
                        };
                        _context.Add(vaultNote);

                        var foods = await _context.Foods.ToListAsync();
                        foreach (var food in foods)
                        {
                            var balances = new PreviousBalance
                            {
                                IdFood = food.Id,
                                VaultNote = vaultNote,
                                StartBalance = 0,
                                EndBalance = 0
                            };

                            _context.Add(balances);
                        }
                    }

                    currentDate = currentDate.AddDays(1);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUser"] = new SelectList(_context.ApplicationUsers, "Id", "Id", vault.IdUser);
            await SetCreateViewData();
            return View(vault);
        }

        // GET: Vaults/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            await SetEditViewData();

            if (id == null)
            {
                return NotFound();
            }

            var vault = await _context.Vaults.FindAsync(id);
            if (vault == null)
            {
                return NotFound();
            }

            return View(vault);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vault vault)
        {
            if (id != vault.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var minDateRange = 10; // Минимальный отрезок времени (в днях)
                var dateRange = (vault.DateEnd.Date - vault.DateStart.Date).TotalDays;

                if (dateRange < minDateRange)
                {
                    ModelState.AddModelError(string.Empty, $"Выбранный отрезок времени должен быть не менее {minDateRange} дней или ошибка в выбранных датах.");
                    await SetEditViewData();
                    return View(vault);
                }

                try
                {
                    _context.Update(vault);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaultExists(vault.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var currentDate = vault.DateStart;
                var endDate = vault.DateEnd;

                while (currentDate <= endDate)
                {
                    if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        var vaultNote = new VaultNote
                        {
                            Date = currentDate,
                            IdVault = vault.Id
                        };

                        _context.Update(vaultNote);
                    }

                    currentDate = currentDate.AddDays(1);
                }

                return RedirectToAction("Index", "Vaults", new { idVaults = id });
            }

            await SetEditViewData();
            return View(vault);
        }

        // GET: Vaults/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vault = await _context.Vaults.Include(v => v.ApplicationUsers).FirstOrDefaultAsync(m => m.Id == id);
            if (vault == null)
            {
                return NotFound();
            }

            return View(vault);
        }

        // POST: Vaults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int idVault)
        {
            ViewBag.IdVault = idVault;
            var vault = await _context.Vaults.FindAsync(id);
            _context.Vaults.Remove(vault);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task SetCreateViewData()
        {
            ViewData["IdUser"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            var fabricatorRoles = await _userManager.GetUsersInRoleAsync("Fabricator");
            var users = fabricatorRoles.Select(s => new SelectOptions { value = s.Id, Fn = s.FirstName, Ln = s.LastName }).ToList();
            TempData["users"] = users;
        }

        private async Task SetEditViewData()
        {
            var fabricatorRoles = await _userManager.GetUsersInRoleAsync("Fabricator");
            var users = fabricatorRoles.Select(s => new SelectOptions { value = s.Id, Fn = s.FirstName, Ln = s.LastName }).ToList();
            TempData["users"] = users;
        }
            
        private bool VaultExists(int id)
        {
            return _context.Vaults.Any(e => e.Id == id);
        }
        public IActionResult ExportData(Vault vault)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string templatePath = @"C:\Users\Keks\Desktop\Диплом крутой\Diplom\Svod.xlsx";

            using (var package = new ExcelPackage(new FileInfo(templatePath)))
            {
                List<VaultNote> vaultNotes = GetVaultNotes();
                
                ExcelWorksheet worksheet = package.Workbook.Worksheets["свод за 10 дней"];
                worksheet.Cells["A1:AA1"].Merge = true;
                
                // Получение данных из модели Food и заполнение столбца A
                List<Food> foods = GetFoods();
                for (int i = 0; i < foods.Count; i++)
                {
                    worksheet.Cells[i + 6, 1].Value = foods[i].NameFood;
                }
                List<Vault> vaults = GetVaults();
                List<ApplicationUser> users = GetApplicationUsers();

                foreach (var vaultt in vaults)
                {
                    if (vaultt.Id == vault.Id)
                    {
                        worksheet.Cells[1, 1].Value = $"Расход продуктов по МДОУ 'Детский сад № 63' с {vaultt.DateStart.Date.ToShortDateString()} по {vaultt.DateEnd.Date.ToShortDateString()}";
                    }
                    foreach (var user in users)
                    {
                        if (vaultt.IdUser == user.Id)
                        {
                            worksheet.Cells["F79:G79"].Merge = true;
                            worksheet.Cells["F79"].Value = user.FirstName + " " + user.LastName;
                        }
                    }
                }
                
                // Получение данных из модели PreviousBalance и заполнение столбца B
                List<PreviousBalance> previousBalances = GetPreviousBalances();
                double? sumbalance = 0;
                int rowB = 6;
                foreach (var vaultnote in vaultNotes)
                {
                    foreach (var food in foods)
                    {
                        foreach (var balance in previousBalances)
                        {
                            if (vaultnote.IdVault == vault.Id)
                            {
                                if (food.Id == balance.IdFood)
                                {
                                    sumbalance += balance.StartBalance;
                                }
                            }
                        }
                        worksheet.Cells[rowB, 2].Value = sumbalance;
                        sumbalance = 0;
                        rowB++;
                    }
                    {
                        break;
                    }
                }

                // Получение данных из модели Arrival и заполнение столбца C
                List<Arrival> arrivals = GetArrivals();
                int rowC = 6;
                double? sumarrival = 0;
                foreach (var vaultnote in vaultNotes)
                {
                    if (vaultnote.IdVault == vault.Id)
                    {
                        foreach (var food in foods)
                        {
                            foreach (var arrival in arrivals)
                            {
                                if (food.Id == arrival.IdFood)
                                {
                                    if (vaultnote.Id == arrival.IdVaultNote)
                                    {
                                        sumarrival += arrival.FoodCount;
                                    }
                                }
                            }
                            worksheet.Cells[rowC, 3].Value = sumarrival;
                            sumarrival = 0;
                            rowC++;
                        }
                        break;
                    }
                }

                // Получение данных VaultNote и заполнение дат в диапазоне D2:W3

                int column = 4;
                foreach (var vaultNote in vaultNotes)
                {
                    if (vaultNote.IdVault == vault.Id)
                    {
                        string dateHeaderText = "Расчет продуктов за " + vaultNote.Date.ToString("dd.MM.yyyy");

                        // Объединяем ячейки
                        worksheet.Cells[2, column, 3, column + 1].Merge = true;

                        // Устанавливаем значение объединенной ячейки
                        worksheet.Cells[2, column].Value = dateHeaderText;

                        column += 2; // Увеличиваем column на 2, чтобы перейти к следующей паре объединенных ячеек
                    }
                }

                // Заполнение KidCount и ChildCount для каждого дня в диапазоне D5:W5
                int row = 5;
                int col = 4;
                foreach (var vaultNote in vaultNotes)
                {
                    if (vaultNote.IdVault == vault.Id)
                    {
                        worksheet.Cells[row, col].Value = vaultNote.KidCount;
                        worksheet.Cells[row, col + 1].Value = vaultNote.ChildCount;
                        col += 2;
                    }
                }

                // Получение данных из модели ProductConsumption и заполнение соответствующих столбцов
                List<ProductConsumption> productConsumptions = GetProductConsumptions();
                row = 5;
                column = 4;

                foreach (var food in foods)
                {
                    row++;
                    column = 4;
                    foreach (var vaultnote in vaultNotes)
                    {
                        if (vaultnote.IdVault == vault.Id)
                        {
                            foreach (var productConsumption in productConsumptions)
                            {
                                if (food.Id == productConsumption.IdFood && vaultnote.Date == productConsumption.Date && vaultnote.Id == productConsumption.IdVaultNote)
                                {

                                    worksheet.Cells[row, column].Value = productConsumption.FoodCountKid;
                                    worksheet.Cells[row, column + 1].Value = productConsumption.FoodCountChild;
                                    column += 2;
                                }
                            }
                        }
                    }

                }

                // Заполнение суммы KidCount и ChildCount за все дни в ячейки X5 и Y5
                int totalKidCount = vaultNotes.Sum(v => v.KidCount);
                int totalChildCount = vaultNotes.Sum(v => v.ChildCount);
                worksheet.Cells[5, 24].Value = totalKidCount;
                worksheet.Cells[5, 25].Value = totalChildCount;

                // Заполнение суммы FoodCountKid и FoodCountChild каждого продукта в столбцах X6:X76 и Y6:Y76
                int rowXY = 6;
                double? sumkid = 0;
                double? sumchild = 0;
                foreach (var vaultnote in vaultNotes)
                {
                    if (vaultnote.IdVault == vault.Id)
                    {
                        foreach (var food in foods)
                        {
                            foreach (var product in productConsumptions)
                            {
                                if (food.Id == product.IdFood)
                                {
                                    sumkid += product.FoodCountKid;
                                    sumchild += product.FoodCountChild;

                                }
                            }
                            worksheet.Cells[rowXY, 24].Value = sumkid;
                            worksheet.Cells[rowXY, 25].Value = sumchild;
                            sumkid = 0;
                            sumchild = 0;
                            rowXY++;
                        }
                    }
                    break;
                }

                // Заполнение суммы FoodCountKid и FoodCountChild каждого продукта в столбце Z6:Z76
                int rowZ = 6;
                double? sumFoodCFoodK = 0;
                foreach (var vaultnote in vaultNotes)
                {
                    if (vaultnote.IdVault == vault.Id)
                    {
                        foreach (var food in foods)
                        {
                            foreach (var product in productConsumptions)
                            {
                                if (product.IdFood == food.Id)
                                {
                                    sumFoodCFoodK += product.FoodCountChild;
                                    sumFoodCFoodK += product.FoodCountKid;
                                }
                            }
                            worksheet.Cells[rowZ, 26].Value = sumFoodCFoodK;
                            sumFoodCFoodK = 0;
                            rowZ++;
                        }
                        break;
                    }
                }

                // Заполнение данных из модели PreviousBalance в столбце AA6:AA76
                row = 6;
                double? sumbalanceend = 0;
                foreach (var vaultnote in vaultNotes)
                {
                    foreach (var food in foods)
                    {
                        if (vaultnote.IdVault == vault.Id)
                        {
                            foreach (var balance in previousBalances)
                            {
                                if (food.Id == balance.IdFood)
                                {
                                    
                                    sumbalanceend += balance.StartBalance;
                                }
                            }
                            foreach (var arrival in arrivals)
                            {
                                if (food.Id == arrival.IdFood)
                                {
                                    sumbalanceend += arrival.FoodCount;
                                }
                            }
                            foreach (var product in productConsumptions)
                            {
                                if (food.Id == product.IdFood)
                                {
                                    sumbalanceend -= product.FoodCountChild;
                                    sumbalanceend -= product.FoodCountKid;
                                }
                            }
                        }

                        worksheet.Cells[row, 27].Value = sumbalanceend;
                        sumbalanceend = 0;
                        row++;
                    }
                    break;
                }


                // Сохранение и возврат файла для скачивания
                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
            }
        }

        // Методы для получения данных из базы данных
        private List<Vault> GetVaults()
        {
            return _context.Vaults.ToList();
        }
        private List<Food> GetFoods()
        {
            return _context.Foods.ToList();
        }

        private List<PreviousBalance> GetPreviousBalances()
        {
            return _context.PreviousBalances.ToList();
        }

        private List<Arrival> GetArrivals()
        {
            return _context.Arrivals.ToList();
        }

        private List<VaultNote> GetVaultNotes()
        {
            return _context.VaultNotes.ToList();
        }

        private List<ProductConsumption> GetProductConsumptions()
        {
            return _context.ProductConsumptions.ToList();
        }
        private List<ApplicationUser> GetApplicationUsers()
        {
            return _context.ApplicationUsers.ToList();
        }
    }
}