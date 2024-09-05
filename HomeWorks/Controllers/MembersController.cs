using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeWorks.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using HomeWorks.Configuration;

namespace HomeWorks.Controllers
{
    public class MembersController : Controller
    {
        private readonly RS0605Context _context;
        private readonly IMailService _mailService;

        public MembersController(RS0605Context context , IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _context.Members.ToListAsync());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MeId == id);
            if (member == null)
            {
                return NotFound();
            }



            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MeId,MeName,MeTel,MeEmail,MePassword,PreId")] Member member)
        {

                member.MeId = fairy.GenerateRandomId(8, 12); // 隨機生成8到12位的帳號
                member.PreId = "A"; // 設置固定的 PreId

                _context.Add(member);
                await _context.SaveChangesAsync();


            var mailData = new MailData
            {
                EmailToId = member.MeEmail,
                EmailToName = member.MeName,
                EmailSubject = "您已成功註冊!!!",
                EmailBody = $" {member.MeName} 您好 ,<br>您的帳號已成功建立。您的新帳號為 {member.MeId} ，請務必保管好自己的帳號。"
            };

            // 發送電子郵件
            var emailSent = _mailService.SendMail(mailData);

            if (emailSent)
            {
                // 處理成功邏輯
                return RedirectToAction("Index", "LogIn");
            }
            else
            {
                // 處理失敗邏輯
                ModelState.AddModelError("", "Failed to send email.");
            }

            return RedirectToAction("Index", "LogIn");

        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MeId,MeName,MeTel,MeEmail,MePassword,PreId")] Member member)
        {
            if (id != member.MeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MeId))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MeId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(string id)
        {
            return _context.Members.Any(e => e.MeId == id);
        }
    }
}
