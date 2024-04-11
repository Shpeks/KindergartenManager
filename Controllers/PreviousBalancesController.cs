using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Diplom.Data;
using Diplom.Models;

namespace Diplom.Controllers
{
    public class PreviousBalancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PreviousBalancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index([FromQuery] string searchString, [FromQuery] int idVaultNote)
        {
            ViewBag.IdVaultNote = idVaultNote;
            var vaultNote = await _context.VaultNotes
                .Include(v => v.Vault)
                .FirstOrDefaultAsync(v => v.Id == idVaultNote);

            var balancesQuery = _context.PreviousBalances
                .Include(a => a.Food)
                .Where(a => a.IdVaultNote == vaultNote.Id);

            if (!string.IsNullOrEmpty(searchString))
            {
                balancesQuery = balancesQuery.Where(a => a.Food.NameFood.Contains(searchString));
            }


            if (vaultNote == null)
            {
                return NotFound();
            }
            int idVault = vaultNote.Vault.Id;
            ViewBag.IdVault = idVault;

            // Проверка наличия записи в таблице Product для данной VaultNote
            var existingBalances = await _context.PreviousBalances
                .Where(a => a.IdVaultNote == vaultNote.Id)
                .ToListAsync();
            
            // Получение списков Arrival и ProductConsumption для данной VaultNote
            var arrivals = await _context.Arrivals
                .Where(a => a.IdVaultNote == vaultNote.Id)
                .ToListAsync();
            var consumptions = await _context.ProductConsumptions
                .Where(c => c.IdVaultNote == vaultNote.Id)
                .ToListAsync();

            var prevVaultNote = await _context.VaultNotes
                .Where(v => v.IdVault == vaultNote.IdVault && v.Date < vaultNote.Date)
                .OrderByDescending(v => v.Date)
                .FirstOrDefaultAsync();
            var prevBalances = await _context.PreviousBalances
                .Where(v => prevVaultNote != null && v.IdVaultNote == prevVaultNote.Id)
                .ToListAsync();

            // Расчет конечного остатка на основе начального остатка, приходов и расходов
            foreach (var balance in existingBalances)
            {
                if (balance.StartBalance == null)
                {
                    balance.StartBalance = 0;
                }
                var prevBalance = prevBalances?.FirstOrDefault(v => v.IdFood == balance.IdFood);
                if (prevBalance != null)
                {
                    balance.StartBalance = prevBalance.EndBalance.Value;
                }
                double originalStartBalance = balance.StartBalance.Value;
                double originalEndBalance = balance.EndBalance.Value;

                double totalArrival = arrivals.Where(a => a.IdFood == balance.IdFood)
                                              .Sum(a => a.FoodCount ?? 0);
                double totalConsumption = consumptions.Where(c => c.IdFood == balance.IdFood)
                                                      .Sum(c => (c.FoodCountChild ?? 0) + (c.FoodCountKid ?? 0));
                double endBalance = (originalStartBalance + totalArrival) - totalConsumption;

                if (prevBalance != null)
                {
                    
                    balance.EndBalance = endBalance;
                    _context.Update(balance);
                }
                else
                {
                    _context.Entry(balance).State = EntityState.Modified;
                    balance.EndBalance = endBalance;
                }
            }
            await _context.SaveChangesAsync();

            var nextVaultNote = await _context.VaultNotes
                .Where(v => v.IdVault == vaultNote.IdVault && v.Date > vaultNote.Date)
                .OrderBy(v => v.Date)
                .FirstOrDefaultAsync();

            var balances = await balancesQuery
                .OrderBy(v => v.Food.NameFood)
                .ToListAsync();

            var balancess = balances.Select(v => new PreviousBalance
            {
                Id = v.Id,
                EndBalance = v.EndBalance,
                IdVaultNote = v.IdVaultNote,
                IdFood = v.IdFood,
                Food = v.Food,
                VaultNote = v.VaultNote,    
                StartBalance = (prevBalances == null ? 0: prevBalances.FirstOrDefault(a => a.IdFood == v.IdFood) != null? prevBalances.FirstOrDefault(a => a.IdFood == v.IdFood).EndBalance.Value : 0)
            })
            .OrderBy(v => v.Food.NameFood)
            .ToList();

            return View(balancess);
        }

        // GET: PreviousBalances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var previousBalance = await _context.PreviousBalances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (previousBalance == null)
            {
                return NotFound();
            }

            return View(previousBalance);
        }

        // GET: PreviousBalances/Create
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PreviousBalance previousBalance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(previousBalance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(previousBalance);
        }

        // GET: PreviousBalances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var previousBalance = await _context.PreviousBalances.FindAsync(id);
            if (previousBalance == null)
            {
                return NotFound();
            }
            return View(previousBalance);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PreviousBalance previousBalance)
        {
            if (id != previousBalance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(previousBalance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreviousBalanceExists(previousBalance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { idVaultNote = previousBalance.IdVaultNote });
            }
            return View(previousBalance);
        }

        // GET: PreviousBalances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var previousBalance = await _context.PreviousBalances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (previousBalance == null)
            {
                return NotFound();
            }

            return View(previousBalance);
        }

        // POST: PreviousBalances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var previousBalance = await _context.PreviousBalances.FindAsync(id);
            _context.PreviousBalances.Remove(previousBalance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PreviousBalanceExists(int id)
        {
            return _context.PreviousBalances.Any(e => e.Id == id);
        }
    }
}
