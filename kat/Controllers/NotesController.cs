using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using kat.Data;
using kat.Models;
using kat.Code;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authorization;

namespace kat.Controllers
{
    public class NotesController : Controller
    {
        private readonly katContext _context;
        private readonly CryptExample _cryptExample;
        private readonly IDataProtector _dataProtector;


        public NotesController(
            katContext context,
            CryptExample cryptExample,
            IDataProtectionProvider dataProtector)
        {
            _context = context;
            _cryptExample = cryptExample;
            _dataProtector = dataProtector.CreateProtector("notesControllerKey");
        }

        [Authorize("RequireAuthenticatedUser")]
        // GET: Notes
        public async Task<IActionResult> Index()
        {
            var data = await _context.Note.Where(n => n.enteredBy == User.Identity.Name).ToListAsync();

            List<Note> newList = new();

            for (int i = 0; i < data.Count; i++)
            {
                Note newNote = new();

                var decryptedMessage = _cryptExample.Decrypt(data[i].message, _dataProtector);
                newNote.message = decryptedMessage;
                newNote.enteredBy = data[i].enteredBy;

                newList.Add(newNote);
            }

            return View(newList);
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .FirstOrDefaultAsync(m => m.id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,message,enteredBy")] Note note)
        {
            if (ModelState.IsValid)
            {
                string encryptedMessage = _cryptExample.Encrypt(note.message, _dataProtector);

                note.message = encryptedMessage;
                note.enteredBy = User.Identity.Name;

                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,message,enteredBy")] Note note)
        {
            if (id != note.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.id))
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
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .FirstOrDefaultAsync(m => m.id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Note.FindAsync(id);
            _context.Note.Remove(note);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.id == id);
        }
    }
}
