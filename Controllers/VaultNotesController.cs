using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Diplom.Data;
using Diplom.Models;

namespace Diplom.Controllers
{
    public class VaultNotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VaultNotesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? idVault)
        {
            ViewBag.IdVault = idVault;
            var vaultNotes = _context.VaultNotes
                .Where(v => v.IdVault == idVault)
                
                .ToList();
            return View(vaultNotes);
        }

        public IActionResult Create(int idVault)
        {
            ViewBag.IdVault = idVault;
            var vaultNote = new VaultNote();
            vaultNote.IdVault = idVault; 
            return View(vaultNote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VaultNote vaultNote, int idVault)
        {
            
            vaultNote.IdVault = idVault;
            if (ModelState.IsValid)
            {
   
                _context.Add(vaultNote);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "VaultNotes", new { idVault = idVault });
            }

            return View(vaultNote);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaultNote = await _context.VaultNotes.FindAsync(id);
            if (vaultNote == null)
            {
                return NotFound();
            }
            ViewData["IdVault"] = new SelectList(_context.Vaults, "IdVault", "IdVault", vaultNote.IdVault);
            return View(vaultNote);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VaultNote vaultNote, int IdVault)
        {
            if (id != vaultNote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vaultNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaultNoteExists(vaultNote.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "VaultNotes", new { IdVault = IdVault });
            }
            ViewData["IdVault"] = new SelectList(_context.Vaults, "IdVault", "IdVault", vaultNote.IdVault);
            return View(vaultNote);
        }

        
        public async Task<IActionResult> Delete(int? id, int idVault)
        {
            ViewBag.id = idVault;
            if (id == null)
            {
                return NotFound();
            }

            var vaultNote = await _context.VaultNotes
                .Include(v => v.Vault)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vaultNote == null)
            {
                return NotFound();
            }

            return View(vaultNote);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int idVault)
        {
            ViewBag.id = idVault;
            var vaultNote = await _context.VaultNotes.FindAsync(id);
            _context.VaultNotes.Remove(vaultNote);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "VaultNotes", new { IdVault = idVault });
        }

        private bool VaultNoteExists(int id)
        {
            return _context.VaultNotes.Any(e => e.Id == id);
        }

        
    }
}
