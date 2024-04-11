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
    public class ArrivalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArrivalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? idVaultNote, string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            if (idVaultNote == null)
            {
                return NotFound();
            }

            var vaultNote = await _context.VaultNotes
                .Include(v => v.Vault)
                .FirstOrDefaultAsync(v => v.Id == idVaultNote);

            if (vaultNote == null)
            {
                return NotFound();
            }

            int idVault = vaultNote.Vault.Id;
            ViewBag.IdVault = idVault;
            ViewBag.IdVaultNote = idVaultNote;

            // Проверка наличия записи в таблице Arrival для данной VaultNote
            var existingArrivals = await _context.Arrivals
                .Where(a => a.IdVaultNote == vaultNote.Id)
                .ToListAsync();

            if (existingArrivals.Count == 0)
            {
                // Создание записей в таблице Arrival
                var foods = await _context.Foods.ToListAsync();
                foreach (var food in foods)
                {
                    var arrival = new Arrival
                    {
                        Date = vaultNote.Date,
                        IdFood = food.Id,
                        IdVaultNote = vaultNote.Id,
                        FoodCount = 0
                    };

                    _context.Add(arrival);
                }

                await _context.SaveChangesAsync();
            }

            var arrivalsQuery = _context.Arrivals
                .Include(a => a.Food)
                .Where(a => a.IdVaultNote == vaultNote.Id);

            if (!String.IsNullOrEmpty(searchString))
            {
                arrivalsQuery = arrivalsQuery.Where(a => a.Food.NameFood.Contains(searchString));
            }

            var arrivals = await arrivalsQuery
                .OrderBy(a => a.Food.NameFood)
                .ToListAsync();

            return View(arrivals);
        }


        // GET: Arrivals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arrival = await _context.Arrivals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arrival == null)
            {
                return NotFound();
            }

            return View(arrival);
        }

        // GET: Arrivals/Create
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Arrival arrival)
        {
            if (ModelState.IsValid)
            {
                _context.Add(arrival);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(arrival);
        }

        // GET: Arrivals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arrival = await _context.Arrivals.FindAsync(id);
            if (arrival == null)
            {
                return NotFound();
            }
            return View(arrival);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Arrival arrival)
        {
            if (id != arrival.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(arrival);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArrivalExists(arrival.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { idVaultNote = arrival.IdVaultNote });
            }
            return View(arrival);
        }

        // GET: Arrivals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arrival = await _context.Arrivals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arrival == null)
            {
                return NotFound();
            }

            return View(arrival);
        }

        // POST: Arrivals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var arrival = await _context.Arrivals.FindAsync(id);
            _context.Arrivals.Remove(arrival);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArrivalExists(int id)
        {
            return _context.Arrivals.Any(e => e.Id == id);
        }
    }
}
