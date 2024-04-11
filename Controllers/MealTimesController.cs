using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Diplom.Data;
using Diplom.Models;

namespace Diplom.Controllers
{
    public class MealTimesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MealTimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MealTimes
        public async Task<IActionResult> Index()
        {
            return View(await _context.MealTimes.ToListAsync());
        }

        // GET: MealTimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealTime = await _context.MealTimes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mealTime == null)
            {
                return NotFound();
            }

            return View(mealTime);
        }

        // GET: MealTimes/Create
        public IActionResult Create(int IdMenu)
        {
            ViewBag.IdMenu = IdMenu;
            return View(new MenuFood { MenuId = IdMenu });

        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MealTime mealTime, MenuFood menuFood, int IdMenu)
        {
            menuFood.MenuId = IdMenu;
            if (ModelState.IsValid)
            {
                _context.Add(mealTime);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "MenuFoods", new { IdMenu = IdMenu });
            }
            return View(menuFood);
        }

        // GET: MealTimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealTime = await _context.MealTimes.FindAsync(id);
            if (mealTime == null)
            {
                return NotFound();
            }
            return View(mealTime);
        }

        // POST: MealTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] MealTime mealTime)
        {
            if (id != mealTime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mealTime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MealTimeExists(mealTime.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mealTime);
        }

        // GET: MealTimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealTime = await _context.MealTimes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mealTime == null)
            {
                return NotFound();
            }

            return View(mealTime);
        }

        // POST: MealTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mealTime = await _context.MealTimes.FindAsync(id);
            _context.MealTimes.Remove(mealTime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MealTimeExists(int id)
        {
            return _context.MealTimes.Any(e => e.Id == id);
        }
    }
}
